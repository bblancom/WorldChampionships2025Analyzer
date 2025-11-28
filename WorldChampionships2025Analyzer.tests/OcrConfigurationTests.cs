using WorldChampionships2025Analyzer.Configuration;

namespace WorldChampionships2025Analyzer.tests
{
	public class OcrConfigurationTests
	{
		[Fact]
		public void DigitsIncluded_Test()
		{
			var digits = OcrConfiguration.OcrDigits.Whitelist.ToCharArray();

			Assert.Contains(['0', '1', '2', '3', '4', '5', '6', '7', '8', '9'], digits);
		}
	}
}
