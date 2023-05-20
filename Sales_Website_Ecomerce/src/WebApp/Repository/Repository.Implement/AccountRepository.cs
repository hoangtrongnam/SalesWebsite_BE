using Common;
using Dapper;
using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Net.Mail;

namespace Repository.Implement
{
    public class AccountRepository : Repository, IAccountRepository
    {
        public AccountRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }
        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int Create(UserRegisterRequestModel item)
        {
            var parameters = new DynamicParameters(new
            {
                UserName = item.UserName,
                PasswordHasd = item.Password,
                FullName = item.FullName,
                EmailAddress = item.EmailAddress,
                PhoneNumber = item.PhoneNumber,
                Address = item.Address
            });

            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);

            Execute("sp_User_Register", parameters, commandType: CommandType.StoredProcedure);

            return parameters.Get<int>("@Result"); 
        }
        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public UserResponseModel Get(string userName)
        {
            return QueryFirstOrDefault<UserResponseModel>("Sp_Get_User_By_UserName", 
                    new { UserName = userName }, commandType: CommandType.StoredProcedure);
        }

        public UserResponseModel Get(string id, string pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<UserResponseModel> GetAll(string pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<UserResponseModel> GetAll()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Update User Common
        /// </summary>
        /// <param name="item"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Update(UpdateUserCommonRequestModel item, int id)
        {
            var parameters = new DynamicParameters(new
            {
                ID = id,
                UserName = item.UserName,
                PasswordHasd = item.Password,
                FullName = item.FullName,
                EmailAddress = item.EmailAddress,
                PhoneNumber = item.PhoneNumber,
                Address = item.Address
            });
            parameters.Add("@Result", dbType: DbType.Int32, direction: ParameterDirection.Output);
            Execute("Sp_Update_User", parameters, commandType: CommandType.StoredProcedure);
            return parameters.Get<int>("@Result");
        }
    }
}
