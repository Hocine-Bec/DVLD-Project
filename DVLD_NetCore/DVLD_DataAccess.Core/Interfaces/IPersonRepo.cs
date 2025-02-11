using DVLD_DataAccess.Core.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DVLD_DataAccess.Core.Interfaces
{
    public interface IPersonRepo
    {
        Task<Person?> GetPersonByIdAsync(int personId);
        Task<Person?> GetPersonByNationalNoAsync(string nationalNo);
        Task<int> AddNewPersonAsync(Person person);
        Task<bool> UpdatePersonAsync(Person person);
        Task<List<Person>?> GetAllPeopleAsync(); 
        Task<bool> DeletePersonAsync(int personId);
        Task<bool> DoesPersonExistAsync(int personId);
        Task<bool> DoesPersonExistAsync(string nationalNo);
    }

}
