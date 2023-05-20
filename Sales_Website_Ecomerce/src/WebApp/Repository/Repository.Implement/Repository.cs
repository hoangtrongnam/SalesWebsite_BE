using Dapper;
using System.Data;
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

        //Thực thi một truy vấn không trả về kết quả và trả về số lượng bản ghi bị ảnh hưởng bởi truy vấn(thường được sử dụng cho các truy vấn UPDATE, INSERT và DELETE)
        protected int Execute(string query, object parameters = null, CommandType commandType = CommandType.Text)
        {
            return _context.Execute(query, parameters, _transaction, commandType: commandType);
        }

        //Phương thức này thực thi một truy vấn và trả về một danh sách đối tượng được tạo từ các bản ghi trả về
        protected IEnumerable<T> Query<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return _context.Query<T>(sql, param, _transaction, commandType: commandType);
        }

        //Phương thức này thực thi một truy vấn và trả về một đối tượng được tạo từ bản ghi đầu tiên của kết quả truy vấn
        protected T QueryFirstOrDefault<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return _context.QueryFirstOrDefault<T>(sql, param, _transaction, commandType: commandType);
        }

        //Phương thức này thực thi một truy vấn và trả về giá trị đơn, chẳng hạn như số bản ghi được ảnh hưởng bởi một truy vấn INSERT hoặc UPDATE
        protected T ExecuteScalar<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return _context.ExecuteScalar<T>(sql, param, _transaction, commandType: commandType);
        }

        //thực thi một truy vấn và trả về nhiều kết quả, mỗi kết quả được trả về dưới dạng một danh sách các đối tượng được tạo từ các bản ghi trả về
        protected SqlMapper.GridReader QueryMultiple(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return _context.QueryMultiple(sql, param, _transaction, commandType: commandType);
        }

        //Asynchronus
        //Thực thi một truy vấn bất đồng bộ và trả về giá trị đơn
        protected async Task<T> ExecuteScalarAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _context.ExecuteScalarAsync<T>(sql, param, _transaction, commandType: commandType);
        }

        //thực thi một truy vấn bất đồng bộ và trả về một đối tượng được tạo từ bản ghi đầu tiên của kết quả truy vấn.
        protected async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _context.QueryFirstOrDefaultAsync<T>(sql, param, _transaction, commandType: commandType);
        }

        //thực thi một truy vấn bất đồng bộ và trả về một danh sách đối tượng được tạo từ các bản ghi trả về
        protected async Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, CommandType commandType = CommandType.Text)
        {
            return await _context.QueryAsync<T>(sql, param, _transaction, commandType: commandType);
        }
    }
}
