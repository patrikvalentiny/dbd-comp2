using MongoDB.Driver;

namespace listings_service.Infrastructure.Services
{
    public interface IMongoTransactionManager
    {
        Task<IClientSessionHandle> BeginTransactionAsync();
        Task CommitTransactionAsync(IClientSessionHandle session);
        Task AbortTransactionAsync(IClientSessionHandle session);
    }
}
