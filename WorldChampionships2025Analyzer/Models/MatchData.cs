using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldChampionships2025Analyzer.Models
{
	public class MatchData
	{
		public string Datetime { get; set; } = "";
		public int Ring { get; set; }
		public string Category { get; set; } = "";
		public string TournamentRound { get; set; } = "";
		public string Round { get; set; } = "";
		public CompetitorInfo BlueCompetitor { get; set; } = new CompetitorInfo();
		public CompetitorInfo RedCompetitor { get; set; } = new CompetitorInfo();
		public Scoreboard Scoreboard { get; set; } = new Scoreboard();
	}
}
