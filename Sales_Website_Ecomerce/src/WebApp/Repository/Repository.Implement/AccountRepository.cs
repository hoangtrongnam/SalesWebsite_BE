using Common;
using Models.RequestModel;
using Repository.Interface;
using System.Data.SqlClient;

namespace Repository.Implement
{
    public class AccountRepository : Repository,IAccountRepository
    {
        public AccountRepository(SqlConnection context, SqlTransaction transaction)
        {
            this._context = context;
            this._transaction = transaction;
        }
        
        //demo compare password, email
        public bool SignInAsync(SignInRequestModel model)
        {
            if (model.EmaiAddress.Equals(Parameters.EmailAddress) && model.Password.Equals(Parameters.Password))
            {
                return true;
            }
            return false;
        }
    }
}
