using OpenCvSharp;
using WorldChampionships2025Analyzer.Interpreters;
using WorldChampionships2025Analyzer.Models;

namespace WorldChampionships2025Analyzer
{
	public class ComputerVisionUtils
	{
		private int? matchRing;
		private int[] matchDifferentials;
		private DateTime? matchDateTime;

		public ComputerVisionUtils(Mat src, OcrEngine ocr, string category)
		{
			RingInterpreter = new RingInterpreter(src, ocr);
			BlueCompetitorInterpreter = new FighterBoxInterpreter(src, ocr, Competitor.Blue, MatchRing);
			RedCompetitorInterpreter = new FighterBoxInterpreter(src, ocr, Competitor.Red, MatchRing);
			DifferentialsInterpreter = new DifferentialsInterpreter(src, ocr, MatchRing);
			DateTimeInterpreter = new DateTimeInterpreter(src, ocr, MatchRing);
			FoulsWarningsInterpreter = new FoulsWarningsInterpreter(src, ocr, MatchRing);
			MatchRoundInterpreter = new MatchRoundInterpreter(src, ocr);
			Category = category;
		}

		public DifferentialsInterpreter DifferentialsInterpreter { get; }
		public RingInterpreter RingInterpreter { get; }
		public DateTimeInterpreter DateTimeInterpreter { get; }
		public FighterBoxInterpreter BlueCompetitorInterpreter { get; }
		public FighterBoxInterpreter RedCompetitorInterpreter { get; }
		public FoulsWarningsInterpreter FoulsWarningsInterpreter { get; }
		public MatchRoundInterpreter MatchRoundInterpreter { get; }
		public string Category { get; }
		public string MatchRound => MatchRoundInterpreter.ExtractRound();
		public DateTime? MatchDateTime => matchDateTime ??= DateTimeInterpreter.ParseDateTime();
		public int MatchRing => matchRing ??= RingInterpreter.ParseRing();
		public int[] MatchDifferentials => matchDifferentials ??= DifferentialsInterpreter.ReadStackDiffs();
		public (bool blueWin, bool redWin) MatchWinnerLoser => DifferentialsInterpreter.InferWinner(MatchDifferentials);

		public MatchData CaptureMatchData()
		{
			// We have to identify ring number first in order to set all configurations based on that...
			var (blueWin, redWin) = MatchWinnerLoser;

			var blueCompetitor = new CompetitorInfo()
			{
				Name = BlueCompetitorInterpreter.Name,
				Country = BlueCompetitorInterpreter.Country,
				Warnings = FoulsWarningsInterpreter.WarningsBlue,
				Fouls = FoulsWarningsInterpreter.FoulsBlue,
				IsWinner = blueWin ? "Yes" : (redWin ? "No" : "Draw"),
			};

			var redCompetitor = new CompetitorInfo()
			{
				Name = RedCompetitorInterpreter.Name,
				Country = RedCompetitorInterpreter.Country,
				Warnings = FoulsWarningsInterpreter.WarningsRed,
				Fouls = FoulsWarningsInterpreter.FoulsRed,
				IsWinner = redWin ? "Yes" : (blueWin ? "No" : "Draw"),
			};
			
			var retVal = new MatchData()
			{
				Datetime = MatchDateTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
				Ring = MatchRing,
				BlueCompetitor = blueCompetitor,
				RedCompetitor = redCompetitor,
				Category = Category,
				Round = MatchRound,
				TournamentRound = "FirstRound?",
				Scoreboard = new Scoreboard()
				{
					UmpireDifferentials = MatchDifferentials,
					NetDifferential = MatchDifferentials.Sum(),
				}
			};

			return retVal;
		}
	}
}
