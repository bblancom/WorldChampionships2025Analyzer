using OpenCvSharp;
using WorldChampionships2025Analyzer.Interpreters;
using WorldChampionships2025Analyzer.Models;

namespace WorldChampionships2025Analyzer
{
	public class ComputerVisionUtils
	{
		private readonly int? matchRing;

		public ComputerVisionUtils(Mat src, OcrEngine ocr, string category)
		{
			RingInterpreter = new RingInterpreter(src, ocr);
			BlueCompetitorInterpreter = new FighterBoxInterpreter(src, ocr, Competitor.Blue);
			RedCompetitorInterpreter = new FighterBoxInterpreter(src, ocr, Competitor.Red);
			DifferentialsInterpreter = new DifferentialsInterpreter(src, ocr, MatchRing);
			DateTimeInterpreter = new DateTimeInterpreter(src, ocr, MatchRing);
			FoulsWarningsInterpreter = new FoulsWarningsInterpreter(src, ocr);
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
		public DateTime? MatchDateTime => DateTimeInterpreter.ParseDateTime();
		public int MatchRing => matchRing ?? RingInterpreter.ParseRing();
		public int[] MatchDifferentials => DifferentialsInterpreter.ReadStackDiffs();
		public (bool blueWin, bool redWin) MatchWinnerLoser => DifferentialsInterpreter.InferWinner(MatchDifferentials);

		public MatchData CaptureMatchData()
		{
			// We have to identify ring number first in order to set all configurations based on that...
			var ring = MatchRing;
			var (blueWin, redWin) = MatchWinnerLoser;
			var diffs = MatchDifferentials;
			var dt = MatchDateTime;
			var round = MatchRound;

			var blueName = BlueCompetitorInterpreter.Name;
			var blueCountry = BlueCompetitorInterpreter.Country;

			var redName = RedCompetitorInterpreter.Name;
			var redCountry = RedCompetitorInterpreter.Country;

			var warningsRedText = FoulsWarningsInterpreter.WarningsRed;
			var foulsRedText = FoulsWarningsInterpreter.FoulsRed;
			var warningsBlueText = FoulsWarningsInterpreter.WarningsBlue;
			var foulsBlueText = FoulsWarningsInterpreter.FoulsBlue;

			var retVal = new MatchData()
			{
				Datetime = MatchDateTime?.ToString("yyyy-MM-dd HH:mm") ?? "",
				Ring = ring,
				BlueCompetitor = new CompetitorInfo()
				{
					Name = blueName,
					Country = blueCountry,
					Warnings = warningsBlueText,
					Fouls = foulsBlueText,
					IsWinner = blueWin ? "Yes" : (redWin ? "No" : "Draw"),
				},
				RedCompetitor = new CompetitorInfo()
				{
					Name = redName,
					Country = redCountry,
					Warnings = warningsRedText,
					Fouls = foulsRedText,
					IsWinner = redWin ? "Yes" : (blueWin ? "No" : "Draw"),
				},
				Category = Category,
				Round = MatchRound,
				TournamentRound = "",
				Scoreboard = new Scoreboard()
				{
					UmpireDifferentials = MatchDifferentials,
					NetDifferential = diffs.Sum(),
				}
			};

			return retVal;
		}
	}
}
