namespace Application.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetAllAsync();
    }
}
