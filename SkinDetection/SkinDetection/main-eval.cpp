#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include "distanceTransform.h"

using namespace cv;
using std::cout;
using std::cin;

extern "C" void DetectSkin(int h, int w, uchar** cube, uchar** error, uchar** hand) {
	Mat3b skin, skinHSV;
	Mat1b output_frame, output_frame1, output_frame2;
	int screen = h * w;
	
	Scalar lb(110, 50, 50);
	Scalar hb(130, 255, 255);

	skin = Mat(h, w, CV_8UC3, *hand);
	cvtColor(skin, skinHSV, CV_BGR2HSV);
	inRange(skinHSV, lb, hb, output_frame);

	float hand_count = 0;
	for (int y = 0; y < output_frame.rows; y++) {
		for (int x = 0; x < output_frame.cols; x++) {
			if (output_frame.at<uchar>(y, x) == 255) {
				hand_count++;
			}
		}
	}


	skin = Mat(h, w, CV_8UC3, *error);
	cvtColor(skin, skinHSV, CV_BGR2HSV);
	inRange(skinHSV, lb, hb, output_frame1);

	float error_count = 0;
	for (int y = 0; y < output_frame1.rows; y++) {
		for (int x = 0; x < output_frame1.cols; x++) {
			if (output_frame1.at<uchar>(y, x) == 255) {
				error_count++;
			}
		}
	}

	skin = Mat(h, w, CV_8UC3, *cube);
	cvtColor(skin, skinHSV, CV_BGR2HSV);
	inRange(skinHSV, lb, hb, output_frame2);
	
	bitwise_and(output_frame, output_frame2, output_frame2);
	
	float a_count = 0;
	for (int y = 0; y < output_frame2.rows; y++) {
		for (int x = 0; x < output_frame2.cols; x++) {
			if (output_frame2.at<uchar>(y, x) == 255) {
				a_count++;
			}
		}
	}

	//a_count /= hand_count;
	cout << error_count << " " << a_count;
	//float err = 100 - ((error_count / a_count / screen) * 100);
	float err = 100 - ((error_count / a_count) * 100);

	cout << "error: " << err << "\n";
}

int main()
{
	Mat img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A1a.jpg");
	Mat img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A1b.jpg");
	Mat img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A1c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A2a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A2b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A2c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A3a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A3b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\A3c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B1a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B1b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B1c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B2a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B2b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B2c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B3a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B3b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B3c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B4a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B4b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\B4c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C1a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C1b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C1c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C2a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C2b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C2c.jpg");
	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);
	img = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C3a.jpg");
	img1 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C3b.jpg");
	img2 = imread("C:\\Users\\cbattisti\\Documents\\HandsAR\\SkinDetection\\SkinDetection\\test_img\\C3c.jpg");

	DetectSkin(img.rows, img.cols, &img.data, &img1.data, &img2.data);

	cin.ignore();
}