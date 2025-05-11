namespace ReviewsService.CQRS.Queries
{
    public interface IGetAverageRatingQuery
    {
        Task<double> ExecuteAsync(Guid targetId, string targetType);
    }
}
