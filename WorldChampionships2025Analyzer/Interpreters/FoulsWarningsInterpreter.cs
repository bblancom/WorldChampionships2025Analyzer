using OpenCvSharp;
using Tesseract;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class FoulsWarningsInterpreter : InterpreterBase
	{
		private readonly int _ringNumber;

		public FoulsWarningsInterpreter(Mat src, OcrEngine ocrEngine, int ringNumber = 1) : base(src, ocrEngine)
		{
			_ringNumber = ringNumber;
		}

		public string WarningsBlue => ExtractIntFromRoi(WarnBlueMat, Competitor.Blue);
		public string FoulsBlue => ExtractIntFromRoi(FoulsBlueMat, Competitor.Blue);
		public string WarningsRed => ExtractIntFromRoi(WarnRedMat, Competitor.Red);
		public string FoulsRed => ExtractIntFromRoi(FoulsRedMat, Competitor.Red);

		private OpenCvSharp.Rect RoiWarningsRed => GetRoiWarningsRedByRing();
		private OpenCvSharp.Rect RoiWarningsBlue => GetRoiWarningsBlueByRing();
		private OpenCvSharp.Rect RoiFoulsRed => GetRoiFoulsRedByRing();
		private OpenCvSharp.Rect RoiFoulsBlue => GetRoiFoulsBlueByRing();

		private OpenCvSharp.Rect RFoulsRed => ScaleFromBase(RoiFoulsRed, SourceX, SourceY, Source.Size());
		private OpenCvSharp.Rect RWarnBlue => ScaleFromBase(RoiWarningsBlue, SourceX, SourceY, Source.Size());
		private OpenCvSharp.Rect RFoulsBlue => ScaleFromBase(RoiFoulsBlue, SourceX, SourceY, Source.Size());
		private OpenCvSharp.Rect RWarnRed => ScaleFromBase(RoiWarningsRed, SourceX, SourceY, Source.Size());

		private Mat WarnRedMat => new(Source, RWarnRed);
		private Mat FoulsRedMat => new(Source, RFoulsRed);
		private Mat WarnBlueMat => new(Source, RWarnBlue);
		private Mat FoulsBlueMat => new(Source, RFoulsBlue);

		public string ExtractIntFromRoi(Mat cropped, Competitor competitor)
		{
			//Cv2.ImShow("debug", cropped);
			//Cv2.WaitKey(0);

			// 1) Upscale before operations
			Cv2.Resize(cropped, cropped, new Size(), 3, 3, InterpolationFlags.Lanczos4);

			// Optional: isolate cyan text (HSV range for cyan)
			using var hsv = new Mat();
			Cv2.CvtColor(cropped, hsv, ColorConversionCodes.BGR2HSV);

			if (competitor == Competitor.Red)
			{
				var lowerRed = new Scalar(0, 100, 100);
				var lowerRed2 = new Scalar(160, 100, 100);   // tune!
															 //Scalar lower = isRed? new Scalar(0, 100, 100) : new Scalar(85, 40, 120);   // tune!
				var upperRed = new Scalar(10, 255, 255);
				var upperRed2 = new Scalar(180, 255, 255);

				using var mask = new Mat();
				using var mask2 = new Mat();
				using var finalMask = new Mat();

				Cv2.InRange(hsv, lowerRed, upperRed, mask);
				Cv2.InRange(hsv, lowerRed2, upperRed2, mask2);

				Cv2.BitwiseOr(mask, mask2, finalMask);
				Cv2.BitwiseNot(finalMask, mask);

				//Cv2.ImShow("debug", mask);
				//Cv2.WaitKey(0);

				var (text, confidence) = OcrEngine.Read(mask, new OcrPreset { Psm = PageSegMode.SingleChar, Whitelist = "0123456789-" });
				var retVal = text.Contains('-') || string.IsNullOrEmpty(text) ? "0" : text.TrimEnd();

				return retVal;
			}
			else
			{
				using var mask = new Mat();
				var lower = new Scalar(85, 80, 120);
				var upper = new Scalar(105, 255, 255);

				Cv2.InRange(hsv, lower, upper, mask);
				Cv2.BitwiseNot(mask, mask);

				//Cv2.ImShow("debug", mask);
				//Cv2.WaitKey(0);

				var (text, confidence) = OcrEngine.Read(mask, new OcrPreset { Psm = PageSegMode.SingleChar, Whitelist = "0123456789-" });
				var retVal = text.Contains('-') || string.IsNullOrEmpty(text)? "0" : text.TrimEnd();

				return retVal;
			}

		}

		private OpenCvSharp.Rect GetRoiWarningsRedByRing()
		{
			switch (_ringNumber)
			{
				default:
					return new(1590, 1000, 50, 35);
			}
		}

		private OpenCvSharp.Rect GetRoiWarningsBlueByRing()
		{
			switch (_ringNumber)
			{
				case 7:
					return new(1590, 1000, 50, 25);
				default:
					return new(1590, 1050, 50, 35);
			}
		}

		private OpenCvSharp.Rect GetRoiFoulsRedByRing()
		{
			switch (_ringNumber)
			{
				default:
					return new(1880, 1000, 50, 35);
			}
		}

		private OpenCvSharp.Rect GetRoiFoulsBlueByRing()
		{
			switch (_ringNumber)
			{
				case 7:
					return new(1880, 1045, 50, 25);
				default:
					return new(1880, 1045, 50, 35);
			}
		}
	}
}
