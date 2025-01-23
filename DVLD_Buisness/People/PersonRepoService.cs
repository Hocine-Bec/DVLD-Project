using System.Data;
using DVLD.DTOs;
using DVLD_DataAccess;

namespace DVLD_Business
{
    public class PersonRepoService
    {
        private PersonRepository _repository;

        public PersonRepoService() 
        {
            _repository = new PersonRepository();
        }

        public int AddNewPerson(PersonDTO dto) => _repository.AddNewPerson(dto);

        public bool UpdatePerson(PersonDTO dto) => _repository.UpdatePerson(dto);
        
        public PersonDTO Find(int personID) => _repository.GetPersonById(personID);

        public PersonDTO Find(string nationalNo) => _repository.GetPersonByNationalNo(nationalNo);

        public bool DeletePerson(int id) => _repository.DeletePerson(id);

        public DataTable GetAllPeople() => _repository.GetAllPeople();

        public bool IsPersonExist(int id) => _repository.IsPersonExist(id);

        public bool IsPersonExist(string nationalNo) => _repository.IsPersonExist(nationalNo);
    }


}



﻿