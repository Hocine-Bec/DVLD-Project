using DVLD_DataAccess.Core.Enums;

namespace DVLD_DataAccess.Core.Entities
{
    public class Person
    {
        public int PersonId { get; set; }
        public required string FirstName { get; set; }
        public string? SecondName { get; set; }
        public string? ThirdName { get; set; }
        public required string LastName { get; set; }
        public required string NationalNo { get; set; }
        public required DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public int CountryId { get; set; }
        public string? ImagePath { get; set; }
    }
}
