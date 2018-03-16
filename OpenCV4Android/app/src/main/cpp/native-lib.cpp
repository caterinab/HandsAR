#include <jni.h>
#include <string>

#include "include/opencv2/core.hpp"

using namespace std;
using namespace cv;

//resolution cols,     rows     input image   delay   filter select  output image
extern "C" int ocv_get_image(int xres, int yres, uint8_t* z,int *delay, int filt, void **result2)
{
    double tg1 = (double)getTickCount();
    //input frame     rows  cols
    Mat framein = Mat(yres, xres, CV_8UC4, z);
    //output frame
    Mat frameout(framein.rows, framein.cols, CV_8UC4, Scalar(238,244,66));
    //processing frame
    Mat frameproc;
    // Filter select
    cvtColor(framein,frameproc,CV_RGBA2BGR);
    switch ( filt ) {
        case 1: {
            // Simple threshold
            threshold(frameproc,frameproc,128,128,THRESH_BINARY);
            Point textplace(0, 50);
            stringstream ss;
            ss << "C" << frameproc.cols << "R" << frameproc.rows;
            // scale  color     thickness  ciara
            putText(frameproc, ss.str() , textplace ,CV_FONT_NORMAL,1,Scalar(255,255,0), 1, LINE_AA);
            rectangle(frameproc, Point( 0,60 ), Point( 100, 100), Scalar( 255,255,0 ), -1, 8 );
            flip(frameproc,frameproc,1);
            blur(frameproc,frameproc,Size(3,3));
            cvtColor(frameproc,frameout, CV_BGR2RGBA,4);
            break;}
        case 2: {
            // Canny Edge
            GaussianBlur(frameproc, frameproc, Size(7,7), 1.5, 1.5);
            cvtColor(frameproc, frameproc, COLOR_BGR2GRAY);
            Canny(frameproc, frameproc, 0, 30, 3);
            cvtColor(frameproc,frameout, CV_GRAY2RGBA,4);
            break;}
        case 3: {
            // Gausian + Gray
            cvtColor(frameproc, frameproc, COLOR_BGR2GRAY);
            cvtColor(frameproc, frameout,CV_GRAY2RGBA,4);
            break;}
        case 4: {
            // Face detection WIP
            cvtColor(frameproc, frameout,CV_BGR2RGBA,4);
            break;}
        default:
        {}
    }
    // Image result
    uint8_t result2data[framein.cols*framein.rows*4];
    memcpy(result2data, frameout.data, framein.cols*framein.rows*4);
    *result2 = result2data;

    //Memory release
    framein.release();
    frameout.release();
    frameproc.release();

    double fpsg = 1/(((double)getTickCount()-tg1)/(getTickFrequency()));
    int delay1 = 0;
    *delay = fpsg;
    return 1;
}