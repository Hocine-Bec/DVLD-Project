using System;
using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class PersonService
    {
        public enum Mode { AddNew = 0, Update = 1 };
        public Mode CurrentMode { get; set; } = Mode.AddNew;

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }

        public string FullName => $"{FirstName} {SecondName} {ThirdName} {LastName}";

        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gender { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }

        public clsCountry CountryInfo { get; set; }
        public string ImagePath { get; set; }

        public PersonService()
        {
            PersonID = -1;
            FirstName = string.Empty;
            SecondName = string.Empty;
            ThirdName = string.Empty;
            LastName = string.Empty;
            DateOfBirth = DateTime.Now;
            Address = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            NationalityCountryID = -1;
            ImagePath = string.Empty;

            CurrentMode = Mode.AddNew;
        }

        private PersonService(PersonDTO personDTO)
        {
            PersonID = personDTO.PersonID;
            FirstName = personDTO.FirstName;
            SecondName = personDTO.SecondName;
            ThirdName = personDTO.ThirdName;
            LastName = personDTO.LastName;
            NationalNo = personDTO.NationalNo;
            DateOfBirth = personDTO.DateOfBirth;
            Gender = personDTO.Gender;
            Address = personDTO.Address;
            Phone = personDTO.Phone;
            Email = personDTO.Email;
            NationalityCountryID = personDTO.NationalityCountryID;
            ImagePath = personDTO.ImagePath;
            CountryInfo = clsCountry.Find(personDTO.NationalityCountryID);

            CurrentMode = Mode.Update;
        }

        private bool AddNewPerson()
        {
            var personDTO = new PersonDTO
            {
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                ThirdName = this.ThirdName,
                LastName = this.LastName,
                NationalNo = this.NationalNo,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Address = this.Address,
                Phone = this.Phone,
                Email = this.Email,
                NationalityCountryID = this.NationalityCountryID,
                ImagePath = this.ImagePath
            };

            PersonID = PersonRepository.AddNewPerson(personDTO);

            return PersonID != -1;
        }

        private bool UpdatePerson()
        {
            var personDTO = new PersonDTO
            {
                PersonID = this.PersonID,
                FirstName = this.FirstName,
                SecondName = this.SecondName,
                ThirdName = this.ThirdName,
                LastName = this.LastName,
                NationalNo = this.NationalNo,
                DateOfBirth = this.DateOfBirth,
                Gender = this.Gender,
                Address = this.Address,
                Phone = this.Phone,
                Email = this.Email,
                NationalityCountryID = this.NationalityCountryID,
                ImagePath = this.ImagePath
            };

            return PersonRepository.UpdatePerson(personDTO);
        }

        public static PersonService Find(int personID)
        {
            var personDTO = PersonRepository.GetPersonInfoById(personID);

            if (personDTO != null)
            {
                return new PersonService(personDTO);
            }

            return null;
        }

        public static PersonService Find(string nationalNo)
        {
            var personDTO = PersonRepository.GetPersonInfoByNationalNo(nationalNo);

            if (personDTO != null)
            {
                return new PersonService(personDTO);
            }

            return null;
        }

        public bool Save()
        {
            switch (CurrentMode)
            {
                case Mode.AddNew:
                    if (AddNewPerson())
                    {
                        CurrentMode = Mode.Update;
                        return true;
                    }
                    return false;

                case Mode.Update:
                    return UpdatePerson();

                default:
                    return false;
            }
        }

        public static DataTable GetAllPeople()
        {
            return PersonRepository.GetAllPeople();
        }

        public static bool DeletePerson(int id)
        {
            return PersonRepository.DeletePerson(id);
        }

        public static bool IsPersonExist(int id)
        {
            return PersonRepository.IsPersonExist(id);
        }

        public static bool IsPersonExist(string nationalNo)
        {
            return PersonRepository.IsPersonExist(nationalNo);
        }
    }
}