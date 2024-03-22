using AuctionService.DTOs;
using AuctionService.Entities;

namespace AuctionService.Data;

public interface IAuctionRepository
{
    void AddAuction(Auction auction);
    Task<AuctionDto> GetAuctionByIdAsync(Guid id);
}