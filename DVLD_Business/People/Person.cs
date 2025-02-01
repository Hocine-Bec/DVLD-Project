using System;

namespace DVLD_Business
{
    public class Person
    {
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
        public int CountryId { get; set; }
        public Countries Country { get; set; } 
        public string ImagePath { get; set; }


        public Person() { }

        public Person(Person person)
        {
            //Refactor later to use AutoMapper
            this.PersonID = person.PersonID;
            this.FirstName = person.FirstName;
            this.LastName = person.LastName;
            this.ThirdName = person.ThirdName;
            this.LastName = person.LastName;
            this.NationalNo = person.NationalNo;
            this.DateOfBirth = person.DateOfBirth;
            this.Email = person.Email;
            this.CountryId = person.CountryId;
            this.Address = person.Address;
            this.Phone = person.Phone;
            this.ImagePath = person.ImagePath;
            this.Gender = person.Gender;
        }
    }


}



﻿