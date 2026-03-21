using Application.Interfaces;
using System.Text.Json;

namespace Persistence.Services
{
    public class CountryService : ICountryService
    {
        public async Task<List<Country>> GetAllAsync()
        {
            var path = Path.Combine(AppContext.BaseDirectory, "Data", "countries.json");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var json = await File.ReadAllTextAsync(path);

            return JsonSerializer.Deserialize<List<Country>>(json, options) ?? new List<Country>();
        }
    }
}