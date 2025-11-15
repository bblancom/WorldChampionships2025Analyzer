namespace WorldChampionships2025Analyzer
{
	public class CsvHandler
	{
		public CsvHandler(string outputFilePath)
		{
			OutputFilePath = outputFilePath;
		}

		public string OutputFilePath { get; }

		public void WriteToCsv(List<Row> rows)
		{
			using (var writer = new StreamWriter(OutputFilePath))
			{
				// Write CSV header
				writer.WriteLine(Row.Headers);

				// Write each row
				foreach (var row in rows)
				{
					writer.WriteLine(row.ToCsv());
				}
			}

			Console.WriteLine($"CSV written: {Path.GetFullPath(OutputFilePath)}");
		}
	}
}
