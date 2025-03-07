using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class CountriesRepo : BaseRepoHelper, ICountryRepo
    {
        private readonly AppDbContext _context;
        public CountriesRepo(AppDbContext context,ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<List<Country>?> GetAllCountries()
        {
            return await ExecuteDbOperationAsync(async () => await _context.Countries.ToListAsync());
        }

        public async Task<Country?> GetByCountryName(string countryName)
        {
            if (string.IsNullOrWhiteSpace(countryName))
                throw new ArgumentException($"Country Name {countryName} cannot be null.");

            return await ExecuteDbOperationAsync(async () => await _context.Countries.FirstOrDefaultAsync(
                x => x.CountryName == countryName));
        }

        public async Task<Country?> GetById(int countryId)
        {
            if (countryId <= 0)
                throw new ArgumentException($"Country ID {countryId} must be greater then 0.");

            return await ExecuteDbOperationAsync(async () => await _context.Countries.FindAsync(countryId));
        }
    }

}
