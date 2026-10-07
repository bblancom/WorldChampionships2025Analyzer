using OpenCvSharp;
using WorldChampionships2025Analyzer.Enums;

namespace WorldChampionships2025Analyzer.Configuration
{
    public class RoiConfiguration
    {
        private readonly int _ringNumber;
        
        private readonly Tournament _tournament;

        /// <summary>
        /// Constructor for RoiConfiguration.
        /// </summary>
        /// <param name="ringNumber">
        /// Ring or area id number.
        /// </param>
        /// <param name="tournament">
        /// Tournament config.
        /// </param>
        public RoiConfiguration(int ringNumber, Tournament tournament)
        {
	        _ringNumber = ringNumber;
	        _tournament = tournament;
        }

        #region Region of interest public properties
        public Rect BlueCompetitorInfoRoi => GetBlueRoiByRingNumber();

        public Rect RedCompetitorInfoRoi => GetRedRoiByRingNumber();

        public Rect DifferentialsPanelRoi => GetRoiStackPanelByRing();

        public Rect[] UmpiresDifferentialsRois => GetUmpiresRoisByRing();

        public Rect? TimeRoi => GetTimeRoiByRing();
        public Rect? DateRoi => GetDateRoiByRing();

        public Rect GetRedScoreRectByRing(Rect rect)
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(rect.X, 0, 63, 35),
                2 => new Rect(rect.X, 0, 63, 35),
				_ => new Rect(rect.X, 0, 63, 40),
            };
        }

        public Rect GetBlueScoreRectByRing(Rect rect)
        {
            return _ringNumber switch
            {
                6 => new Rect(rect.X, 45, 63, 40),
                7 => new Rect(rect.X, 45, 63, 40),
                _ => new Rect(rect.X, 49, 63, 40),
            };
        }

        public Rect RoiWarningsRed => GetRoiWarningsRedByRing();
        public Rect RoiWarningsBlue => GetRoiWarningsBlueByRing();
        public Rect RoiFoulsRed => GetRoiFoulsRedByRing();
        public Rect RoiFoulsBlue => GetRoiFoulsBlueByRing();

		public Scalar LowerRedScalar => new(0, 100, 100);
        public Scalar LowerRedScalar2 => new(160, 100, 100);
		public Scalar UpperRedScalar => new(10, 255, 255);
		public Scalar UpperRedScalar2 => new(180, 255, 255);

        public Scalar LowerBlueScalar => new(85, 80, 120);
        public Scalar UpperBlueScalar => new(105, 255, 255);

		#endregion

		#region Private methods for ROIs
		/// <summary>
		/// Get blue competitor info region of interest.
		/// </summary>
		/// <returns>
		/// Rect for roi.
		/// </returns>
		private Rect GetBlueRoiByRingNumber()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(725, 980, 450, 130),
                2 => new Rect(750, 960, 480, 150),
                _ => new Rect(750, 960, 480, 150),
            };
        }

        /// <summary>
        /// Get red competitor info region of interest.
        /// </summary>
        /// <returns>
        /// Rect for roi.
        /// </returns>
        private Rect GetRedRoiByRingNumber()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(60, 980, 450, 130),
                2 => new Rect(60, 960, 450, 150),
                _ => new Rect(60, 960, 450, 150),
            };
        }

        /// <summary>
        /// Get differentials panel region of interest.
        /// </summary>
        /// <returns>
        /// Rect for roi.
        /// </returns>
        private Rect GetRoiStackPanelByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(1370, 880, 270, 90),
                1 when _tournament == Tournament.WorldChampionship2025 => new Rect(1600, 900, 450, 90),
                2 => new Rect(1620, 890, 450, 90),
                6 => new Rect(1640, 920, 450, 90),
                _ => new Rect(1640, 900, 450, 90),
            };
        }

        /// <summary>
        /// Get blue competitor info region of interest.
        /// </summary>
        /// <returns>
        /// Rect for roi.
        /// </returns>
        private Rect[] GetUmpiresRoisByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => [
		            new Rect(0, 0, 67, 90),
		            new Rect(68, 0, 67, 90),
		            new Rect(135, 0, 67, 90),
		            new Rect(203, 0, 67, 90),
	            ],
                1 when _tournament == Tournament.WorldChampionship2025 => [
                            new Rect(0, 0, 68, 90),
                            new Rect(69, 0, 68, 90),
                            new Rect(137, 0, 68, 90),
                            new Rect(205, 0, 68, 90),
                        ],
				2 => [
					        new Rect(0, 0, 68, 90),
							new Rect(74, 0, 68, 90),
							new Rect(143, 0, 68, 90),
							new Rect(213, 0, 65, 90),
						],
				6 => [
                            new Rect(0, 0, 68, 85),
                            new Rect(69, 0, 68, 90),
                            new Rect(137, 0, 68, 90),
                            new Rect(205, 0, 68, 90),
                        ],
                7 => [
                            new Rect(0, 0, 68, 90),
                            new Rect(69, 0, 60, 90),
                            new Rect(137, 0, 60, 90),
                            new Rect(200, 0, 60, 90),
                        ],
                _ => [
                            new Rect(0, 0, 68, 90),
                            new Rect(69, 0, 68, 90),
                            new Rect(137, 0, 68, 90),
                            new Rect(205, 0, 68, 90),
                        ],
            };
        }

        private Rect? GetTimeRoiByRing()
        {
			return _ringNumber switch
			{
				1 => (Rect?)new Rect(150, 900, 90, 30),
				2 => (Rect?)new Rect(150, 905, 60, 30),
				6 => null,// No Datetime
				7 => (Rect?)new Rect(150, 900, 95, 25),
				_ => (Rect?)new Rect(150, 900, 90, 30),
			};
		}

        private Rect? GetDateRoiByRing()
        {
			return _ringNumber switch
			{
				1 => (Rect?)new Rect(140, 920, 110, 40),
				2 => (Rect?)new Rect(140, 930, 110, 40),
				6 => null,// No Datetime
				7 => (Rect?)new Rect(140, 930, 115, 25),
				_ => (Rect?)new Rect(140, 920, 110, 40),
			};
		}

        private Rect GetRoiWarningsRedByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(1560, 990, 50, 35),
                2 => new Rect(1530, 990, 50, 35),
				_ => new Rect(1590, 1000, 50, 35),
            };
        }

        private Rect GetRoiWarningsBlueByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(1560, 1040, 50, 35),
                2 => new Rect(1530, 1035, 50, 35),
				7 => new Rect(1590, 1000, 50, 25),
                _ => new Rect(1590, 1050, 50, 35),
            };
        }

        private Rect GetRoiFoulsRedByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(1850, 990, 50, 35),
                2 => new Rect(1830, 990, 50, 35),
				_ => new Rect(1880, 1000, 50, 35),
            };
        }

        private Rect GetRoiFoulsBlueByRing()
        {
            return _ringNumber switch
            {
	            1 when _tournament == Tournament.WorldCup2026 => new Rect(1850, 1040, 50, 35),
                2 => new Rect(1830, 1035, 50, 35),
				7 => new Rect(1880, 1045, 50, 25),
                _ => new Rect(1880, 1045, 50, 35),
            };
        }

        #endregion
    }
}