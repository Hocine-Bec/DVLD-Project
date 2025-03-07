using Microsoft.Extensions.Logging;

namespace DVLD_DataAccess.Core.Repositories
{
    public abstract class BaseRepoHelper
    {
        private readonly ILogger _logger;

        protected BaseRepoHelper(ILogger logger)
        {
            _logger = logger;            
        }

        protected async Task<T?> ExecuteDbOperationAsync<T>(Func<Task<T>> operation)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database operation failed");
                throw;
            }
        }

        protected void ExecuteDbOperation(Action operation)
        {
            try
            {
                operation();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database operation failed");
                throw;
            }
        }
    }
}
