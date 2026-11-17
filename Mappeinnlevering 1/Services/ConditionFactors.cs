using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.Services;

    // Tilstand på produkt => pris
public static class ConditionFactors
{
    public static double Get(Condition condition)
    {
        switch (condition)
        {
            case Condition.New: return 1.00;
            case Condition.LikeNew: return 0.85;
            case Condition.Good: return 0.70;
            case Condition.Used: return 0.55;
            case Condition.Defect: return 0.30;
            default:
                throw new
                    ArgumentOutOfRangeException(nameof(condition));
            
        }
    }

        // Normalisert pris
    public static double Normalize(int price, Condition condition)
    {
        return price / Get(condition);
    }
}