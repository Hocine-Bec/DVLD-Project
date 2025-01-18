using System;
using System.Data;
using System.Xml.Linq;
using DVLD.DTOs;
using DVLD_DataAccess;


namespace DVLD_Business
{

    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }

        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public short Gender { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }

        public clsCountry CountryInfo;

        public string ImagePath;
      
        public clsPerson()
        {
            this.PersonID = -1;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.ImagePath = "";

            Mode = enMode.AddNew;
        }

        private clsPerson(PersonDTO personDTO)

        {
            this.PersonID = personDTO.PersonID;
            this.FirstName = personDTO.FirstName;
            this.SecondName = personDTO.SecondName;
            this.ThirdName = personDTO.ThirdName;
            this.LastName = personDTO.LastName;
            this.NationalNo = personDTO.NationalNo;
            this.DateOfBirth = personDTO.DateOfBirth;
            this.Gender = personDTO.Gender;
            this.Address = personDTO.Address;
            this.Phone = personDTO.Phone;
            this.Email = personDTO.Email;
            this.NationalityCountryID = personDTO.NationalityCountryID;
            this.ImagePath = personDTO.ImagePath;
            this.CountryInfo = clsCountry.Find(personDTO.NationalityCountryID);
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
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

            this.PersonID = clsPersonData.AddNewPerson(personDTO);

            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
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


            return clsPersonData.UpdatePerson(personDTO);

        }

        public static clsPerson Find(int personID)
        {

            var personDTO = clsPersonData.GetPersonInfoById(personID);

            if (personDTO != null)
            {
                return new clsPerson(personDTO);
            }

            return null;
        }

        public static clsPerson Find(string nationalNo)
        {
            var personDTO = clsPersonData.GetPersonInfoByNationalNo(nationalNo);

            if (personDTO != null)
            {
                return new clsPerson(personDTO);
            }

            return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID); 
        }

        public static bool isPersonExist(int ID)
        {
           return clsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationalNo)
        {
            return clsPersonData.IsPersonExist(NationalNo);
        }

    }
}
