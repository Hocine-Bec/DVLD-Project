using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class PersonRepo : IPersonRepo
    {
        private readonly AppDbContext _context;

        public PersonRepo(AppDbContext context)
        {
            _context = context;
        }
        
        #region Public Methods
        public async Task<Person?> GetPersonByIdAsync(int personId)
        {
            if (personId <= 0)
                return null;

            return await ExecuteDbOperationAsync(async () => await _context.People.FindAsync(personId));
        }

        public async Task<Person?> GetPersonByNationalNoAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return null;

            return await ExecuteDbOperationAsync(async () 
                => await _context.People.FirstOrDefaultAsync(x => x.NationalNo == nationalNo));
        }

        public async Task<int> AddNewPersonAsync(Person person)
        {
            if (person == null)
                return -1;

            ExecuteDbOperation(() => _context.People.Add(person));
            await _context.SaveChangesAsync();

            return person.Id;
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            if (person == null)
                return false;


            ExecuteDbOperation(() => _context.People.Update(person));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<Person>?> GetAllPeopleAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.People.ToListAsync());
        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            if (personId <= 0)
                return false;

            var person = _context.People.Find(personId);

            if (person == null)
                return false;

            ExecuteDbOperation(() => _context.People.Remove(person));
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DoesPersonExistAsync(int personId)
        {
            if (personId <= 0)
                return false;

            return await ExecuteDbOperationAsync(async () => await _context.People.AnyAsync(x => x.Id == personId));
        }

        public async Task<bool> DoesPersonExistAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return false;

            return await ExecuteDbOperationAsync(async () 
                => await _context.People.AnyAsync(x => x.NationalNo == nationalNo));

        }
        #endregion


        #region Private Helpers
        private async Task<T?> ExecuteDbOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch
            {
                return default;
            }
        }

        private void ExecuteDbOperation(Action operation)
        {
            try
            {
                operation();
            }
            catch
            {
                //Handle exceptions later
            }
        }
        #endregion

    }

}
