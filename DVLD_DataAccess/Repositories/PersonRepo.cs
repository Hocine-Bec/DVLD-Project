using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities.Identity;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class PersonRepo : BaseRepoHelper, IPersonRepo
    {
        private readonly AppDbContext _context;

        public PersonRepo(AppDbContext context, ILogger logger) : base(logger) 
        {
            _context = context;
        }
        
        #region Public Methods
        public async Task<Person?> GetPersonByIdAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.People.FindAsync(personId));
        }

        public async Task<Person?> GetPersonByNationalNoAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                throw new ArgumentException("National No cannot be empty.");

            return await ExecuteDbOperationAsync(async () 
                => await _context.People.FirstOrDefaultAsync(x => x.NationalNo == nationalNo));
        }

        public async Task<int> AddNewPersonAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException("person cannot be Null.");

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.People.AddAsync(person);
                await _context.SaveChangesAsync();

                return person.Id;
            });
        }

        public async Task<bool> UpdatePersonAsync(Person person)
        {
            if (person == null)
                throw new ArgumentNullException("person cannot be Null.");

            ExecuteDbOperation(() => _context.People.Update(person));
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }

        public async Task<List<Person>?> GetAllPeopleAsync()
        {
            return await ExecuteDbOperationAsync(async () => await _context.People.ToListAsync());
        }

        public async Task<bool> DeletePersonAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            var person = await ExecuteDbOperationAsync(async () => await _context.People.FindAsync(personId));

            if (person == null)
                throw new ArgumentNullException("person cannot be Null.");

            ExecuteDbOperation(() => _context.People.Remove(person));
            return await ExecuteDbOperationAsync(async () => await _context.SaveChangesAsync() > 0);
        }

        public async Task<bool> DoesPersonExistAsync(int personId)
        {
            if (personId <= 0)
                throw new ArgumentException($"Person ID {personId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.People.AnyAsync(x => x.Id == personId));
        }

        public async Task<bool> DoesPersonExistAsync(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                throw new ArgumentException("National No cannot be empty.");

            return await ExecuteDbOperationAsync(async () 
                => await _context.People.AnyAsync(x => x.NationalNo == nationalNo));

        }
        #endregion

    }
}
