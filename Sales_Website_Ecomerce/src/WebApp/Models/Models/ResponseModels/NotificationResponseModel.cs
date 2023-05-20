namespace Models.ResponseModels
{
    public class NotificationResponseModel
    {
        public int? NewNotificatonNumber { get; set; }
        public List<Notification> lstNotification { get; set; }

        public class Notification
        {
            public int NotificationID { get; set; }
            public string? Content { get; set; }
            public Boolean? NewNotification { get; set; }

        }
    }
}