using Common;
using Microsoft.Extensions.Configuration;
using UnitOfWork.Interface;

namespace UnitOfWork.Database
{
    public class UnitOfWorkDatabase : IUnitOfWork
    {
        private readonly IConfiguration _configuration;

        public UnitOfWorkDatabase(IConfiguration configuration = null)
        {
            _configuration = configuration;
        }
        public IUnitOfWorkAdapter Create()
        {
            var connectionString = _configuration == null
                ? Parameters.ConnectionString
                : _configuration.GetValue<string>("SqlConnectionString");

            return new UnitOfWorkDatabaseAdapter(connectionString);
        }
    }
}
