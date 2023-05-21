using Models.RequestModel;
using Repository.Interface;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class JobScheduleRepository : Repository, IJobScheduleRepository
    {
        public JobScheduleRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }
        
        public int Create(JobRequestModel item)
        {
            var command = CreateCommand("sp_InsertJobScheduleLog");
            command.Parameters.AddWithValue("@JobName", item.JobName);
            command.Parameters.AddWithValue("@Status", item.Status);
            command.Parameters.AddWithValue("@Content", item.Content);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }
    }
}
