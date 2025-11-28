using OpenCvSharp;

namespace WorldChampionships2025Analyzer.Configuration
{
    public class RoiConfiguration
    {
        private readonly int _ringNumber;

        public RoiConfiguration(int ringNumber) => _ringNumber = ringNumber;

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
                1 => new Rect(1640, 900, 450, 90),
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
                1 => [
                            new Rect(0, 0, 68, 90),
                            new Rect(69, 0, 68, 90),
                            new Rect(137, 0, 68, 90),
                            new Rect(205, 0, 68, 90),
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
            switch (_ringNumber)
            {
                case 1:
                    return new Rect(150, 900, 90, 30);
                case 2:
                    return new Rect(150, 905, 60, 30);
                case 6:
                    // No Datetime
                    return null;
                case 7:
                    return new Rect(150, 900, 95, 25);
                default:
                    return new Rect(150, 900, 90, 30);
            }
        }

        private Rect? GetDateRoiByRing()
        {
            switch (_ringNumber)
            {
                case 1:
                    return new Rect(140, 920, 110, 40);
                case 2:
                    return new Rect(140, 930, 110, 40);
                case 6:
                    // No Datetime
                    return null;
                case 7:
                    return new Rect(140, 930, 115, 25);
                default:
                    return new Rect(140, 920, 110, 40);
            }
        }

        private Rect GetRoiWarningsRedByRing()
        {
            return _ringNumber switch
            {
                _ => new(1590, 1000, 50, 35),
            };
        }

        private Rect GetRoiWarningsBlueByRing()
        {
            return _ringNumber switch
            {
                7 => new(1590, 1000, 50, 25),
                _ => new(1590, 1050, 50, 35),
            };
        }

        private Rect GetRoiFoulsRedByRing()
        {
            return _ringNumber switch
            {
                _ => new(1880, 1000, 50, 35),
            };
        }

        private Rect GetRoiFoulsBlueByRing()
        {
            return _ringNumber switch
            {
                7 => new(1880, 1045, 50, 25),
                _ => new(1880, 1045, 50, 35),
            };
        }

        #endregion
    }
}