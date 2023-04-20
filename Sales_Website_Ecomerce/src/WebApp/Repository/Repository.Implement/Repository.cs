using System.Data.SqlClient;

namespace Repository.Implement
{
    public abstract class Repository
    {
        protected SqlConnection _context;
        protected SqlTransaction _transaction;

        //SqlCommand: SqlCommand (tên đầy đủ là System.Data.SqlClient.SqlCommand) là class chịu trách nhiệm thực thi truy vấn trên một kết nối tới cơ sở dữ liệu Sql Server.
        protected SqlCommand CreateCommand(string query)
        {
            return new SqlCommand(query, _context, _transaction);
        }
    }
}
