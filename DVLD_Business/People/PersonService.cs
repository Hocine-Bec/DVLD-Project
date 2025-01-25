using System.Data;
using DVLD_Business;

namespace DVLD_Business
{ 
    public class PersonService
    {
        private PersonMapper _personMapper { get; }
        private PersonValidator _personValidator { get; }
        private PersonRepoService _repositoryService { get; }

        public PersonService()
        {
            _personMapper = new PersonMapper();
            _personValidator = new PersonValidator();
            _repositoryService = new PersonRepoService();
        }

        public bool AddNewPerson(Person person)
        {
            if (_personValidator.IsPersonObjectEmpty(person))
            {
                return false;
            }

            var dto = _personMapper.ToDTO(person);

            person.PersonID = _repositoryService.AddNewPerson(dto);
        
            return person.PersonID != -1;
        }

        public bool UpdatePerson(Person person)
        {
            if (_personValidator.IsPersonObjectEmpty(person))
            {
                return false;
            }

            var dto = _personMapper.ToDTO(person);

            return _repositoryService.UpdatePerson(dto);
        }

        public Person Find(int personID)
        {
            var dto = _repositoryService.Find(personID);

            if (_personValidator.IsPersonDTOEmpty(dto))
            {
                return null;
            }

            return _personMapper.FromDTO(dto);
        }

        public Person Find(string nationalNo)
        {
            var dto = _repositoryService.Find(nationalNo);

            if (!_personValidator.IsPersonDTOEmpty(dto))
            {
                return _personMapper.FromDTO(dto);
            }

            return null;
        }

        public bool DeletePerson(int id) => _repositoryService.DeletePerson(id);

        public DataTable GetAllPeople() => _repositoryService.GetAllPeople();

        public bool IsPersonExist(int id) => _repositoryService.IsPersonExist(id);

        public bool IsPersonExist(string nationalNo) => _repositoryService.IsPersonExist(nationalNo);

    }

}



﻿