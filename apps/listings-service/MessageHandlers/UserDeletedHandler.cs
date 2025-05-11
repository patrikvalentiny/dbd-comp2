using listings_service.Repositories;
using ListingsService.Repositories;
using MessageClient.Interfaces;
using Messages;
using MongoDB.Driver;

namespace listings_service.MessageHandlers;

public class UserDeletedHandler : IMessageHandler<UserDeleted>
{
    private readonly IListingRepository _listingRepository;
    private readonly ILogger<UserDeletedHandler> _logger;

    public UserDeletedHandler(IListingRepository listingRepository, ILogger<UserDeletedHandler> logger)
    {
        _listingRepository = listingRepository;
        _logger = logger;
    }

    public async Task Handler(UserDeleted message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling user deletion for user {UserId}", message.UserId);

        var userListings = await _listingRepository.GetBySellerIdAsync(message.UserId.ToString());
            
            foreach (var listing in userListings)
            {
                await _listingRepository.DeleteAsync(listing.Id);
                _logger.LogInformation("Deleted listing {ListingId} for deleted user {UserId}", 
                    listing.Id, message.UserId);
            }
    }
}
