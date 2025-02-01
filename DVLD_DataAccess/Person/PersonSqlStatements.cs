namespace DVLD_DataAccess
{
    public static class PersonSqlStatements
    {
        public const string GetByNationalNo = "SELECT * FROM People WHERE NationalNo = @nationalNo";

        public const string GetByPersonId = "SELECT * FROM People WHERE PersonID = @PersonID";

        public const string AddNewPerson = @"
            INSERT INTO People 
            (
                FirstName, SecondName, ThirdName, LastName, NationalNo, DateOfBirth, Gender, 
                Address, Phone, Email, NationalityCountryID, ImagePath
            )
            VALUES 
            (
                @FirstName, @SecondName, @ThirdName, @LastName, @NationalNo, @DateOfBirth, @Gender, 
                @Address, @Phone, @Email, @NationalityCountryID, @ImagePath
            );
            SELECT SCOPE_IDENTITY();";

        public const string UpdatePerson = @"
            UPDATE People  
            SET 
                FirstName = @FirstName,
                SecondName = @SecondName,
                ThirdName = @ThirdName,
                LastName = @LastName, 
                NationalNo = @NationalNo,
                DateOfBirth = @DateOfBirth,
                Gender = @Gender,
                Address = @Address,  
                Phone = @Phone,
                Email = @Email, 
                NationalityCountryID = @NationalityCountryID,
                ImagePath = @ImagePath
            WHERE PersonID = @PersonID";

        public const string GetAllPeople = @"
            SELECT 
                People.PersonID, People.NationalNo,
                People.FirstName, People.SecondName, People.ThirdName, People.LastName,
                People.DateOfBirth, People.Gender,  
                CASE
                    WHEN People.Gender = 0 THEN 'Male'
                    ELSE 'Female'
                END AS GenderCaption,
                People.Address, People.Phone, People.Email, 
                People.NationalityCountryID, Countries.CountryName, People.ImagePath
            FROM People 
            INNER JOIN Countries ON People.NationalityCountryID = Countries.CountryID
            ORDER BY People.FirstName";

        public const string DeletePerson = "DELETE FROM People WHERE PersonID = @PersonID";

        public const string IsPersonExistById = "SELECT Found = 1 FROM People WHERE PersonID = @PersonID";

        public const string IsPersonExistByNationalNo = "SELECT Found = 1 FROM People WHERE PersonID = @nationalNo";
    }

}
