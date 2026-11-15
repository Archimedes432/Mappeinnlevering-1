namespace Mappeinnlevering_1.Services;

public class Statistics
{

    public static double Median(List<double> values)
    {
        List<double> sorted = values.OrderBy(v => v).ToList();
        int mid = sorted.Count / 2;

        if (sorted.Count % 2 == 0)
        {
            return (sorted[mid - 1] + sorted[mid]) / 2.0;
        }

        return sorted[mid];
    }
}