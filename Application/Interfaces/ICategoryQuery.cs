namespace Application.Interfaces
{
    public interface ICategoryQuery
    {
        Task<bool> ExistsAsync(int id);
    }
}
