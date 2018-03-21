#include "stdafx.h"
#include <opencv2\opencv.hpp>

using namespace cv;
using std::cout;

int main()
{
	String filename = "sample2.mp4";
	//String filename = "Manos.mov";
	VideoCapture capture(filename);
	Mat3b frame, bgr_frame;

	if (!capture.isOpened())
		throw "Error when reading steam_avi";

	for (; ; )
	{
		capture >> bgr_frame;
		if (bgr_frame.empty())
			break;
		resize(bgr_frame, bgr_frame, Size(), 0.175, 0.175, INTER_AREA);
		
		// THRESHOLD ON HSV
		cvtColor(bgr_frame, frame, CV_BGR2HSV);
		//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
		//GaussianBlur(frame, frame, Size(7, 7), 1, 1);
		
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
		
		// BGR CONVERSION AND THRESHOLD
		Mat1b frame_gray;
		cvtColor(frame, frame, CV_HSV2BGR);
		cvtColor(frame, frame_gray, CV_BGR2GRAY);
		threshold(frame_gray, frame_gray, 0, 255, CV_THRESH_BINARY);
		//morphologyEx(frame_gray, frame_gray, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
		//morphologyEx(frame_gray, frame_gray, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
		//morphologyEx(frame_gray, frame_gray, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

		//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
		//resize(frame_gray, frame_gray, Size(), 2, 2, INTER_LINEAR);
		// optional blurring
		//GaussianBlur(frame_gray, frame_gray, Size(7, 7), 1, 1);
		//blur(frame_gray, frame_gray, Size(5, 5));
		imshow("Threshold", frame_gray);

		//cvtColor(frame, frame, CV_BGR2HSV);
		imshow("Video", frame);

		waitKey(1); // waits to display frame
	}
	waitKey(0); // key press to close window
				// releases and window destroy are automatic in C++ interface
}
