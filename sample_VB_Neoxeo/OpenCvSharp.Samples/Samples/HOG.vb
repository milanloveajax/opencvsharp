﻿Imports System
Imports System.Collections.Generic
Imports System.Diagnostics
Imports System.Linq
Imports System.Text
Imports OpenCvSharp
Imports OpenCvSharp.CPlusPlus
Imports CPP = OpenCvSharp.CPlusPlus
'using GPU = OpenCvSharp.Gpu;

Namespace OpenCvSharpSamples
	''' <summary>
	''' samples/c/peopledetect.c
	''' </summary>
    Friend Module HOG
        Public Sub Start()
            Dim img As CPP.Mat = CPP.CvCpp.ImRead([Const].ImageAsahiyama, LoadMode.Color)

            '            
            '            if (GPU.CvGpu.IsEnabled)
            '            {
            '                GPU.GpuMat imgGpu = new GPU.GpuMat(img);
            '
            '                GPU.HOGDescriptor hog = new GPU.HOGDescriptor();
            '                hog.SetSVMDetector(OpenCvSharp.CPlusPlus.HOGDescriptor.GetDefaultPeopleDetector());
            '
            '                //bool b = hog.CheckDetectorSize();
            '                //b.ToString();
            '            }
            '            else
            '            //
            Dim hog As New CPP.HOGDescriptor()
            hog.SetSVMDetector(CPP.HOGDescriptor.GetDefaultPeopleDetector())

            Dim b As Boolean = hog.CheckDetectorSize()
            b.ToString()

            Dim watch As Stopwatch = Stopwatch.StartNew()

            ' run the detector with default parameters. to get a higher hit-rate
            ' (and more false alarms, respectively), decrease the hitThreshold and
            ' groupThreshold (set groupThreshold to 0 to turn off the grouping completely).
            Dim found() As CvRect = hog.DetectMultiScale(img, 0, New CvSize(8, 8), New CvSize(24, 16), 1.05, 2)

            watch.Stop()
            Form1.TextBox1.AppendText(Environment.NewLine & String.Format("Detection time = {0}ms", watch.ElapsedMilliseconds))
            Form1.TextBox1.AppendText(Environment.NewLine & String.Format("{0} region(s) found", found.Length))

            For Each rect As CvRect In found
                ' the HOG detector returns slightly larger rectangles than the real objects.
                ' so we slightly shrink the rectangles to get a nicer output.
                Dim r As CvRect = New CvRect With {.X = rect.X + CInt(Math.Truncate(Math.Round(rect.Width * 0.1))), .Y = rect.Y + CInt(Math.Truncate(Math.Round(rect.Height * 0.1))), .Width = CInt(Math.Truncate(Math.Round(rect.Width * 0.8))), .Height = CInt(Math.Truncate(Math.Round(rect.Height * 0.8)))}
                img.Rectangle(r.TopLeft, r.BottomRight, CvColor.Red, 3, LineType.Link8, 0)
            Next rect

            Using window As New CvWindow("people detector", WindowMode.None, img.ToIplImage())
                window.SetProperty(WindowProperty.Fullscreen, 1)
                Cv.WaitKey(0)
            End Using
        End Sub
    End Module
End Namespace