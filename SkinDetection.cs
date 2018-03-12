using UnityEngine;
using System.Collections;
using System;
using System.IO;
using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
//using Emgu.CV.Structure;
//using Emgu.CV;
//using System.Drawing;

//using OpenCvSharp.Extensions;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;


public class SkinDetection : MonoBehaviour
{
    public GameObject planeObj;
    public GameObject cubeObj;
    public WebCamTexture webcamTexture;
    public Texture2D texImage;
    public string deviceName;
    private int devId = 1;
    private int imWidth = 640;
    private int imHeight = 480;
    private string errorMsg = "No errors found!";
    static IplImage matrix;
    private int speed = 20;
        
    IplImage skin;
    int avg_y = 100;
    int avg_cb_min = 80;
    int avg_cb_max = 135;
    int avg_cr_min = 131;
    int avg_cr_max = 185;

    Scalar Scalar1;
    Scalar Scalar2;
    Scalar black;
    Scalar white;
    //	CvScalar Scalar1 = Cv.Scalar2(100, 131, 80);
    //	CvScalar Scalar2 = cvScalar(255, 185, 135);
    //int skinRange = 20;

    // Use this for initialization
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        Debug.Log("num:" + devices.Length);

        for (int i = 0; i < devices.Length; i++)
        {
            print(devices[i].name);
            if (devices[i].name.CompareTo(deviceName) == 1)
            {
                devId = i;
            }
        }

