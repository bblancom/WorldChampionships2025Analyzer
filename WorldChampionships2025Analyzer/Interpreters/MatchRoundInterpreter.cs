using OpenCvSharp;

namespace WorldChampionships2025Analyzer.Interpreters
{
	public class MatchRoundInterpreter : InterpreterBase
    {
        public MatchRoundInterpreter(Mat src, OcrEngine ocrEngine) : base(src, ocrEngine) { }

        public string ExtractRound()
        {
            // For now, hardcoded
            return "R?";
        }
    }
}
