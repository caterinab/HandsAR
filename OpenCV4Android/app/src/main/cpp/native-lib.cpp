#include "stdafx.h"
#include <opencv2/opencv.hpp>

using namespace cv;
using std::cout;
using std::cin;
using std::vector;

static uchar DT_index2code(int dim, int ind)
{
	return((dim - 1) * 2 + ((ind > 0) ? 1 : 2));
}


void DT_code2index(int code, int *dim, int *index)
{
	*dim = (code + 1) / 2;
	*index = (code % 2 == 0) ? -1 : 1;
}

static void dist_transf_1D_L1NormRet(double *line, uchar *path, int dim, int len)
{
	int q;

	for (q = 1; q < len; q++)
		if (line[q - 1] + 1 < line[q])
		{
			line[q] = line[q - 1] + 1;
			path[q] = DT_index2code(dim, -1);
		}

	for (q = len - 2; q >= 0; q--)
		if (line[q + 1] + 1 < line[q])
		{
			line[q] = line[q + 1] + 1;
			path[q] = DT_index2code(dim, 1);
		}

	return;
}

static void dist_transform_1D_L1NormRet(double *line, uchar *path, int dim, int len, int step)
{
	int q, ind;

	if (step == 1)
	{
		dist_transf_1D_L1NormRet(line, path, dim, len);
		return;
	}

	for (q = 1, ind = step; q < len; q++, ind += step)
		if (line[ind - step] + 1 < line[ind])
		{
			line[ind] = line[ind - step] + 1;
			path[ind] = DT_index2code(dim, -1);
		}

	for (q = len - 2, ind = (len - 2)*step; q >= 0; q--, ind -= step)
		if (line[ind + step] + 1 < line[ind])
		{
			line[ind] = line[ind + step] + 1;
			path[ind] = DT_index2code(dim, 1);
		}

	return;
}

void DistanceTransform2D_NormL1Ret(double *map, int rows, int cols, double *dt, uchar *path)
{
	//double *dt = new double[rows*cols];
	memcpy((char *)dt, (char *)map, rows * cols * sizeof(double));

	//uchar *path = new uchar[rows*cols];

	memset((char *)path, 0, rows * cols * sizeof(uchar));
	int r, c;
	//GMSet(0.0, path);

	for (r = 0; r < rows; r++)
		dist_transform_1D_L1NormRet(&(dt[r*cols]), &(path[r*cols]), 1, cols, 1);

	for (c = 0; c < cols; c++)
		dist_transform_1D_L1NormRet(&(dt[c]), &(path[c]), 2, rows, cols);

	//DT = dt;
	//PATH = path;

	return;
}

extern "C" void DetectSkin(int h, int w, uchar** input_frame, uchar** hands, uchar** cubes) {
	Mat3b skin;
	Mat1b output_frame;

	skin = Mat(h, w, CV_8UC3, *input_frame);
	resize(skin, skin, Size(), 0.5, 0.5, INTER_LINEAR);
    flip(skin, skin, 0);

	//cvtColor(frame, output_frame, CV_BGR2GRAY);
	cvtColor(skin, skin, CV_RGB2HSV);
	//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
	//GaussianBlur(frame, frame, Size(7, 7), 1, 1);

	// generate binary mask
	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(skin, hsv_l, hsv_h, output_frame);

	// convert to 3 channel image

	Mat skinBin;
	Mat in[] = { output_frame, output_frame, output_frame };
	merge(in, 3, skinBin);

	// apply mask to original image
	bitwise_and(skin, skinBin, skin);

	cvtColor(skin, skin, CV_HSV2RGB);

	//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
	//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
	//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

	//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
	resize(skin, skin, Size(), 2, 2, INTER_NEAREST);
	resize(output_frame, output_frame, Size(), 2, 2, INTER_NEAREST);

	Mat handsMt = Mat(h, w, CV_8UC3, *hands);
	Mat depthChannels[3];

	split(handsMt, depthChannels);

	threshold(depthChannels[0], depthChannels[0], 2, 255, THRESH_BINARY_INV);	// depthChannels[0] = binary mask of handsMt
    morphologyEx(depthChannels[0], depthChannels[0], CV_MOP_DILATE, Mat1b(3, 3, 1), Point(-1, -1), 3);

	double* dt = new double[h*w];
	uchar* path = new uchar[h*w];

	double* d = new double[h*w];

	for (int i = 0, k = 0; i < h; i++)
	{
		const uchar* m = depthChannels[0].ptr<uchar>(i);

		for (int j = 0; j < w; j++, k++)
		{
			if (m[j] == 0)
			{
				d[k] = 0.0;
			}
			else
			{
				d[k] = 1.0e20;
			}
		}
	}

	DistanceTransform2D_NormL1Ret(d, depthChannels[0].rows, depthChannels[0].cols, dt, path);

	Mat outputDepth = Mat(h, w, CV_8UC1, cvScalar(0));

	//output_frame = Mat(h, w, CV_8UC1, Scalar(1));

	for (int i = 0, k = 0; i < h; i++)
	{
		const uchar* s = output_frame.ptr<uchar>(i);
		uchar* o = outputDepth.ptr<uchar>(i);

		for (int j = 0; j < w; j++, k++)
		{
			if (s[j] > 0)
			{
				int code;
				int dim, index;
				int r0 = i, c0 = j;
				bool moved = false;

				while ((code = path[r0*w+c0]) != 0)
				{
					moved = true;
					DT_code2index(code, &dim, &index);
					if (dim == 2)
						r0 += index;
					if (dim == 1)
						c0 += index;
				}

				const uchar* d = depthChannels[1].ptr<uchar>(r0);
				o[j] = d[c0];
			}
		}
	}

	Mat tmp[] = { outputDepth, outputDepth, outputDepth };
	merge(tmp, 3, outputDepth);

	Mat cubesMt = Mat(h, w, CV_8UC3, *cubes);

	// compare hands depth with objects depth
	Mat visibileOutput = outputDepth > cubesMt;

	bitwise_and(skin, visibileOutput, skin);
	/*
	imshow("hands", outputDepth);
	imshow("cubes", cubesMt);
	imshow("output", visibileOutput);
	imshow("skin", skin);
	waitKey(0);
	*/

	std::copy(skin.data, skin.data + h * w * 3, *input_frame);

    delete[] dt;
	delete[] path;
	delete[] d;
}
/*
int main()
{
	double d[] = { 0.0, 1.0, 1.0, 1.0, 0.0 };
	double dt[5];
	uchar p[5];

	DistanceTransform2D_NormL1Ret(d, 1, 5, dt, p);

	for (int i = 0; i < 4; i++)
	{
		cout << dt[i] << " " << (int)p[i] << "\n";
	}


	Mat img = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\b.jpg");
	Mat img2 = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\a.jpg");
	Mat hands = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\hands.jpg");

	VideoCapture c("sample.mp4");
	Mat frame;
	clock_t begin;
	while (true) {
		c >> frame;
		begin = clock();
		DetectSkin(hands.rows, hands.cols, &hands.data, &img.data, &img2.data);
		clock_t end = clock();
		double elapsed_secs = double(end - begin) / CLOCKS_PER_SEC;
		cout << 1 / elapsed_secs << " fps\n";

		imshow("", Mat(frame.rows, frame.cols, CV_8UC3, frame.data));
		waitKey(1);


		for (int i = 0; i < frame.rows*frame.cols; i++)
		{
		cout << (int)frame.data[i] << " ";
		}

	}
}
*/