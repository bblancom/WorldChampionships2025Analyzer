using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldChampionships2025Analyzer
{
	public class Row
	{
		public static string Headers =>
			"Datetime,Ring,Blue,Blue Country,Blue Total Warnings,Blue total fouls,Blue IsWinner,Red,Red Country,Red Total Warnings,Red total fouls,Red IsWinner,Category,Round,Umpire 1 diff,Umpire 2 diff,Umpire 3 diff,Umpire 4 diff,Net diff";

		public string Datetime { get; set; } = "";
		public int Ring { get; set; }
		public string Blue { get; set; } = "";
		public string BlueCountry { get; set; } = "";
		public string BlueWarnings { get; set; } = "0";
		public string BlueFouls { get; set; } = "0";
		public string BlueIsWinner { get; set; } = "Draw";
		public string Red { get; set; } = "";
		public string RedCountry { get; set; } = "";
		public string RedWarnings { get; set; } = "0";
		public string RedFouls { get; set; } = "0";
		public string RedIsWinner { get; set; } = "Draw";
		public string Category { get; set; } = "";
		public string TournamentRound { get; set; } = "TournamentRound?";
		public string Round { get; set; } = "Round?";
		public int U1 { get; set; }
		public int U2 { get; set; }
		public int U3 { get; set; }
		public int U4 { get; set; }
		public int Net { get; set; }

		public string ToCsv()
		{
			// unquoted fields, as per your format preference
			return string.Join(",",
				Datetime, Ring, Blue, BlueCountry, BlueWarnings, BlueFouls, BlueIsWinner,
				Red, RedCountry, RedWarnings, RedFouls, RedIsWinner,
				Category, Round, U1, U2, U3, U4, Net);
		}
	}
}
