#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include "distanceTransform.h"

using namespace cv;
using std::cout;
using std::cin;
using std::vector;

extern "C" void DetectSkin(int h, int w, uchar** input_frame, uchar** hands, uchar** cubes) {
	Mat3b skin, skinHSV;
	Mat1b output_frame;
	int h_xs = h / 2;
	int w_xs = w / 2;

	skin = Mat(h, w, CV_8UC3, *input_frame);
	resize(skin, skin, Size(), 0.5, 0.5, INTER_LINEAR);
	flip(skin, skin, 0);

	//cvtColor(frame, output_frame, CV_BGR2GRAY);
	cvtColor(skin, skinHSV, CV_RGB2HSV);
	//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
	//GaussianBlur(frame, frame, Size(7, 7), 1, 1);

	// generate binary mask
	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(skinHSV, hsv_l, hsv_h, output_frame);

	//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
	//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
	//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

	//resize(skin, skin, Size(), 2, 2, INTER_NEAREST);
	//resize(output_frame, output_frame, Size(), 2, 2, INTER_NEAREST);

	Mat handsMt = Mat(h, w, CV_8UC3, *hands);
	resize(handsMt, handsMt, Size(), 0.5, 0.5, INTER_LINEAR);
	Mat depthChannels[3];
	split(handsMt, depthChannels);

	threshold(depthChannels[0], depthChannels[0], 0, 255, THRESH_BINARY_INV);	// depthChannels[0] = binary mask of handsMt
	// dilate mask to reduce border imprecision
	morphologyEx(depthChannels[0], depthChannels[0], CV_MOP_DILATE, Mat1b(9, 9, 1), Point(-1, -1), 1);

	double* dt = new double[h_xs * w_xs];
	uchar* path = new uchar[h_xs * w_xs];

	double* d = new double[h_xs * w_xs];

	for (int i = 0, k = 0; i < h_xs; i++)
	{
		const uchar* m = depthChannels[0].ptr<uchar>(i);

		for (int j = 0; j < w_xs; j++, k++)
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

	DistanceTransform2D_NormL1Ret(d, h_xs, w_xs, dt, path);

	Mat outputDepth = Mat(h_xs, w_xs, CV_8UC1, cvScalar(0));

	for (int i = 0, k = 0; i < h_xs; i++)
	{
		const uchar* s = output_frame.ptr<uchar>(i);
		uchar* o = outputDepth.ptr<uchar>(i);

		for (int j = 0; j < w_xs; j++, k++)
		{
			if (s[j] > 0)
			{
				int code;
				int dim, index;
				int r0 = i, c0 = j;
				bool moved = false;

				while ((code = path[r0 * w_xs + c0]) != 0)
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

	Mat cubesMt = Mat(h, w, CV_8UC3, *cubes);
	resize(cubesMt, cubesMt, Size(), 0.5, 0.5, INTER_LINEAR);
	//morphologyEx(cubesMt, cubesMt, CV_MOP_DILATE, Mat1b(3, 3, 1), Point(-1, -1), 1);
	//blur(cubesMt, cubesMt, Size(5, 5));
	Mat cubesChannels[3];
	split(cubesMt, cubesChannels);

	// compare hands depth with objects depth
	Mat visibleOutput = Mat(h_xs, w_xs, CV_8UC1, Scalar(0));
	visibleOutput = outputDepth >= cubesChannels[0];

	// dilate output to remove holes and spikes + AND to recover precise border
	morphologyEx(visibleOutput, visibleOutput, CV_MOP_DILATE, Mat1b(9, 9, 1), Point(-1, -1), 1);
	bitwise_and(visibleOutput, output_frame, visibleOutput);

	// convert to 3 channel image
	Mat outputBin;
	Mat in[] = { visibleOutput, visibleOutput, visibleOutput };
	merge(in, 3, outputBin);

	// apply mask to original image
	bitwise_and(skin, outputBin, skin);

	resize(skin, skin, Size(), 2, 2, INTER_NEAREST);
	/*
	imshow("hands", outputDepth);
	imshow("cubes", cubesMt);
	imshow("output", visibleOutput);
	imshow("skin", skin);
	waitKey(0);
	*/
	std::copy(skin.data, skin.data + h * w * 3, stdext::checked_array_iterator<uchar*>(*input_frame, h*w * 3));

	delete[] dt;
	delete[] path;
	delete[] d;
}

int main()
{/*
	double d[] = { 0.0, 1.0, 1.0, 1.0, 0.0 };
	double dt[5];
	uchar p[5];

	DistanceTransform2D_NormL1Ret(d, 1, 5, dt, p);

	for (int i = 0; i < 4; i++)
	{
		cout << dt[i] << " " << (int)p[i] << "\n";
	}
	*/
	Mat img = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\a.jpg");
	Mat img2 = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\depth.jpg");
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
		/*
		imshow("", Mat(frame.rows, frame.cols, CV_8UC3, frame.data));
		waitKey(1);
		*/
		/*
		for (int i = 0; i < frame.rows*frame.cols; i++)
		{
		cout << (int)frame.data[i] << " ";
		}
		*/
	}
}