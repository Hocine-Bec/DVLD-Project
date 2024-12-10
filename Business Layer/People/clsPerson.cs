using Database_Layer;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Security.Policy;

namespace clsPerson_Layer
{

    public class clsPerson
    {
        public enum enMode { AddNew = 1, Update = 2 }

        enMode Mode;

        public clsPersonInfo person { get; set; }

        public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string ThirdName { get; set; }
        public string LastName { get; set; }
        public string NationalNo { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        public clsPerson(clsPersonInfo person)
        {
            this.PersonID = person.PersonID;
            this.FirstName = person.FirstName;
            this.SecondName = person.SecondName;
            this.ThirdName = person.ThirdName;
            this.LastName = person.LastName;
            this.NationalNo = person.NationalNo;
            this.DateOfBirth = person.DateOfBirth;
            this.Gendor = person.Gendor;
            this.Address = person.Address;
            this.Phone = person.Phone;
            this.Email = person.Email;
            this.NationalityCountryID = person.NationalityCountryID;
            this.ImagePath = person.ImagePath;

            Mode = enMode.Update;
        }

        public clsPerson() 
        {
            Mode = enMode.AddNew; 
        }


        //Find methods for 3 operations: personID, NationalNo or Firstname..., or using filter option
        public static clsPerson Find(int ID)
        {
            var person = new clsPersonInfo();

            if (clsPersonDataLayer.Find(ID, ref person))
            {
                return new clsPerson(person);
            }

            return null;
        }
        public static clsPerson Find(string name)
        {
            var person = new clsPersonInfo();

            if (clsPersonDataLayer.Find(name, ref person))
            {
                return new clsPerson(person);
            }

            return null;
        }
        public static clsPerson Find(string Filter, string name)
        {
            var person = new clsPersonInfo();

            if (clsPersonDataLayer.Find(Filter, name, ref person))
            {
                return new clsPerson(person);
            }

            return null;
        }


        //Check if the person exists or not
        public static bool IsPersonExist(int ID)
        {
            return clsPersonDataLayer.IsPersonExist(ID);
        }
        public static bool IsPersonExist(string No)
        {
            return clsPersonDataLayer.IsPersonExist(No);
        }


        private bool _AddNewPerson()
        {
            PersonID = clsPersonDataLayer.AddNewPerson(FirstName, SecondName, ThirdName, LastName, 
                NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);

            return (PersonID != 0);
        }
        public bool DeletePerson()
        {
            return clsPersonDataLayer.Delete(this.PersonID);
        }
        private bool _Update()
        {
            return (clsPersonDataLayer.Update(PersonID, FirstName, SecondName, ThirdName, LastName,
                NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID,
                ImagePath));
        }

        public static DataTable GetPeopleList()
        {
            return clsPersonDataLayer.GetPeopleList();
        }
        public static DataTable GetCountriesList()
        {
            return clsPersonDataLayer.GetCountriesList();
        }

        public bool Save()
        {
            
            switch (Mode)
            {
                case enMode.AddNew:
                    Mode = enMode.Update;
                    return _AddNewPerson(); 
                    
                case enMode.Update:
                    return _Update();

            }
            return false;
            
        }
    }

}





