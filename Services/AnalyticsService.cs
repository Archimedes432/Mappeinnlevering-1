using Mappeinnlevering_1.Models;
using Mappeinnlevering_1.Repositories.Interfaces;
using Mappeinnlevering_1.Services.Interfaces;

namespace Mappeinnlevering_1.Services;

public class AnalyticsService : IAnalyticsService
{
  private readonly IAnalyticsRepository _repo;

  public AnalyticsService(IAnalyticsRepository repo)
  {
    _repo = repo;
  }
}