using OpenCvSharp;
using System.Threading.Tasks;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class DifferentialsInterpreter : InterpreterBase
	{
		public DifferentialsInterpreter(Mat src, OcrEngine ocrEngine, int ringNumber) : base(src, ocrEngine, ringNumber)
		{
		}

		public int[] ReadStackDiffs()
		{
			var rStack = ScaleFromBase(InterpreterConfiguration.DifferentialsPanelRoi, SourceX, SourceY, Source.Size());
			var stackMat = new Mat(Source, rStack);

			//Cv2.ImShow("Debug", stackMat);
			//Cv2.WaitKey(0);

			// Convert to HSV and threshold for red and blue
			using var hsv = new Mat();
			Cv2.CvtColor(stackMat, hsv, ColorConversionCodes.BGR2HSV);

			// Red has two hue ranges
			var redMask1 = new Mat();
			var redMask2 = new Mat();
			Cv2.InRange(hsv, new Scalar(0, 120, 70), new Scalar(10, 255, 255), redMask1);
			Cv2.InRange(hsv, new Scalar(170, 120, 70), new Scalar(180, 255, 255), redMask2);
			var redMask = new Mat();
			Cv2.BitwiseOr(redMask1, redMask2, redMask);

			// Blue
			var blueMask = new Mat();
			var lowerb = new Scalar(90, 60, 50);
			var upperb = new Scalar(115, 255, 255);
			Cv2.InRange(hsv, lowerb, upperb, blueMask);

			// Clean masks
			var kOpen = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
			var kClose = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(7, 3)); // wider horizontally
			var kErodeV = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(1, 3));
			Cv2.MorphologyEx(blueMask, blueMask, MorphTypes.Open, kOpen);
			Cv2.MorphologyEx(blueMask, blueMask, MorphTypes.Close, kClose);
			Cv2.Erode(blueMask, blueMask, kErodeV, iterations: 1);

			//Cv2.ImShow("Debug", blueMask);
			//Cv2.WaitKey(0);

			//Cv2.ImShow("Debug", redMask);
			//Cv2.WaitKey(0);

			var diffs = new List<int>();

			foreach (var rect in InterpreterConfiguration.UmpiresDifferentialsRois)
			{
				//Cv2.ImShow("Debug", stackMat[rect]);
				//Cv2.WaitKey(0);

				var redScoreRect = InterpreterConfiguration.GetRedScoreRectByRing(rect);
				var blueScoreRect = InterpreterConfiguration.GetBlueScoreRectByRing(rect);

				//Cv2.ImShow("Debug", redMask[redScoreRect]);
				//Cv2.WaitKey(0);

				//Cv2.ImShow("Debug", blueMask[blueScoreRect]);
				//Cv2.WaitKey(0);

				var redCount = CountHorizontalBars(redMask[redScoreRect]);
				var blueCount = CountHorizontalBars(blueMask[blueScoreRect]);

				if (redCount > 0 && blueCount > 0)
				{
					// Something is wrong
					Console.Error.WriteLine("Error counting scores");
				}

				diffs.Add(redCount - blueCount);
			}

			return [.. diffs];
		}

		private int CountHorizontalBars(Mat mask)
		{
			// Count connected components that look like horizontal bars
			int rectangleHeightSum = 0;

			Cv2.Threshold(mask, mask, 127, 255, ThresholdTypes.Binary);
			Cv2.FindContours(mask, out var contours, out _, RetrievalModes.External, ContourApproximationModes.ApproxSimple);

			foreach (var contour in contours)
			{
				var height = Cv2.BoundingRect(contour).Height;
				var width = Cv2.BoundingRect(contour).Width;

				rectangleHeightSum += height;
			}

			//if (rectangleHeightSum >= 27 && rectangleHeightSum <= 35)
			if (rectangleHeightSum >= 27)
			{
				return 4;
			}

			if (rectangleHeightSum >= 20 && rectangleHeightSum <= 26)
			{
				return 3;
			}

			if (rectangleHeightSum >= 9 && rectangleHeightSum <= 19)
			{
				return 2;
			}

			if (rectangleHeightSum >= 1 && rectangleHeightSum <= 8)
			{
				return 1;
			}

			return 0;
		}

		public (bool blueWin, bool redWin) InferWinner(int[] diffs)
		{
			var blue = diffs.Count(d => d < 0);
			var red = diffs.Count(d => d > 0);

			if (blue > red) return (true, false);
			if (red > blue) return (false, true);

			return (false, false);
		}
	}
}
