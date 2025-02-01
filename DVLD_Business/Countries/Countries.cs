using System;
using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class Countries
    {
        public int Id { set; get; }
        public string CountryName { set; get; }
   
        public Countries()
        {
            this.Id = -1;
            this.CountryName = "";
        }

        private Countries(CountriesDTO countriesDTO)
        {
            this.Id = countriesDTO.Id;
            this.CountryName = countriesDTO.CountryName;
        }

        public static Countries Find(int id)
        {
            var dto = CountriesRepository.GetCountryInfoById(id);

            return (dto != null) ? new Countries(dto) : null;
        }

        public static Countries Find(string countryName)
        {
            var dto = CountriesRepository.GetCountryInfoByName(countryName);

            return (dto != null) ? new Countries(dto) : null;
        }

        public static DataTable GetAllCountries() => CountriesRepository.GetAllCountries();

    }
}
