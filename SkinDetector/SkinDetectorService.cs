using OpenCvSharp;
using OpenCvSharp.CPlusPlus;
using System;
using System.Collections.Generic;
using System.Text;

namespace SkinDetector
{
    public class SkinDetectorService
    {
        protected int CountSkinPoints(IplImage imgHueMask, CvColor color)
        {
            int count = 0;
            for (int y = 0; y < imgHueMask.Height; y++)
            {
                for (int x = 0; x < imgHueMask.Width; x++)
                {
                    byte value = (byte)imgHueMask[y, x].Val0;
                    if (value != 0)
                    {
                        count++;
                        imgHueMask[y, x] = color;
                    }
                }
            }
            return count;
        }

        public double GetPercentageOfSkinInImage(string filePath, string debugImageFilePath = null)
        {
            using (IplImage imgSrc = new IplImage(filePath))
            {
                using (CvMemStorage storage = new CvMemStorage())
                {
                    CvSeq<CvPoint> points;
                    using (IplImage imgGray = new IplImage(filePath, LoadMode.GrayScale))
                    {
                        imgGray.FindContours(storage, out points, CvContour.SizeOf, ContourRetrieval.External, ContourChain.ApproxNone);
                    }
                    var rect = Cv.BoundingRect(points);
                    if (rect.Width < imgSrc.Width * 0.10 || rect.Height < imgSrc.Height * 0.10)
                        rect = new CvRect(0, 0, imgSrc.Width, imgSrc.Height);
                    using (var subImg = imgSrc.GetSubImage(rect))
                    {
                        using (IplImage imgHueMask = new IplImage(subImg.Size, BitDepth.U8, 1))
                        {
                            CvAdaptiveSkinDetector detector = new CvAdaptiveSkinDetector(1, MorphingMethod.ErodeDilate);
                            detector.Process(subImg, imgHueMask);
                            int count = CountSkinPoints(imgHueMask, CvColor.White);
                            var percent = ((double)count / (double)(subImg.Width * subImg.Height) * 100);
                            if (debugImageFilePath != null) imgHueMask.SaveImage(debugImageFilePath);
                            return percent;
                        }
                    }
                }
            }
        }
    }
}
