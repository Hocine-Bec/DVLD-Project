using DVLD.DTOs;

namespace DVLD_Business
{
    public class PersonMapper
    {
        public PersonDTO ToDTO(Person person)
        {
            return new PersonDTO
            {
                PersonID = person.PersonID,
                FirstName = person.FirstName,
                SecondName = person.SecondName,
                ThirdName = person.ThirdName,
                LastName = person.LastName,
                NationalNo = person.NationalNo,
                DateOfBirth = person.DateOfBirth,
                Gender = person.Gender,
                Address = person.Address,
                Phone = person.Phone,
                Email = person.Email,
                CountryId = person.CountryId,
                ImagePath = person.ImagePath
            };
        }

        public Person FromDTO(PersonDTO dto)
        {
            return new Person
            {
                PersonID = dto.PersonID,
                FirstName = dto.FirstName,
                SecondName = dto.SecondName,
                ThirdName = dto.ThirdName,
                LastName = dto.LastName,
                NationalNo = dto.NationalNo,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                Address = dto.Address,
                Phone = dto.Phone,
                Email = dto.Email,
                CountryId = dto.CountryId,
                Country = Countries.Find(dto.CountryId),
                ImagePath = dto.ImagePath
            };
        }
    }


}



﻿