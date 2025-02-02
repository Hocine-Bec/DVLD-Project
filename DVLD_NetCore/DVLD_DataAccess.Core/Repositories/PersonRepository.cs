using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DVLD_DataAccess.Core.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly AppDbContext _context;

        public PersonRepository(AppDbContext context)
        {
            _context = context;
        }

        public Person? GetPersonById(int personId)
        {
            return ExecuteDbOperation(() => _context.People.Find(personId));
        }

        public Person? GetPersonByNationalNo(string nationalNo)
        {
            if(string.IsNullOrWhiteSpace(nationalNo)) 
                return null;

            return ExecuteDbOperation(() => _context.People.FirstOrDefault(x => x.NationalNo == nationalNo));
        }

        public int AddNewPerson(Person person)
        {
            if (person == null)
                return -1;

            ExecuteDbOperation(() => _context.People.Add(person));
            _context.SaveChanges();

            return person.PersonId;
        }

        public bool UpdatePerson(Person person)
        {
            if (person == null)
                return false;


            ExecuteDbOperation(() => _context.People.Update(person));
            return _context.SaveChanges() > 0;
        }

        public List<Person>? GetAllPeople()
        {
            return ExecuteDbOperation(() => _context.People.ToList());
        }

        public bool DeletePerson(int personId)
        {
            var person = _context.People.Find(personId);

            if (person == null)
                return false;

            ExecuteDbOperation(() => _context.People.Remove(person));
            return _context.SaveChanges() > 0;
        }

        public bool DoesPersonExist(int personId)
        {
            if (personId == -1)
                return false;

            return ExecuteDbOperation(() => _context.People.Any(x => x.PersonId == personId));
        }

        public bool DoesPersonExist(string nationalNo)
        {
            if (string.IsNullOrWhiteSpace(nationalNo))
                return false;

            return ExecuteDbOperation(() => _context.People.Any(x => x.NationalNo == nationalNo));

        }

        
        private T? ExecuteDbOperation<T>(Func<T> operation)
        {
            try
            {
                return operation();
            }
            catch
            {
                return default;
            }
        }


    }

}
