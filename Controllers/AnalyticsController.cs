using Mappeinnlevering_1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Mappeinnlevering_1.Controllers;



public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsService _analytics;

    public AnalyticsController(IAnalyticsService analytics)
    {
        _analytics = analytics;
    }
}