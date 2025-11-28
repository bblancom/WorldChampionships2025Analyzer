using OpenCvSharp;
using WorldChampionships2025Analyzer.Configuration;

namespace WorldChampionships2025Analyzer.tests
{
	public class RoiConfigurationTests
	{
		[Fact]
		public void CheckDefaultConfig_GetBlueScoreRectByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1, 49, 63, 40);

			Assert.Equal(expectedResult, configuration.GetBlueScoreRectByRing(new Rect(1, 1, 1, 1)));
		}

		[Fact]
		public void CheckDefaultConfig_GetRedScoreRectByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1, 0, 63, 40);

			Assert.Equal(expectedResult, configuration.GetRedScoreRectByRing(new Rect(1, 1, 1, 1)));
		}

		[Fact]
		public void CheckDefaultConfig_GetTimeRoiByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(150, 900, 90, 30);

			Assert.Equal(expectedResult, configuration.TimeRoi);
		}

		[Fact]
		public void CheckDefaultConfig_GetDateRoiByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(140, 920, 110, 40);

			Assert.Equal(expectedResult, configuration.DateRoi);
		}

		[Fact]
		public void CheckDefaultConfig_GetRoiWarningsRedByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1590, 1000, 50, 35);

			Assert.Equal(expectedResult, configuration.RoiWarningsRed);
		}

		[Fact]
		public void CheckDefaultConfig_GetRoiWarningsBlueByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1590, 1050, 50, 35);

			Assert.Equal(expectedResult, configuration.RoiWarningsBlue);
		}

		[Fact]
		public void CheckDefaultConfig_GetRoiFoulsRedByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1880, 1000, 50, 35);

			Assert.Equal(expectedResult, configuration.RoiFoulsRed);
		}

		[Fact]
		public void CheckDefaultConfig_GetRoiFoulsBlueByRing_Test()
		{
			var configuration = new RoiConfiguration(31);
			var expectedResult = new Rect(1880, 1045, 50, 35);

			Assert.Equal(expectedResult, configuration.RoiFoulsBlue);
		}
	}
}
