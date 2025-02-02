using DVLD_DataAccess.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IPersonRepository
    {
        Person? GetPersonById(int personId);
        Person? GetPersonByNationalNo(string nationalNo);
        int AddNewPerson(Person person);
        bool UpdatePerson(Person person);
        List<Person>? GetAllPeople(); 
        bool DeletePerson(int personId);
        bool DoesPersonExist(int personId);
        bool DoesPersonExist(string nationalNo);
    }

}