        if (devId >= 0)
        {
            planeObj = GameObject.Find("Plane");
            cubeObj = GameObject.Find("Cube");
            texImage = new Texture2D(imWidth, imHeight, TextureFormat.RGB24, false);
            webcamTexture = new WebCamTexture(devices[devId].name, imWidth, imHeight, 60);
            webcamTexture.Play();
            
            matrix = new IplImage(imWidth, imHeight, BitDepth.U8, 3);
            skin = new IplImage(imWidth, imHeight, BitDepth.U8, 3);

            Scalar1 = new Scalar(100, 131, 80);
            Scalar2 = new Scalar(255, 185, 135);
            black = new Scalar(0, 0, 0);
            white = new Scalar(255, 255, 255);
        }
    }
    
    void SkinColorDetection()
    {
        Scalar scalarImg;
        double y, cb, cr;
        for (int i = 0; i < skin.Height; i++)
            for (int j = 0; j < skin.Width; j++)
            {
                scalarImg = skin.Get2D(i, j);
                y = scalarImg.Val0;
                cr = scalarImg.Val1;
                cb = scalarImg.Val2;
                if ((y > avg_y) && (cb > avg_cb_min && cb < avg_cb_max) && (cr > avg_cr_min && cr < avg_cr_max))
                    skin.Set2D(i, j, white);
                else
                    skin.Set2D(i, j, black);
            }
    }
    void Update()
    {
        if (devId >= 0)
        {

            Texture2DtoIplImage();
            /*
					CvFont font = new CvFont (FontFace.Vector0, 1.0, 1.0);
					CvColor rcolor = CvColor.Random ();
					Cv.PutText (matrix, "Snapshot taken!", new CvPoint (15, 30), font, rcolor);
					IplImage cny = new IplImage (imWidth, imHeight, BitDepth.U8, 1);
					matrix.CvtColor (cny, ColorConversion.RgbToGray);
					Cv.Canny (cny, cny, 50, 50, ApertureSize.Size3);
					Cv.CvtColor(cny, matrix, ColorConversion.GrayToBgr);
*/
            matrix.CvtColor(skin, ColorConversion.BgrToCrCb);
            SkinColorDetection();
            skin.Erode(matrix);

            if (webcamTexture.didUpdateThisFrame)
            {
                IplImageToTexture2D();
            }


        }
        else
        {
            Debug.Log("Can't find camera!");
        }
        if (Input.GetKey("a"))
            cubeObj.transform.Rotate(Vector3.up * Time.deltaTime * speed);
        if (Input.GetKey("d"))
            cubeObj.transform.Rotate(-1 * Vector3.up * Time.deltaTime * speed);
    }

    void OnGUI()
    {
        GUI.Label(new UnityEngine.Rect(200, 200, 100, 90), errorMsg);
    }

    void IplImageToTexture2D()
    {
        int jBackwards = imHeight;

        for (int i = 0; i < imHeight; i++)
        {
            for (int j = 0; j < imWidth; j++)
            {
                float b = (float)matrix[i, j].Val0;
                float g = (float)matrix[i, j].Val1;
                float r = (float)matrix[i, j].Val2;
                UnityEngine.Color color = new UnityEngine.Color(r / 255.0f, g / 255.0f, b / 255.0f);


                jBackwards = imHeight - i - 1; // notice it is jBackward and i
                texImage.SetPixel(j, jBackwards, color);
            }
        }
        texImage.Apply();
        planeObj.GetComponent<Renderer>().material.mainTexture = texImage;

    }

    void Texture2DtoIplImage()
    {
        int jBackwards = imHeight;

        for (int v = 0; v < imHeight; ++v)
        {
            for (int u = 0; u < imWidth; ++u)
            {

                Scalar col = new Scalar();
                col.Val0 = (double)webcamTexture.GetPixel(u, v).b * 255;
                col.Val1 = (double)webcamTexture.GetPixel(u, v).g * 255;
                col.Val2 = (double)webcamTexture.GetPixel(u, v).r * 255;

                jBackwards = imHeight - v - 1;

                matrix.Set2D(jBackwards, u, col);
                //matrix [jBackwards, u] = col;
            }
        }
        //Cv.SaveImage ("C:\\Hasan.jpg", matrix);
    }
}
/*
public class HandGestureRecognition
{
	IColorSkinDetector skinDetector;
	Image<Bgr, Byte> currentFrame;
	Image<Bgr, Byte> currentFrameCopy;
	AdaptiveSkinDetector detector;
	int frameWidth;
	int frameHeight;
	Hsv hsv_min;
	Hsv hsv_max;
	Ycc YCrCb_min;
	Ycc YCrCb_max;
	
	Seq<Point> hull;
	Seq<Point> filteredHull;
	Seq<MCvConvexityDefect> defects;
	MCvConvexityDefect[] defectArray;
	MCvBox2D box;
	Ellipse ellip;
	public HandGestureRecognition(){
		detector = new AdaptiveSkinDetector(1, AdaptiveSkinDetector.MorphingMethod.NONE);
		hsv_min = new Hsv(0, 45, 0); 
		hsv_max = new Hsv(20, 255, 255);            
		YCrCb_min = new Ycc(0, 131, 80);
		YCrCb_max = new Ycc(255, 185, 135);
		box = new MCvBox2D();
		ellip = new Ellipse();
	}
}
public abstract class IColorSkinDetector
{
	public abstract Image<Gray, Byte> DetectSkin(Image<Bgr, Byte> Img, IColor min, IColor max);        
}
public class HsvSkinDetector:IColorSkinDetector
{
	
	public override Image<Gray, byte> DetectSkin(Image<Bgr, byte> Img, IColor min, IColor max)
	{
		Image<Hsv, Byte> currentHsvFrame = Img.Convert<Hsv, Byte>();
		Image<Gray, byte> skin = new Image<Gray, byte>(Img.Width, Img.Height);
		skin = currentHsvFrame.InRange((Hsv)min,(Hsv)max);
		return skin;
	}
}
public class YCrCbSkinDetector:IColorSkinDetector
{
	public override Image<Gray, byte> DetectSkin(Image<Bgr, byte> Img, IColor min, IColor max)
	{
		Image<Ycc, Byte> currentYCrCbFrame = Img.Convert<Ycc, Byte>();
		Image<Gray, byte> skin = new Image<Gray, byte>(Img.Width, Img.Height);
		skin = currentYCrCbFrame.InRange((Ycc)min,(Ycc) max);
		StructuringElementEx rect_12 = new StructuringElementEx(12, 12, 6, 6, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
		CvInvoke.cvErode(skin, skin, rect_12, 1);
		StructuringElementEx rect_6 = new StructuringElementEx(6, 6, 3, 3, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_RECT);
		CvInvoke.cvDilate(skin, skin, rect_6, 2);
		return skin;
	}
	
}
*/
