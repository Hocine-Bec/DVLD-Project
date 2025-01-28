namespace DVLD_DataAccess
{
    public static class CountrySqlStatements
    {
        public const string GetById = "SELECT * FROM Countries WHERE CountryID = @CountryID";
        public const string GetByName = "SELECT * FROM Countries WHERE CountryName = @CountryName";
        public const string GetAllCountries = "SELECT * FROM Countries ORDER BY CountryName";
    }
   

}
