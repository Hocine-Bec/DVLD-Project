using DVLD_DataAccess.Core.Entities;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface ICountryRepo
    {
        public Task<Country?> GetById(int countryId);
        public Task<Country?> GetByCountryName(string countryName);
        public Task<List<Country>?> GetAllCountries();
    }
}
