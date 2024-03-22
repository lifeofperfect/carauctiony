using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly AuctionDbContext _ctx;
    public AuctionsController(AuctionDbContext ctx)
    {
        _ctx = ctx;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAuctions()
    {
        var auctions = await _ctx.Auctions.ToListAsync();
        return Ok(auctions);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAuctionById(Guid id)
    {
        var auction = await _ctx.Auctions.FirstOrDefaultAsync(x => x.Id == id);
        return Ok(auction);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAuction(CreateAuctionDto request)
    {

        var auction = new Auction()
        {
            ReservePrice = request.ReservePrice,
            Seller = "test",
            AuctionEnd = request.AuctionEnd,
            Item = new Item()
            {
                Make = request.Make,
                Model = request.Model,
                Color = request.Color,
                ImageUrl = request.ImageUrl,
                Mileage = request.Mileage,
                Year = request.Year,
            }
        };

        _ctx.Auctions.Add(auction);

        var result = await _ctx.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the db");
        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, new Auction());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuction(Guid id, UpdateAuctionDto request)
    {
        var auction = await _ctx.Auctions.Include(x => x.Item)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (auction == null) return NotFound();

        auction.Item.Make = request.Make ?? auction.Item.Make;
        auction.Item.Color = request.Color ?? auction.Item.Color;
        auction.Item.Model = request.Model ?? auction.Item.Model;
        auction.Item.Mileage = request.Mileage ?? auction.Item.Mileage;
        auction.Item.Year = request.Year ?? auction.Item.Year;

        var result = await _ctx.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Something wrong");
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuction(Guid id)
    {
        var auction = await _ctx.Auctions.FirstOrDefaultAsync(x => x.Id == id);

        if (auction is null) return NotFound();

        _ctx.Auctions.Remove(auction);
        var result = await _ctx.SaveChangesAsync() > 0;
        if (result) return Ok();

        return BadRequest("not deleted");
    }
}