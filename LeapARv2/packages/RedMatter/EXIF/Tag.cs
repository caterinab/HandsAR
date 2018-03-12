using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedMatter.EXIF
{
    public enum Tag
    {
        /// <summary>
        /// A pointer to the Exif IFD
        /// </summary>
        EXIFIFD = 34665,
        /// <summary>
        /// A pointer to the GPS Info IFD.
        /// </summary>
        GPSInfo = 34853,
        /// <summary>
        /// The Interoperability structure of Interoperability IFD.
        /// </summary>
        InteroperabilityIFD = 40965,
        /// <summary>
        /// The number of columns of image data, equal to the number of pixels per row. 
        /// </summary>
        ImageWidth = 256,
        /// <summary>
        /// The number of rows of image data.
        /// </summary>
        ImageLength = 257,
        /// <summary>
        /// The number of bits per image component. 
        /// </summary>
        BitsPerSample = 258,
        /// <summary>
        /// The compression scheme used for the image data. 
        /// 1 = uncompressed 
        /// 6  =  JPEG compression (thumbnails only) 
        /// </summary>
        Compression = 259,
        /// <summary>
        /// The pixel composition.
        /// 2 = RGB 
        /// 6 = YCbCr 
        /// </summary>
        PhotometricInterpretation = 262,
        /// <summary>
        /// The image orientation viewed in terms of rows and columns.  
        /// </summary>
        Orientation = 274,
        /// <summary>
        /// Indicates whether pixel components are recorded in chunky or planar format. 
        /// 1 = chunky format 
        /// 2 = planar format 
        /// </summary>
        PlanarConfiguration = 284,
        /// <summary>
        /// The sampling ratio of chrominance components in relation to the luminance component. 
        /// </summary>
        YCbCrSubSampling = 530,
        /// <summary>
        /// The position of chrominance components in relation to the luminance component.
        /// </summary>
        YCbCrPositioning = 531,
        /// <summary>
        /// The number of pixels per ResolutionUnit in the  ImageWidth direction. 
        /// </summary>
        XResolution = 282,
        /// <summary>
        /// The number of pixels per  ResolutionUnit in the  ImageLength direction. 
        /// </summary>
        YResolution = 283,
        /// <summary>
        /// The unit for measuring XResolution and YResolution. 
        /// 2 = inches 
        /// 3 = centimeters 
        /// </summary>
        ResolutionUnit = 296,
        /// <summary>
        /// The date and time of image creation. 
        /// </summary>
        DateTime = 306,
        /// <summary>
        /// A character string giving the title of the image. 
        /// </summary>
        ImageDescription = 270,
        /// <summary>
        /// The manufacturer of the recording equipment. 
        /// </summary>
        Make = 271,
        /// <summary>
        /// The model name or model number of the equipment. 
        /// </summary>
        Model = 272,
        /// <summary>
        /// This tag records the name and version of the software or  firmware of the camera or image input device used to 
        /// generate the image. 
        /// </summary>
        Software = 305,
        /// <summary>
        /// This tag records the name of the camera owner, photographer or image creator. 
        /// </summary>
        Artist = 315,
        /// <summary>
        /// Copyright information.
        /// </summary>
        Copyright = 33432,
        /// <summary>
        /// The version of this standard supported. 
        /// </summary>
        ExifVersion = 36864,
        /// <summary>
        /// The date and time when the original image data was generated. 
        /// </summary>
        DateTimeOriginal = 36867,
        /// <summary>
        /// The date and time when the image was stored as digital data. 
        /// </summary>
        DateTimeDigitized = 36868,
        /// <summary>
        /// A tag used to record fractions of seconds for the DateTime tag. 
        /// </summary>
        SubsecTime = 37520,
        /// <summary>
        /// A tag used to record fractions of seconds for the DateTimeOriginal tag. 
        /// </summary>
        SubsecTimeOriginal = 37521,
        /// <summary>
        /// A tag used to record fractions of seconds for the DateTimeDigitized tag. 
        /// </summary>
        SubsecTimeDigitized = 37522,
        /// <summary>
        /// Exposure time, given in seconds (sec). 
        /// </summary>
        ExposureTime = 33434,
        /// <summary>
        /// The F number. 
        /// </summary>
        FNumber = 33437,
        /// <summary>
        /// The class of the program used by the camera to set exposure when the picture is taken.
        /// 0 = Not defined 
        ///1 = Manual 
        ///2 = Normal program 
        ///3 = Aperture priority 
        ///4 = Shutter priority 
        ///5  =  Creative program (biased toward depth of field) 
        ///6  =  Action program (biased toward fast shutter speed) 
        ///7  =  Portrait mode (for closeup photos with the background out of focus)   
        ///8  =  Landscape mode (for landscape photos with the background in focus) 
        /// </summary>
        ExposureProgram = 34850,
        /// <summary>
        /// Indicates the spectral sensitivity of each channel of the camera used. 
        /// </summary>
        SpectralSensitivity = 34852,
        /// <summary>
        /// Indicates the ISO Speed and ISO Latitude of the camera or input device as specified in ISO 12232. 
        /// </summary>
        ISOSpeedRatings = 34855,
        /// <summary>
        /// Shutter speed. 
        /// </summary>
        ShutterSpeedValue = 37377,
        /// <summary>
        /// The lens aperture. 
        /// </summary>
        ApertureValue = 37378,
        /// <summary>
        /// The value of brightness. 
        /// </summary>
        BrightnessValue = 37379,
        /// <summary>
        /// The exposure bias. 
        /// </summary>
        ExposureBiasValue = 37380,
        /// <summary>
        /// The smallest F number of the lens.
        /// </summary>
        MaxApertureValue = 37381,
        /// <summary>
        /// The distance to the subject, given in meters. 
        /// </summary>
        SubjectDistance = 37382,
        /// <summary>
        /// The metering mode. 
        /// 0 = unknow
        /// 1 = Average
        /// 2 = CenterW
        /// 3 = Spot 
        /// 4 = MultiSpo
        /// 5 = Pattern 
        /// 6 = Partial 
        /// Other = reserved
        /// 255 = other 
        /// </summary>
        MeteringMode = 37383,
        /// <summary>
        /// The kind of light source.
        /// 0 = unknown 
        /// 1 = Daylight 
        /// 2 = Fluorescent 
        /// 3  =  Tungsten (incandescent light) 
        /// 4 = Flash 
        /// 9 = Fine weather 
        /// 10 = Cloudy weather 
        /// 11 = Shade 
        /// 12  =  Daylight fluorescent (D 5700 – 7100K) 
        /// 13  =  Day white fluorescent (N 4600 – 5400K) 
        /// 14  =  Cool white fluorescent (W 3900 – 4500K) 
        /// 15  =  White fluorescent (WW 3200 – 3700K) 
        /// 17  =  Standard light A 
        /// 18  =  Standard light B 
        /// 19  =  Standard light C 
        /// 20 = D55 
        /// 21 = D65 
        /// 22 = D75 
        /// 23 = D50 
        /// 24  =  ISO studio tungsten 
        /// 255  =  other light source 
        /// </summary>
        LightSource = 37384,
        /// <summary>
        /// This tag indicates the location and area of the main subject in the overall scene. 
        /// </summary>
        SubjectArea = 37396,
        /// <summary>
        /// The actual focal length of the lens, in mm. 
        /// </summary>
        FocalLength = 37386,
        /// <summary>
        /// Indicates the strobe energy at the time the image is  captured, as measured in Beam Candle Power Seconds 
        /// (BCPS). 
        /// </summary>
        FlashEnergy = 41483,
        /// <summary>
        /// This tag records the camera or input device spatial frequency table.
        /// </summary>
        SpatialFrequencyResponse = 41484,
        /// <summary>
        /// Indicates the number of pixels in the image width (X) direction per FocalPlaneResolutionUnit 
        /// on the camera focal plane. 
        /// </summary>
        FocalPlaneXResolution = 41486,
        /// <summary>
        /// Indicates the number of pixels in the image height (Y) direction per FocalPlaneResolutionUnit 
        /// on the camera focal plane. 
        /// </summary>
        FocalPlaneYResolution = 41487,
        /// <summary>
        /// Indicates the unit for measuring FocalPlaneXResolution and FocalPlaneYResolution. 
        /// </summary>
        FocalPlaneResolutionUnit = 41488,
        /// <summary>
        /// Indicates the location of the main subject in the scene. 
        /// </summary>
        SubjectLocation = 41492,
        /// <summary>
        /// Indicates the exposure index selected on the camera or input device at the time the image is captured.   
        /// </summary>
        ExposureIndex = 41493,
        /// <summary>
        /// Indicates the image sensor type on the camera or input device.
        /// </summary>
        SensingMethod = 41495,
        /// <summary>
        /// Indicates the type of scene.
        /// </summary>
        SceneType = 41729,
        /// <summary>
        /// Indicates the color filter array 
        /// </summary>
        CFAPattern = 41730,
        /// <summary>
        /// This tag indicates the use of special processing on image data, such as rendering geared to output. 
        /// </summary>
        CustomRendered = 41985,
        /// <summary>
        /// This tag indicates the exposure mode set when the image was shot.
        /// 0  =  Auto exposure 
        /// 1  =  Manual exposure 
        /// 2  =  Auto bracket 
        /// </summary>
        ExposureMode = 41986,
        /// <summary>
        /// This tag indicates the white balance mode set when the image was shot. 
        /// 0  =  Auto white balance
        /// 1  =  Manual white balance 
        /// </summary>
        WhiteBalance = 41987,
        /// <summary>
        /// This tag indicates the equivalent focal length assuming a 35mm film camera, in mm. 
        /// </summary>
        DigitalZoomRatio = 41989,
        /// <summary>
        /// This tag indicates the type of scene that was shot. 
        /// 0  =  Standard 
        /// 1  =  Landscape 
        /// 2  =  Portrait 
        /// 3  =  Night scene 
        /// </summary>
        SceneCaptureType = 41990,
        /// <summary>
        /// This tag indicates the degree of overall image gain adjustment. 
        /// </summary>
        GainControl = 41991,
        /// <summary>
        /// This tag indicates the direction of contrast processing applied by the camera when the image was shot. 
        /// 0  =  Normal 
        /// 1  =  Soft 
        /// 2  =  Hard 
        /// </summary>
        Contrast = 41992,
        /// <summary>
        /// This tag indicates the direction of saturation processing applied by the camera when the image was shot. 
        /// 0  =  Normal 
        /// 1  =  Low saturation 
        /// 2  =  High saturation 
        /// </summary>
        Saturation = 41993,
        /// <summary>
        /// This tag indicates the direction of sharpness processing applied by the camera when the image was shot. 
        /// 0  =  Normal 
        /// 1  =  Soft 
        /// 2  =  Hard 
        /// </summary>
        Sharpness = 41994,
        /// <summary>
        /// This tag indicates information on the picture-taking conditions of a particular camera model. 
        /// </summary>
        DeviceSettingDescription = 41995,
        /// <summary>
        /// This tag indicates the distance to the subject. 
        /// 0  =  unknown 
        /// 1  =  Macro 
        /// 2  =  Close view 
        /// 3  =  Distant view 
        /// </summary>
        SubjectDistanceRange = 41996,
        /// <summary>
        /// Indicates the identification of the Interoperability  rule. 
        /// </summary>
        InteroperabilityIndex = 1,
        /// <summary>
        /// Information specific to compressed data. 
        /// </summary>
        ComponentsConfiguration = 37121,
        /// <summary>
        /// Information specific to compressed data. 
        /// </summary>
        CompressedBitsPerPixel = 37122,
        /// <summary>
        /// A tag for manufacturers of Exif writers to record any desired information. 
        /// </summary>
        MakerNote = 37500,
        /// <summary>
        /// The Flashpix format version supported by a FPXR file. 
        /// </summary>
        FlashpixVersion = 40960,
        /// <summary>
        /// The color space information tag (ColorSpace) is always recorded as the color space specifier. 
        /// </summary>
        ColorSpace = 40961,
        /// <summary>
        /// Information specific to compressed data.
        /// </summary>
        PixelYDimension = 40963,
        /// <summary>
        /// Information specific to compressed data.
        /// </summary>
        PixelXDimension = 40962,
        /// <summary>
        /// The offset to the start byte (SOI) of JPEG compressed  thumbnail data.
        /// </summary>
        JPEGInterchangeFormat = 513,
        /// <summary>
        /// The number of bytes of JPEG compressed thumbnail data. 
        /// </summary>
        JPEGInterchangeFormatLength = 514,
    }
}
