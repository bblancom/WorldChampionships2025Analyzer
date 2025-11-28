using OpenCvSharp;
using System.Data;
using Tesseract;
using WorldChampionships2025Analyzer.Static_data;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class FighterBoxInterpreter : InterpreterBase
	{
		public FighterBoxInterpreter(Mat src,
									 OcrEngine ocr,
									 Competitor competitor,
									 int ringNumber = 1) : base(src, ocr, ringNumber)
		{
			var rBlue = ScaleFromBase(InterpreterConfiguration.BlueCompetitorInfoRoi, SourceX, SourceY, src.Size());
			var rRed = ScaleFromBase(InterpreterConfiguration.RedCompetitorInfoRoi, SourceX, SourceY, src.Size());
			var bgr = competitor == Competitor.Blue
					? new Mat(src, rBlue)
					: new Mat(src, rRed);

			//Cv2.ImShow("Debug", bgr);
			//Cv2.WaitKey(0);

			var text = ReadNameBlock(bgr);
			ParseNameCountryLines(text);
		}

		public string Name { get; set; }

		public string Country { get; set; }

		public string ReadNameBlock(Mat bgr)
		{
			using var roi = bgr.Clone();

			// 1) Upscale
			Cv2.Resize(roi, roi, new Size(), 2.0, 2.0, InterpolationFlags.Cubic);

			// 2) HSV -> isolate white text (low S, high V)
			using var hsv = new Mat();
			Cv2.CvtColor(roi, hsv, ColorConversionCodes.BGR2HSV);
			var whiteMask = new Mat();
			Cv2.InRange(hsv, new Scalar(0, 0, 200), new Scalar(180, 70, 255), whiteMask);

			// 3) Clean/connect strokes
			var k = Cv2.GetStructuringElement(MorphShapes.Rect, new Size(3, 3));
			Cv2.MorphologyEx(whiteMask, whiteMask, MorphTypes.Close, k);

			// 4) Invert for OCR
			var forOcr = new Mat();
			Cv2.BitwiseNot(whiteMask, forOcr);

			// 5) OCR with two PSMs; take best/longest
			var t1 = OcrEngine.Read(forOcr, new OcrPreset { Psm = PageSegMode.SingleBlock, Whitelist = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,-'" }).Text;
			var t2 = OcrEngine.Read(forOcr, new OcrPreset { Psm = PageSegMode.SparseText, Whitelist = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz .,-'" }).Text;

			return CleanupBlock(t1.Length >= t2.Length ? t1 : t2);
		}

		private string CleanupBlock(string value)
		{
			var valueWithoutJumpLines = value.Replace('\r', '\n');
			var valueWithoutTabs = System.Text.RegularExpressions.Regex.Replace(valueWithoutJumpLines, "[ \t]+", " ");
			//value = valueWithoutTabs.Replace("–", "-").Replace("—", "-");
			var retVal = string.Join("\n", value.Split('\n')
							   .Select(line => line.Trim())
							   .Where(line => line.Length > 0));

			return retVal;
		}

		private void ParseNameCountryLines(string text)
		{
			var lines = text.Split('\n')
							.Select(x => x.Trim())
							.Where(x => x.Length > 0)
							.ToList();

			if (lines.Count == 0) return;

			var stringApproximationUtils = new StringApproximation();

			// Try detect country explicitly
			for (int i = lines.Count - 1; i >= 0; i--)
			{
				var cleanedLine = StripNonLetters(lines[i]).ToUpperInvariant();
				var (match, similarity) = stringApproximationUtils.FindBestMatch(cleanedLine, [.. StringComparisonDataSets.CountrySet]);

				if (similarity > 50)
				{
					Country = match;
					break;
				}
				else
				{
					var federationExpected = string.Concat(lines.Slice(1, lines.Count - 2));
					var (fedMatch, fedSimilarity) = stringApproximationUtils.FindBestMatch(federationExpected, [.. StringComparisonDataSets.FederationSet.Keys]);

					if (fedSimilarity > 50)
					{
						Country = StringComparisonDataSets.FederationSet.GetValueOrDefault(fedMatch, "");

						break;
					}
				}

				Country = CleanCountryLine(match);
			}

			// Find the first line that looks like a name and not federation
			for (int i = 0; i < lines.Count; i++)
			{
				var (name, similarityName) = stringApproximationUtils.FindBestMatch(lines[i], [.. StringComparisonDataSets.NameSet]);

				// Not so high similarity, after all should be quite accurate to pick one
				if (similarityName > 40)
				{
					Name = NormalizeName(name);
					break;
				}
			}

			// Fallbacks
			if (string.IsNullOrEmpty(Name)) Name = NormalizeName(lines.First());
			if (string.IsNullOrEmpty(Country)) Country = GuessCountryFromLines(lines);
		}

		private string NormalizeName(string name)
		{
			// Title-case only if input is all-caps; otherwise keep as-is
			if (name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
			{
				return string.Join(" ", name.ToLowerInvariant().Split(' ', StringSplitOptions.RemoveEmptyEntries)
					.Select(w => char.ToUpper(w[0]) + (w.Length > 1 ? w.Substring(1) : "")));
			}
			return name.Trim();
		}

		private string CleanCountryLine(string rawCountryLine)
		{
			if (string.IsNullOrWhiteSpace(rawCountryLine)) return "";

			// 1) Keep only letters, spaces, and hyphens
			string filtered = new string(rawCountryLine
				.Where(ch => char.IsLetter(ch) || ch == ' ' || ch == '-')
				.ToArray()).Trim();

			// Normalize multiple spaces and hyphens
			filtered = System.Text.RegularExpressions.Regex.Replace(filtered, @"\s+", " ");
			filtered = filtered.Replace(" - ", "-").Replace("–", "-").Replace("—", "-").Trim();

			if (filtered.Length == 0) return "";

			// 2) If line contains a known country anywhere, extract that substring
			// Try longest country names first to avoid partial matches
			var byLengthDesc = StringComparisonDataSets.CountrySet.OrderByDescending(c => c.Length);

			foreach (var cname in byLengthDesc)
			{
				var idx = filtered.IndexOf(cname, StringComparison.OrdinalIgnoreCase);

				if (idx >= 0)
				{
					// Ensure we are on token boundaries
					var leftOk = idx == 0 || !char.IsLetter(filtered[idx - 1]);
					var end = idx + cname.Length;
					var rightOk = end >= filtered.Length || !char.IsLetter(filtered[end]);

					if (leftOk && rightOk)
						return cname.ToUpperInvariant();
				}
			}

			// 3) Token-based heuristic: drop short noise tokens from the start
			var tokens = filtered.Split(' ', StringSplitOptions.RemoveEmptyEntries).ToList();

			// Remove up to first two short tokens if they look like flag noise
			var guard = 0;

			while (tokens.Count > 1 && guard++ < 2)
			{
				var firstToken = tokens[0];

				if (StringComparisonDataSets.LikelyFlagNoise.Contains(firstToken) || firstToken.Length <= 2)
					tokens.RemoveAt(0);
				else
					break;
			}

			var candidate = string.Join(" ", tokens);

			// 4) Final normalization: if all caps and length reasonable, accept as country
			var upper = candidate.ToUpperInvariant();
			if (upper.Length <= 24 && upper.All(ch => char.IsUpper(ch) || ch == ' ' || ch == '-'))
				return upper;

			return upper; // fallback: return the cleaned line
		}

		private string GuessCountryFromLines(List<string> lines)
		{
			foreach (var line in lines.AsEnumerable().Reverse())
			{
				var cleanLine = StripNonLetters(line).ToUpperInvariant();

				if (StringComparisonDataSets.CountrySet.Contains(cleanLine)) return cleanLine;

				if (IsAllCapsAlpha(cleanLine) && cleanLine.Length <= 20) return cleanLine;
			}

			return null;
		}

		private string StripNonLetters(string value)
		{
			return new(value.Where(character => char.IsLetter(character) || character == ' ' || character == '-').ToArray());
		}

		private bool IsAllCapsAlpha(string value)
		{
			return value.Length > 0 && value.All(character => char.IsUpper(character) || character == ' ' || character == '-');
		}
	}
}
