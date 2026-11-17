using Mappeinnlevering_1.Services;

namespace Mappeinnlevering_1.Tests.Analytics;

public class StatisticsTests
{
    // 1. Median med oddetall 3 verdier (3, 1, 2)
    [Fact]
    public void Median_OddCount_ReturnsMiddleValue()
    {
        // sortert: 1, 2, 3 -> midterste er 2
        List<double> data = new List<double> { 3, 1, 2 };
        Assert.Equal(2, Statistics.Median(data));
    }

    // 1. Median med partall 4 verdier (4, 1, 3, 2)
    [Fact]
    public void Median_EvenCount_ReturnsAverageOfTwoMiddle()
    {
        // sortert: 1, 2, 3, 4 -> (2 + 3) / 2 = 2.5
        List<double> data = new List<double> { 4, 1, 3, 2 };
        Assert.Equal(2.5, Statistics.Median(data));
    }

    // 2. Q1 og Q3 stemmer med QUARTILE.INC for et gitt datasett
    [Fact]
    public void Quartiles_MatchExcelQuartileInc()
    {
        // Excel: =QUARTILE.INC({7;15;36;39;40;41};1) = 20.25 og ;3) = 39.75
        // Q1: posisjon (6-1)*0.25 = 1.25 -> 15 + 0.25*(36-15) = 20.25
        // Q3: posisjon (6-1)*0.75 = 3.75 -> 39 + 0.75*(40-39) = 39.75
        List<double> data = new List<double> { 7, 15, 36, 39, 40, 41 };

        Assert.Equal(20.25, Statistics.Quartile(data, 0.25), 6);
        Assert.Equal(39.75, Statistics.Quartile(data, 0.75), 6);
    }
}