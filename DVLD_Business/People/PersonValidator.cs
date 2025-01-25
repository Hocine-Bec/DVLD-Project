using DVLD.DTOs;

namespace DVLD_Business
{
    public class PersonValidator
    {
        public bool IsPersonDTOEmpty(PersonDTO personDTO) => (personDTO == null) ? true : false;

        public bool IsPersonObjectEmpty(Person person) => (person == null) ? true : false;

    }


}



﻿