using System.Data;
using OpenCvSharp;
using WorldChampionships2025Analyzer;

class Program
{
	// ============================
	// CONFIG: Set once per stream
	// ============================
	// Output CSV name
	const string OutputCsv = "sparring_results.csv";

	// Tesseract data path (put tessdata folder here). You can use an absolute path.
	static readonly string TessDataPath = "/Users/bblancom/Repos/WorldChampionships2025Analyzer/WorldChampionships2025Analyzer/tesssdata"; // ensure tessdata/eng.traineddata exists

	// ============================
	// MAIN
	// ============================

	static void Main(string[] args)
	{
		// Fixed category (you can change per batch run)
		string category = "Individual Female Sparring over 60 to 65 kg";

		if (args.Length == 0)
		{
			Console.WriteLine("Usage:");
			Console.WriteLine("  dotnet run -- <image_or_folder_path> [category]");
			Console.WriteLine("Examples:");
			Console.WriteLine("  dotnet run -- frames/");
			Console.WriteLine("  dotnet run -- videoframe_11060506.png \"Individual Female Sparring over 60 to 65 kg\"");
			return;
		}

		string inputPath = args[0];
		if (args.Length >= 2)
		{
			// Optional category override
			category = args[1];
		}

		if (!Directory.Exists(TessDataPath))
		{
			Console.WriteLine($"ERROR: tessdata not found at {Path.GetFullPath(TessDataPath)}");
			return;
		}

		var filePaths = GetInputFiles(inputPath);

		// Not a file path, then the param was rubbish
		if (filePaths == null || filePaths.Count == 0) return;

		var rows = GenerateRows(filePaths, category);

		// Write CSV
		var csvHandler = new CsvHandler(OutputCsv);

		csvHandler.WriteToCsv(rows);
	}

	private static List<string> GetInputFiles(string inputPath)
	{
		List<string> retVal = [];

		if (string.IsNullOrEmpty(inputPath))
		{
			Console.WriteLine("Input path is empty.");
			return retVal;
		}

		if (Directory.Exists(inputPath))
		{
			retVal = [.. Directory.EnumerateFiles(inputPath)
							 .Where(IsImage)
							 .OrderBy(f => f)];
		}
		else if (File.Exists(inputPath))
		{
			retVal.Add(inputPath);
		}

		return retVal;
	}

	private static List<Row> GenerateRows(List<string> filePaths, string category)
	{
		List<Row> rows = [];

		using var ocr = new OcrEngine(TessDataPath);

		foreach (var filePath in filePaths)
		{
			Mat src = Cv2.ImRead(filePath);

			if (src.Empty())
			{
				Console.WriteLine($"Skipping unreadable image: {filePath}");
				continue;
			}

			var computerVisionUtils = new ComputerVisionUtils(src, ocr, category);
			var matchData = computerVisionUtils.CaptureMatchData();

			// Build row
			var row = new Row
			{
				Datetime = matchData.Datetime,
				Ring = matchData.Ring,
				Category = matchData.Category,
				Round = matchData.Round,
				Blue = matchData.BlueCompetitor.Name,
				BlueCountry = matchData.BlueCompetitor.Country,
				BlueWarnings = matchData.BlueCompetitor.Warnings,
				BlueFouls = matchData.BlueCompetitor.Fouls,
				BlueIsWinner = matchData.BlueCompetitor.IsWinner,
				Red = matchData.RedCompetitor.Name,
				RedCountry = matchData.RedCompetitor.Country,
				RedWarnings = matchData.RedCompetitor.Warnings,
				RedFouls = matchData.RedCompetitor.Fouls,
				RedIsWinner = matchData.RedCompetitor.IsWinner,
				U1 = matchData.Scoreboard.UmpireDifferentials[0],
				U2 = matchData.Scoreboard.UmpireDifferentials[1],
				U3 = matchData.Scoreboard.UmpireDifferentials[2],
				U4 = matchData.Scoreboard.UmpireDifferentials[3],
				Net = matchData.Scoreboard.NetDifferential
			};

			rows.Add(row);
			Console.WriteLine($"{Path.GetFileName(filePath)} → {row.Blue} vs {row.Red} :: {row.U1},{row.U2},{row.U3},{row.U4} → {row.BlueIsWinner}/{row.RedIsWinner}");
		}

		// Order by datetime (unknowns last)
		var retVal = rows.OrderBy(r => string.IsNullOrEmpty(r.Datetime) ? "9999-99-99 99:99" : r.Datetime).ToList();

		return retVal;
	}

	static bool IsImage(string path)
	{
		string ext = Path.GetExtension(path).ToLowerInvariant();
		return ext is ".png" or ".jpg" or ".jpeg" or ".bmp" or ".tiff";
	}
}