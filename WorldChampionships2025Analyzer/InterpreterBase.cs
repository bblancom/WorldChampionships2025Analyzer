using OpenCvSharp;
using Microsoft.Extensions.Configuration;
using WorldChampionships2025Analyzer.Configuration;
using WorldChampionships2025Analyzer.Static_data;

namespace WorldChampionships2025Analyzer
{
    public class InterpreterBase
	{
		public InterpreterBase(Mat src, OcrEngine ocrEngine, int ringNumber = 1)
		{
			Source = src;
			OcrEngine = ocrEngine;
			InterpreterConfiguration = new RoiConfiguration(ringNumber);
			Init();
		}

		private void Init()
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			IConfiguration config = builder.Build();

			if (config != null)
			{
				var value = config["DefaultDatetime"];

				if (!string.IsNullOrEmpty(value))
					DefaultDatetime = DateTime.ParseExact(value, "yyyy-MM-dd", null);

				TessDataPath = config["TessDataPath"] ?? "./tessdata";

				BaseWidth = int.Parse(config["BaseWidth"]);
				BaseHeight = int.Parse(config["BaseHeight"]);
			}
		}

		public string TessDataPath { get; private set; } // ensure tessdata/eng.traineddata exists

		public Mat Source { get; private set; }

		public int BaseWidth { get; private set; }

		public int BaseHeight { get; private set; }

		// Scale ROIs if the image resolution differs from base
		public double SourceX => Source.Width / (double) BaseWidth;
		public double SourceY => Source.Height / (double) BaseHeight;

		public OcrEngine OcrEngine { get; private set; }
		public RoiConfiguration InterpreterConfiguration { get; }
		public DateTime DefaultDatetime { get; private set; }

		public Rect ScaleFromBase(Rect r, double sx, double sy, Size imgSize)
		{
			int x = Clamp((int)Math.Round(r.X * sx), 0, imgSize.Width - 1);
			int y = Clamp((int)Math.Round(r.Y * sy), 0, imgSize.Height - 1);
			int w = Clamp((int)Math.Round(r.Width * sx), 1, imgSize.Width - x);
			int h = Clamp((int)Math.Round(r.Height * sy), 1, imgSize.Height - y);

			return new Rect(x, y, w, h);
		}

		static int Clamp(int v, int lo, int hi) => Math.Max(lo, Math.Min(hi, v));
	}
}
