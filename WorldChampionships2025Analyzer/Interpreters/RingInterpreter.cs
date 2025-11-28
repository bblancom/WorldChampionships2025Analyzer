using OpenCvSharp;
using WorldChampionships2025Analyzer.Configuration;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class RingInterpreter : InterpreterBase
    {
        public RingInterpreter(Mat src,
							   OcrEngine ocrEngine) : base(src, ocrEngine)
        {
        }

        public int ParseRing()
        {
            var RoiRing = new Rect(0, 910, 120, 50);           // "Ring 1"
            var rRing = ScaleFromBase(RoiRing, SourceX, SourceY, Source.Size());
            var mat = new Mat(Source, rRing);
            var ringText = OcrEngine.Read(mat, OcrConfiguration.OcrDigits).Text;

            // Expect "Ring 1" or "Ring1"
            foreach (var tok in ringText.Split(' ', StringSplitOptions.RemoveEmptyEntries))
            {
				if (int.TryParse(new string([.. tok.Where(char.IsDigit)]), out int r))
                    return r;
            }
            return 0;
        }
    }
}
