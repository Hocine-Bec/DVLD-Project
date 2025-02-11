using DVLD_DataAccess.Core.Context;
using DVLD_DataAccess.Core.Entities;
using DVLD_DataAccess.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public class TestTypesRepo : BaseRepoHelper, ITestTypeRepo
    {
        private readonly AppDbContext _context;

        public TestTypesRepo(AppDbContext context, ILogger logger) : base(logger)
        {
            _context = context;
        }

        public async Task<TestType?> GetByIdAsync(int testTypeId)
        {
            if (testTypeId <= 0)
                throw new ArgumentException($"Test Type ID {testTypeId} must be greater than 0.");

            return await ExecuteDbOperationAsync(async () => await _context.TestTypes.FindAsync(testTypeId));
        }

        public async Task<TestType?> GetByTitleAsync(string? testTypeTitle)
        {
            if (string.IsNullOrWhiteSpace(testTypeTitle))
                throw new ArgumentException("Test Type Title cannot be null or empty.");

            return await ExecuteDbOperationAsync(async () =>
                await _context.TestTypes
                    .FirstOrDefaultAsync(t => t.Title == testTypeTitle));
        }

        public async Task<List<TestType>?> GetAllTestTypesAsync()
        {
            return await ExecuteDbOperationAsync(async () =>
                await _context.TestTypes
                    .OrderBy(t => t.Id)
                    .ToListAsync());
        }

        public async Task<int> AddTestTypeAsync(TestType? testType)
        {
            if (testType == null)
                throw new ArgumentNullException(nameof(testType));

            return await ExecuteDbOperationAsync(async () =>
            {
                await _context.TestTypes.AddAsync(testType);
                await _context.SaveChangesAsync();
                return testType.Id;
            });
        }

        public async Task<bool> UpdateTestTypeAsync(TestType? testType)
        {
            if (testType == null)
                throw new ArgumentNullException(nameof(testType));

            return await ExecuteDbOperationAsync(async () =>
            {
                _context.TestTypes.Update(testType);
                return await _context.SaveChangesAsync() > 0;
            });
        }
    }
}