using Models.RequestModel;
using Models.ResponseModels;
using Repository.Interface;
using Repository.Interfaces.Actions;
using System.Data.SqlClient;
using Dapper;
using static Models.ResponseModels.NotificationResponseModel;
using System.Data;

namespace Repository.Implement
{
    public class NotificationRepository : Repository, INotificationRepository
    {
        public NotificationRepository(SqlConnection context, SqlTransaction _transaction)
        {
            this._context = context;
            this._transaction = _transaction;
        }

        public int Create(NotificationRequestModel item)
        {
            var command = CreateCommand("sp_Notification");
            command.Parameters.AddWithValue("@Note", item.Note);
            command.Parameters.AddWithValue("@Content", item.Content);
            command.Parameters.AddWithValue("@Status", item.Status);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }

        public NotificationResponseModel Get(int id)
        {
            throw new NotImplementedException();
        }

        public NotificationResponseModel Get(int role, int pageIndex)
        {
            //Note: 1: sale, 2: accountant, 3: warehouse staff, 4: customer
            var command = CreateCommand("sp_GetPagedNotifications");
            command.Parameters.AddWithValue("@Role", role);
            command.Parameters.AddWithValue("@PageIndex", pageIndex);
            command.Parameters.AddWithValue("@PageSize", 10);

            //Thêm tham số đầu ra cho stored procedure
            SqlParameter outputParam = new SqlParameter("@NewNotificationNumber", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.CommandType = System.Data.CommandType.StoredProcedure;            

            NotificationResponseModel result = new NotificationResponseModel();
            command.ExecuteNonQuery();
            result.NewNotificatonNumber = (int)outputParam.Value;

            using (var reader = command.ExecuteReader())
            {
                var lstNotification = new List<Notification>();
                while (reader.Read())
                {
                    var notification = new Notification
                    {
                        NotificationID = string.IsNullOrEmpty(reader["NotificationID"].ToString()) ? 0 : Convert.ToInt32(reader["NotificationID"]),
                        Content = reader["Content"].ToString() ?? "",
                    };
                    lstNotification.Add(notification);
                }
                result.lstNotification = lstNotification;
            };
            return result;
        }

        public List<NotificationResponseModel> GetAll(int pageIndex)
        {
            throw new NotImplementedException();
        }

        public List<NotificationResponseModel> GetAll()
        {
            throw new NotImplementedException();
        }

        public int Update(NotificationRequestModel item, int notificationID)
        {
            var command = CreateCommand("sp_UpdateNotification");
            command.Parameters.AddWithValue("@Content", item.Content);
            command.Parameters.AddWithValue("@Note", item.Note);
            command.Parameters.AddWithValue("@Status", item.Status);
            command.Parameters.AddWithValue("@NotificationID", notificationID);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            return command.ExecuteNonQuery();
        }
    }
}
