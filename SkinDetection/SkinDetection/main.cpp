#include "stdafx.h"
#include <opencv2/opencv.hpp>

using namespace cv;
using std::cout;

extern "C" void DetectSkin(int h, int w, uchar** input_frame) {
	Mat3b frame;
	Mat1b output_frame;

	frame = Mat(h, w, CV_8UC3, *input_frame);
	resize(frame, frame, Size(), 0.5, 0.5, INTER_AREA);

	cvtColor(frame, frame, CV_BGR2HSV);
	//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
	//GaussianBlur(frame, frame, Size(7, 7), 1, 1);

	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(frame, hsv_l, hsv_h, output_frame);

	//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
	//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
	//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

	//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
	resize(output_frame, output_frame, Size(), 2, 2, INTER_NEAREST);

	cv::Mat out;
	cv::Mat in[] = { output_frame, output_frame, output_frame };
	cv::merge(in, 3, out);

	//imshow("", Mat(h,w,CV_8UC1,*array));
	//std::cin.ignore();

	std::copy(out.data, out.data + h * w * 3, stdext::checked_array_iterator<uchar*>(*input_frame, h*w*3));

}

int main()
{
	
	VideoCapture c("sample.mp4");
	Mat frame;
	clock_t begin;
	while (true) {
	c >> frame;
	begin = clock();
	uchar** p = &frame.data;
	DetectSkin(frame.rows, frame.cols, p);
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
