using OpenCvSharp;
using System.Globalization;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class DateTimeInterpreter : InterpreterBase
	{
		public DateTimeInterpreter(Mat src,
								   OcrEngine ocrEngine,
								   int ringNumber = 1) : base(src, ocrEngine, ringNumber)
		{
			RoiTime = InterpreterConfiguration.TimeRoi;
			RoiDate = InterpreterConfiguration.DateRoi;
		}

		private Rect? RoiTime { get; set; }
		private Rect? RoiDate { get; set; }

		public DateTime? ParseDateTime()
		{
			// Means there is no value for this specific ring
			if (!RoiDate.HasValue) return DefaultDatetime;

			var rTime = ScaleFromBase(RoiTime.Value, SourceX, SourceY, Source.Size());
			var rDate = ScaleFromBase(RoiDate.Value, SourceX, SourceY, Source.Size());
			var timeMat = new Mat(Source, rTime);
			var dateMat = new Mat(Source, rDate);

			//Cv2.ImShow("Debug", timeMat);
			//Cv2.WaitKey(0);

			//Cv2.ImShow("Debug", dateMat);
			//Cv2.WaitKey(0);

			var timeText = OcrEngine.Read(timeMat, Configuration.OcrConfiguration.OcrDigits).Text;
			var dateText = OcrEngine.Read(dateMat, Configuration.OcrConfiguration.OcrDigits).Text;

			if (string.IsNullOrEmpty(timeText))
			{
				Console.Error.WriteLine($"Warning: OCR Engine could not read time from picture. Value:[{timeText}]");
				return null;
			}

			if (string.IsNullOrEmpty(dateText))
			{
				Console.Error.WriteLine($"Warning: OCR Engine could not read date from picture. Value:[{dateText}]");
				return null;
			}

			// Expect time like "16:38" and date like "10.10.2025."
			var time = ExtractTime(timeText);
			var date = ExtractDate(dateText);

			if (string.IsNullOrEmpty(time))
			{
				Console.Error.WriteLine($"Warning: Unable to extract time from OCR text '{timeText}'");
				return null;
			}

			if (string.IsNullOrEmpty(date))
			{
				Console.Error.WriteLine($"Warning: Unable to extract date from OCR text '{dateText}'");
				return null;
			}

			if (string.IsNullOrEmpty(time) || string.IsNullOrEmpty(date)) return null;

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
					return $"{parts[0].PadLeft(2, '0')}:{parts[1][..2]}";
			}

			// For some reason, did not understand the :
			if (digits.Length == 4)
			{
				return $"{digits[0]}{digits[1]}:{digits[2]}{digits[3]}";
			}

			if (digits.Length == 3)
			{
				return $"{digits[0]}:{digits[1]}{digits[2]}";
			}

			return "";
		}

		private string ExtractDate(string value)
		{
			var dateSeparator = value.Contains('.') ? '.' : '/';
			// keep "DD.MM.YYYY" (strip trailing dot)
			var digits = new string([.. value.Where(ch => char.IsDigit(ch) || ch == dateSeparator)]).TrimEnd('.');

			// TODO: Rework feature using regexp
			if (digits.Count(ch => ch == dateSeparator) == 2)
			{
				var segs = digits.Split(dateSeparator, StringSplitOptions.RemoveEmptyEntries);

				if (segs.Length >= 3 && segs[0].Length <= 2 && segs[1].Length <= 2 && segs[2].Length >= 4)
				{
					return $"{segs[0].PadLeft(2, '0')}.{segs[1].PadLeft(2, '0')}.{segs[2][..4]}";
				}
			}

			return "";
		}
	}
}
