using Mappeinnlevering_1.Models.Enums;

namespace Mappeinnlevering_1.DTOs;

public class AnalyticsDtos
{
}

// A. prisforslag
public class PriceSuggestionDto
{
    public int CategoryId { get; set;  }
    public Condition Condition { get; set;  }
    public bool HasEnoughData { get; set;  }
    public int? SuggestedPrice { get; set;  }
    public int NumberOfSales { get; set;  }
    public string? Message { get; set;  }
    
    
}

// B. Prisvurdering
public enum PriceRating
{
    Low,
    Normal,
    High,
    Unknown
}

public class PriceEvaluationDto
{
    public int ListingId { get; set; }
    public PriceRating Rating { get; set; }
    public double NormalizedPrice { get; set; }
    public double? LowerLimit { get; set; }
    public double? UpperLimit { get; set; }
    public int NumberOfSales { get; set; }
    
    
}