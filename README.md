SkinDetector
============

Simple C# library to detect percentage of skin in image. Use opencv and OpenCVSharp to do this (CvAdaptiveSkinDetector).

This library need native opencv dll (version 2.4.0). To install it on windows I suggest that you use chocolatey.org : 
cinst OpenCV
You will then need to add the dll path to your environement path :
set PATH=%PATH%;C:\OpenCV240\opencv\build\x86\vc10\bin