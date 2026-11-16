namespace Mappeinnlevering_1.Services;

public abstract class Statistics
{

    // Median: midterste verdi i sortert liste. Ved partall antall verdier brukes snittet av de to midterste.
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

    
    // Kvartiler (Q1, Q3): lineær interpolasjon, samme metode som Excel QUARTILE.INC.
    
    public static double Quartile(List<double> values, double p)
    {
        List<double> sorted = values.OrderBy(v => v).ToList();

        double position = (sorted.Count - 1) * p;
        int lower = (int)Math.Floor(position);
        double fraction = position - lower;

        if (lower + 1 >= sorted.Count)
        {
            return sorted[lower];
        }

        return sorted[lower] + fraction * (sorted[lower + 1] - sorted[lower]);
    }
    
}