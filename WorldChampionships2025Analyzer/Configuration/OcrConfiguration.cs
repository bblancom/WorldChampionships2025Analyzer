namespace WorldChampionships2025Analyzer.Configuration
{
	public class OcrConfiguration
	{
		public static OcrPreset OcrDefault => new()
		{
			Psm = Tesseract.PageSegMode.SingleLine,
			Oem = Tesseract.EngineMode.LstmOnly,
			Whitelist = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,-:/0123456789"
		};

		public static OcrPreset OcrDigits => new()
		{
			Psm = Tesseract.PageSegMode.SingleLine,
			Oem = Tesseract.EngineMode.LstmOnly,
			Whitelist = "0123456789:-./"
		};

	}
}
