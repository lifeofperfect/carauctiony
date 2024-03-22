using AuctionService.DTOs;
using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.Data;

public class AuctionRepository : IAuctionRepository
{
    private readonly AuctionDbContext _context;
    public AuctionRepository(AuctionDbContext context)
    {
        _context = context;
    }
    
    public void AddAuction(Auction auction)
    {
        _context.Auctions.Add(auction);
    }

    public async Task<AuctionDto> GetAuctionByIdAsync(Guid id)
    {
        var response = await _context.Auctions.FirstAsync(x => x.Id == id);

        return new AuctionDto()
        {
            Id = response.Id,
            ReservePrice = response.ReservePrice,
            Seller = response.Seller,
            Winner = response.Winner,
            SoldAmount = response.SoldAmount ?? 0,
            CurrentHighBid = response.CurrentHighBid.Value,
            CreatedAt = response.CreatedAt,
            UpdatedAt = response.UpdatedAt,
            AuctionEnd = response.AuctionEnd,
            Status = response.Status.ToString()

        };
    }
}