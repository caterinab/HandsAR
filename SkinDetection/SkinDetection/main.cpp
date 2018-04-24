#include "stdafx.h"
#include <opencv2/opencv.hpp>

using namespace cv;
using std::cout;
using std::cin;
using std::vector;

extern "C" void DetectSkin(int h, int w, uchar** input_frame, uchar** hands, uchar** cubes) {
	Mat3b frame;
	Mat1b output_frame;

	frame = Mat(h, w, CV_8UC3, *input_frame);
	resize(frame, frame, Size(), 0.5, 0.5, INTER_LINEAR);

	//cvtColor(frame, output_frame, CV_BGR2GRAY);
	cvtColor(frame, frame, CV_BGR2HSV);
	//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
	//GaussianBlur(frame, frame, Size(7, 7), 1, 1);

	// generate binary mask
	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(frame, hsv_l, hsv_h, output_frame);

	// convert to 3 channel image

	Mat out;
	Mat in[] = { output_frame, output_frame, output_frame };
	merge(in, 3, out);

	// apply mask to original image
	bitwise_and(frame, out, frame);

	cvtColor(frame, frame, CV_HSV2BGR);

	//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
	//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
	//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

	//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
	resize(frame, frame, Size(), 2, 2, INTER_NEAREST);

	Mat handsMt = Mat(h, w, CV_8UC3, *hands);
	Mat channels[3];
	
	split(handsMt, channels);

	threshold(channels[0], channels[0], 2, 255, THRESH_BINARY_INV);
	imshow("", channels[0]);
	waitKey(0);

	Mat labels = Mat(h, w, CV_32SC1);
	
	distanceTransform(channels[0], channels[1], labels, DIST_L2, DIST_MASK_PRECISE, DIST_LABEL_PIXEL);

	// restringere la ricerca a solo pixel mano

	double min, max;
	minMaxLoc(labels, &min, &max);
	//labels.convertTo(labels, CV_8UC1, 255.0 / max, 0);
	cout << "max " << max << "\n";

	Mat mask = Mat(h, w, CV_8UC1);
	

	vector<Vec2i> label_to_index;
	
	for (int row = 0; row < channels[0].rows; row++)
	{
		for (int col = 0; col < channels[0].cols; col++)
		{
			if (channels[0].at<uchar>(row, col) == 0)
			{
				label_to_index.push_back(Vec2i(row, col));
			}
		}
	}
	label_to_index.push_back(0);

	cout << "size " << label_to_index.size() << "\n";

	for (int i = 0; i < mask.rows; i++)
	{
		const double* m = mask.ptr<double>(i);

		for (int j = 0; j < mask.cols; j++)
		{
			mask.at<uchar>(i, j) = channels[2].at<uchar>(label_to_index.at(labels.at<int>(i, j)));
		}
	}

	imshow("", mask);
	waitKey(0);

	Mat singles[] = { mask, mask, mask };
	merge(singles, 3, out);
	
	Mat cubesMt = Mat(h, w, CV_8UC3, *cubes);
		
	// compare hands depth with objects depth
	Mat dst = out >= cubesMt;
	/*
	imshow("", dst);
	waitKey(0);
	*/
	bitwise_and(frame, dst, frame);
	//flip(frame, frame, 0);

	std::copy(frame.data, frame.data + h * w * 3, stdext::checked_array_iterator<uchar*>(*input_frame, h*w * 3));
}

int main()
{
	Mat img = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\b.jpg");
	Mat img2 = imread(
		"C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\b.jpg");

	VideoCapture c("sample.mp4");
	Mat frame;
	clock_t begin;
	while (true) {
		c >> frame;
		begin = clock();
		DetectSkin(frame.rows, frame.cols, &frame.data, &img.data, &img2.data);
		clock_t end = clock();
		double elapsed_secs = double(end - begin) / CLOCKS_PER_SEC;
		cout << 1 / elapsed_secs << " fps\n";

		imshow("", Mat(frame.rows, frame.cols, CV_8UC3, frame.data));
		waitKey(1);

		/*
		for (int i = 0; i < frame.rows*frame.cols; i++)
		{
		cout << (int)frame.data[i] << " ";
		}
		*/
	}
}