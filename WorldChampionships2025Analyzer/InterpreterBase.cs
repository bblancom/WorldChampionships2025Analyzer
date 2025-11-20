using OpenCvSharp;
using Microsoft.Extensions.Configuration;

namespace WorldChampionships2025Analyzer
{
	public class InterpreterBase
	{
		public InterpreterBase(Mat src, OcrEngine ocrEngine)
		{
			Source = src;
			OcrEngine = ocrEngine;
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
			}
		}

		// Tesseract data path (put tessdata folder here). You can use an absolute path.
		public string TessDataPath => "C:\\Users\\bruno\\source\\repos\\WorldChampionships2025Analyzer\\WorldChampionships2025Analyzer\\bin\\Debug\\net8.0\\tesssdata"; // ensure tessdata/eng.traineddata exists

		public Mat Source { get; private set; }

		// Image resolution you capture from (example: 1920x1080). ROIs below assume 1920x1080.
		// If your images differ, adjust ROIs or scale them using ScaleFromBase function.
		const int BaseW = 1920;
		const int BaseH = 1080;

		// Scale ROIs if the image resolution differs from base
		public double SourceX => Source.Width / (double)BaseW;
		public double SourceY => Source.Height / (double)BaseH;

		// OCR presets
		public OcrPreset OcrDefault => new OcrPreset
		{
			Psm = Tesseract.PageSegMode.SingleLine,
			Oem = Tesseract.EngineMode.LstmOnly,
			Whitelist = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,-:/0123456789"
		};

		public OcrPreset OcrDigits => new OcrPreset
		{
			Psm = Tesseract.PageSegMode.SingleLine,
			Oem = Tesseract.EngineMode.LstmOnly,
			Whitelist = "0123456789:-./"
		};

		public OcrEngine OcrEngine { get; private set; }
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
