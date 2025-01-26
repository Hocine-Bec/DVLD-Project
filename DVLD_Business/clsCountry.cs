using System;
using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class clsCountry
    {

        public int ID { set; get; }
        public string CountryName { set; get; }
   
        public clsCountry()

        {
            this.ID = -1;
            this.CountryName = "";

        }

        private clsCountry(CountriesDTO countriesDTO)

        {
            this.ID = countriesDTO.ID;
            this.CountryName = countriesDTO.CountryName;
        }

        public static clsCountry Find(int ID)
        {
            var dto = CountriesRepository.GetCountryInfoById(ID);

            return (dto != null) ? new clsCountry(dto) : null;
        }

        public static clsCountry Find(string countryName)
        {
            var dto = CountriesRepository.GetCountryInfoByName(countryName);

            return (dto != null) ? new clsCountry(dto) : null;
        }

        public static DataTable GetAllCountries()
        {
            return CountriesRepository.GetAllCountries();

        }

    }
}
