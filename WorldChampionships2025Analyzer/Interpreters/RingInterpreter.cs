using OpenCvSharp;
using WorldChampionships2025Analyzer.Configuration;
using WorldChampionships2025Analyzer.Enums;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class RingInterpreter : InterpreterBase
    {
        private readonly Tournament _tournament;

        public RingInterpreter(Mat src,
							   OcrEngine ocrEngine,
                               Tournament tournament = Tournament.WorldCup2026) : base(src, ocrEngine)
        {
            _tournament = tournament;
        }

        public int ParseRing()
        {
            var roiRing = GetRoiPerTournament();
            var rRing = ScaleFromBase(roiRing, SourceX, SourceY, Source.Size());
            var mat = new Mat(Source, rRing);
            
            //Cv2.ImShow("Debug", mat);
            //Cv2.WaitKey(0);
            
            var ringText = OcrEngine.Read(mat, OcrConfiguration.OcrDigits).Text.TrimEnd();
            
            // Expect "Ring 1" or "Ring1"
            foreach (var tok in ringText.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
				if (int.TryParse(new string([.. tok.Where(char.IsDigit)]), out int ringNumber))
                    return ringNumber;
            }
            return 0;
        }

        private Rect GetRoiPerTournament()
        {
            switch (_tournament)
            {
                case Tournament.WorldCup2026:
                    return new Rect(0, 905, 120, 35);
                case Tournament.WorldChampionship2025:
                    return new Rect(0, 910, 120, 50);
                default:
                    return new Rect(0, 910, 120, 50);
            }
        }
    }
}
