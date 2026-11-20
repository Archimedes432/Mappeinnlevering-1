using System.Security.Claims;
using Mappeinnlevering_1.Controllers;
using Mappeinnlevering_1.DTOs;
using Mappeinnlevering_1.Models.Enums;
using Mappeinnlevering_1.Services;
using Mappeinnlevering_1.Tests.Mocks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mappeinnlevering_1.Tests.Controllers;

public class ControllerTests
{
    private readonly MockListingRepository _listings = new MockListingRepository();
    private readonly MockOfferRepository _offers = new MockOfferRepository();

    // Lager en innlogget bruker
    private static ControllerContext LoggedInAs(string userId)
    {
        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, userId) }, "Test");
        return new ControllerContext { HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal(identity) } };
    }

    // 404: annonsen finnes ikke
    [Fact]
    public async Task GetListing_NotFound_Returns404()
    {
        var controller = new ListingsController(new ListingService(_listings));

        var result = await controller.GetById(999);

        Assert.IsType<NotFoundResult>(result.Result);
    }

    // 403: annen bruker prøver å endre annonsen
    [Fact]
    public async Task UpdateListing_ByOtherUser_Returns403()
    {
        var listing = _listings.Add("selger", 1000);
        var controller = new ListingsController(new ListingService(_listings));
        controller.ControllerContext = LoggedInAs("annen");

        var dto = new UpdateListingDto { Title = "Test", Price = 900, Condition = Condition.Good };
        var result = await controller.Update(listing.Id, dto);

        Assert.IsType<ForbidResult>(result);
    }

    // 400: bud på egen annonse
    [Fact]
    public async Task CreateOffer_OnOwnListing_Returns400()
    {
        var listing = _listings.Add("selger", 1000);
        var controller = new OffersController(new OfferService(_offers, _listings));
        controller.ControllerContext = LoggedInAs("selger");

        var result = await controller.Create(listing.Id, new CreateOfferDto { Amount = 800 });

        Assert.IsType<BadRequestObjectResult>(result.Result);
    }
}
