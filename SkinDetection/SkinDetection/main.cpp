#include "stdafx.h"
#include <opencv2/opencv.hpp>

using namespace cv;
using std::cout;
using std::vector;

extern "C" void DetectSkin(int h, int w, uchar** input_frame) {
	Mat3b frame;
	Mat1b output_frame;//, inv;
	//RNG rng(12345);

	frame = Mat(h, w, CV_8UC3, *input_frame);
	resize(frame, frame, Size(), 0.5, 0.5, INTER_AREA);

	//cvtColor(frame, output_frame, CV_BGR2GRAY);
	cvtColor(frame, frame, CV_BGR2HSV);
	//cvtColor(bgr_frame, frame, CV_BGR2YCrCb);
	//GaussianBlur(frame, frame, Size(7, 7), 1, 1);

	// generate binary mask
	Scalar hsv_l(0, 38, 51);
	Scalar hsv_h(17, 250, 242);
	inRange(frame, hsv_l, hsv_h, output_frame);

	/*
	// A image with size greater than the present object is created, it is needed from floodFill()
	cv::Mat mask = cv::Mat::zeros(frame.rows + 2, frame.cols + 2, CV_8U);

	cv::floodFill(output_frame, mask, cv::Point(0, 0), 255, 0, cv::Scalar(), cv::Scalar(), 4 + (255 << 8) + cv::FLOODFILL_MASK_ONLY);
	//NOTE Since the mask is larger than the filled image, a pixel  (x, y) in image corresponds to the pixel (x+1, y+1) in the mask .

	//remove the extra rows/cols added earlier in the initialization of the mask, if you want of course it is just "optional"
	mask(Range(1, mask.rows - 1), Range(1, mask.cols - 1)).copyTo(output_frame);
	bitwise_not(output_frame, output_frame);
	*/
	/*
	vector<vector<Point> > contours;
	vector<Vec4i> hierarchy;

	/// Detect edges using canny
	Canny(output_frame, output_frame, 100, 100 * 2, 3);
	/// Find contours
	findContours(output_frame, contours, hierarchy, CV_RETR_TREE, CV_CHAIN_APPROX_SIMPLE, Point(0, 0));

	/// Draw contours
	for (int i = 0; i< contours.size(); i++)
	{
	Scalar color = Scalar(rng.uniform(0, 255), rng.uniform(0, 255), rng.uniform(0, 255));
	drawContours(output_frame, contours, i, color, 1, 8, hierarchy, 0, Point());
	}

	bitwise_not(output_frame, output_frame);
	*/

	// convert to 3 channel image
	cv::Mat out;
	cv::Mat in[] = { output_frame, output_frame, output_frame };
	cv::merge(in, 3, out);

	/*
	for (int i = 0; i < frame.rows; ++i)
	{
	cv::Vec3b* pixel = frame.ptr<cv::Vec3b>(i); // point to first pixel in row
	for (int j = 0; j < frame.cols; ++j)
	{
	if ((pixel[j][0] > 0) &&
	(pixel[j][0] < 17) &&
	(pixel[j][1] > 38) &&
	(pixel[j][1] < 250) &&
	(pixel[j][2] > 51) &&
	(pixel[j][2] < 242)
	); // do nothing
	else pixel[j] = { 0, 0, 0 };
	}
	}
	*/

	// apply mask to original image
	bitwise_and(frame, out, frame);

	cvtColor(frame, frame, CV_HSV2BGR);

	//morphologyEx(output_frame, output_frame, CV_MOP_ERODE, Mat1b(3, 3, 1), Point(-1, -1), 3);	// erosion
	//morphologyEx(output_frame, output_frame, CV_MOP_OPEN, Mat1b(7, 7, 1), Point(-1, -1), 1);	// erosion + dilatation
	//morphologyEx(output_frame, output_frame, CV_MOP_CLOSE, Mat1b(9, 9, 1), Point(-1, -1), 1);	// dilatation + erosion

	//resize(frame, frame, Size(), 2, 2, INTER_LINEAR);
	resize(frame, frame, Size(), 2, 2, INTER_NEAREST);

	std::copy(frame.data, frame.data + h * w * 3, stdext::checked_array_iterator<uchar*>(*input_frame, h*w * 3));
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