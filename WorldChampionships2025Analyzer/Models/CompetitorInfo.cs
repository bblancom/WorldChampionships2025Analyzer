namespace WorldChampionships2025Analyzer.Models
{
	public class CompetitorInfo
	{
		public string Name { get; set; } = "";
		public string Country { get; set; } = "";
		public string Warnings { get; set; } = "0";
		public string Fouls { get; set; } = "0";
		public string IsWinner { get; set; } = "Draw";
	}
}
