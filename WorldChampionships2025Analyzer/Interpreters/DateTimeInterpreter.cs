using OpenCvSharp;
using System.Globalization;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class DateTimeInterpreter : InterpreterBase
	{
		public DateTimeInterpreter(Mat src, OcrEngine ocrEngine, int ringNumber = 1) : base(src, ocrEngine)
		{
			Init(ringNumber);
		}

		private void Init(int ringNumber)
		{
			// Set date & time ROI
			switch (ringNumber)
			{
				case 1:
					RoiDate = new Rect(140, 920, 110, 40);
					RoiTime = new Rect(150, 900, 90, 30);
					break;
				case 6:
					// No Datetime
					break;
				default:
					RoiDate = new Rect(140, 920, 110, 40);
					RoiTime = new Rect(150, 900, 90, 30);
					break;
			}
		}

		private Rect RoiTime { get; set; }
		private Rect? RoiDate { get; set; }

		public DateTime? ParseDateTime()
		{
			// Means there is no value for this specific ring
			if (!RoiDate.HasValue) return DefaultDatetime;

			var rTime = ScaleFromBase(RoiTime, SourceX, SourceY, Source.Size());
			var rDate = ScaleFromBase(RoiDate.Value, SourceX, SourceY, Source.Size());
			var timeMat = new Mat(Source, rTime);
			var dateMat = new Mat(Source, rDate);

			//Cv2.ImShow("Debug", timeMat);
			//Cv2.WaitKey(0);

			var timeText = OcrEngine.Read(timeMat, OcrDigits).Text;
			var dateText = OcrEngine.Read(dateMat, OcrDigits).Text;

			// Expect time like "16:38" and date like "10.10.2025."
			string time = ExtractTime(timeText);
			string date = ExtractDate(dateText);
			if (time == "" || date == "") return null;

			if (DateTime.TryParseExact($"{date} {time}", "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dt))
				return dt;

			// alternate with trailing dot removed
			if (DateTime.TryParseExact($"{date.Replace(".", "/")} {time}", "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
				return dt;

			return null;
		}

		private string ExtractTime(string value)
		{
			// keep "HH:MM"
			var digits = new string(value.Where(ch => char.IsDigit(ch) || ch == ':').ToArray());
			if (digits.Length >= 4 && digits.Contains(':'))
			{
				var parts = digits.Split(':');
				if (parts.Length >= 2 && parts[0].Length <= 2 && parts[1].Length >= 2)
					return $"{parts[0].PadLeft(2, '0')}:{parts[1].Substring(0, 2)}";
			}
			return "";
		}

		private string ExtractDate(string value)
		{
			// keep "DD.MM.YYYY" (strip trailing dot)
			var digits = new string(value.Where(ch => char.IsDigit(ch) || ch == '.').ToArray());
			if (digits.Count(ch => ch == '.') >= 2)
			{
				var segs = digits.Split('.', StringSplitOptions.RemoveEmptyEntries);
				if (segs.Length >= 3 && segs[0].Length <= 2 && segs[1].Length <= 2 && segs[2].Length >= 4)
				{
					return $"{segs[0].PadLeft(2, '0')}.{segs[1].PadLeft(2, '0')}.{segs[2].Substring(0, 4)}";
				}
			}
			return "";
		}
	}
}
