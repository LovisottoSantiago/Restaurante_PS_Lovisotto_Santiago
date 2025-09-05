namespace Application.Interfaces.Query
{
    public interface ICategoryQuery
    {
        Task<bool> ExistsAsync(int categoryId);
    }
}
