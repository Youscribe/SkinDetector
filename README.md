SkinDetector
============

Simple C# library to detect percentage of skin in image. Use opencv and OpenCVSharp to do this (CvAdaptiveSkinDetector).

This library need native opencv dll (version 2.4.0). To install it on windows I suggest that you use chocolatey.org : 
```bat
cinst OpenCV
cinst tbb
```
You will then need to add the dll path to your environement path :
```
set PATH=%PATH%;C:\OpenCV240\opencv\build\x86\vc10\bin
set PATH=%PATH%;C:\Chocolatey\lib\tbb.4.1.3\tools\tbb41_20130314oss\bin\ia32\vc10
```
Path may vary upon package version


* OpenCV website : http://opencv.org
* OpenCVSharp website : https://code.google.com/p/opencvsharp/
