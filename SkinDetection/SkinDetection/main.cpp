#include "stdafx.h"
#include <opencv2\opencv.hpp>
#include <chrono>

using namespace cv;
using std::cout;

String filename = "sample2.mp4";
//String filename = "Manos.mov";
VideoCapture capture(filename);
Mat3b frame;
Mat1b gray_frame;
clock_t begin;

void HSV_slow() {
	for (int r = 0; r < frame.rows; ++r) {
		for (int c = 0; c < frame.cols; ++c) {	// 0 < H < 0.25 --- 0.15 < S < 0.9 --- 0.2 < V < 0.95
			if ((frame(r, c)[0] > 0) &&		// > 5
				(frame(r, c)[0] < 17) &&
				(frame(r, c)[1] > 38) &&
				(frame(r, c)[1] < 250) &&
				(frame(r, c)[2] > 51) &&
				(frame(r, c)[2] < 242)
				); // do nothing
			else frame(r, c) = { 0, 0, 0 };
		}
	}

	cvtColor(frame, frame, CV_HSV2BGR);
	cvtColor(frame, gray_frame, CV_BGR2GRAY);
	threshold(gray_frame, gray_frame, 0, 255, CV_THRESH_BINARY);
	//GaussianBlur(frame_gray, frame_gray, Size(7, 7), 1, 1);
	//blur(frame_gray, frame_gray, Size(5, 5));
	imshow("Threshold", gray_frame);
	
	imshow("Video", frame);
}

void HSV_fast() {
	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(frame, hsv_l, hsv_h, gray_frame);

	//morphologyEx(gray_frame, gray_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion
	
	imshow("Video", gray_frame);
}

int main()
{
	if (!capture.isOpened())
		throw "Error when reading steam";

	for (; ; )
	{
		auto startT = std::chrono::high_resolution_clock::now();
				
		capture >> frame;
		if (frame.empty())
			break;
		resize(frame, frame, Size(), 0.175, 0.175, INTER_AREA);
		
		// THRESHOLD ON HSV
		cvtColor(frame, frame, CV_BGR2HSV);
		//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
		//GaussianBlur(frame, frame, Size(7, 7), 1, 1);
		
		// BGR CONVERSION AND THRESHOLD
		HSV_fast();

		//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
		//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
		//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

		//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
		//resize(frame_gray, frame_gray, Size(), 2, 2, INTER_LINEAR);
		
		waitKey(1); // waits to display frame
		
		auto finishT = std::chrono::high_resolution_clock::now();
		std::chrono::duration<double> elapsed = finishT - startT;
		cout << 1 / elapsed.count() << " fps\n";
	}
	waitKey(0); // key press to close window
				// releases and window destroy are automatic in C++ interface
}
