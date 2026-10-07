using OpenCvSharp;
using Tesseract;

namespace WorldChampionships2025Analyzer
{
	public class OcrEngine : IDisposable
	{
		private readonly TesseractEngine _engine;

		public OcrEngine(string tessDataPath) => _engine = new TesseractEngine(tessDataPath, "eng", EngineMode.LstmOnly);

		public (string Text, float Confidence) Read(Mat img, OcrPreset preset)
		{
			Cv2.ImEncode(".png", img, out byte[] pngBytes);
			using var pix = Pix.LoadFromMemory(pngBytes);
			
			_engine.DefaultPageSegMode = preset.Psm;

			if (!string.IsNullOrEmpty(preset.Whitelist))
			{
				_engine.SetVariable("tessedit_char_whitelist", preset.Whitelist);
			}

			using var page = _engine.Process(pix);
			var text = page.GetText();
			var conf = page.GetMeanConfidence();

			return (text, conf);
		}

		public void Dispose() => _engine?.Dispose();
	}
}
