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

        private CvRect FindBorder(IplImage image)
        {
            byte color = (byte)image[0, 0].Val0;
            var minY = image.Height;
            var minX = image.Width;
            var maxY = 0;
            var maxX = 0;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var currentColor = (byte)image[y, x].Val0;
                    if (color != currentColor)
                    {
                        if (x < minX) minX = x;
                        if (x > maxX) maxX = x;
                        if (y < minY) minY = y;
                        if (y > maxY) maxY = y;
                    }
                }
            }
            if (minY == image.Height) minY = 0;
            if (minX == image.Width) minX = 0;
            if (maxY == 0) maxY = image.Height;
            if (maxX == 0) maxX = image.Width;
            return new CvRect(minX, minY, maxX - minX, maxY - minY);
        }

        public double GetPercentageOfSkinInImage(string filePath, string debugImageFilePath = null)
        {
            using (IplImage imgSrc = new IplImage(filePath))
            {
                using (CvMemStorage storage = new CvMemStorage())
                {
                    CvRect rect;
                    using (IplImage imgGray = new IplImage(filePath, LoadMode.GrayScale))
                    {
                        rect = FindBorder(imgGray);

                    }
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
