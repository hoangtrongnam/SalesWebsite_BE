namespace Common
{
    public class Parameters
    {
        public const string ConnectionString = "Data Source=156.67.216.141;Initial Catalog=Sales_Website_DB_Dev_V1;User ID=NAMCODEIT;Password=Ban@180296";
        //public const string ConnectionString = "Data Source=156.67.216.141;Initial Catalog=DB_Sang;User ID=NAMCODEIT;Password=Ban@180296";


        //login information demo
        public const string EmailAddress = "admin@gmail.com";
        public const string Password = "Admin@123456";

        // Thiết lập ITrigger để kích hoạt công việc vào 7 giờ sáng theo múi giờ Việt Nam
        public static DateTimeOffset StartTime = new DateTimeOffset(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0, TimeSpan.FromHours(7));
        public const int TimeMinutes = 12;
        public const int TimeHours = 12;

        #region Satus Common
        public const int StatusSuccess = 1;
        public const int StatusFailed = 0;
        public const int StatusCanceled = 2;
        #endregion

        #region Status Cart
        public const int StatusCartInsert = 70; //Thêm cart mới
        public const int StatusCartInsertSuccess = 71; //Thêm cart thành công
        public const int StatusCartProductInsert = 72; //Thêm cart_product
        public const int StatusCartProductInsertSuccess = 73; //Thêm cart_product thành công

        public const int StatusCartUpdate = 74; //Sửa cart
        public const int StatusCartUpdateSuccess = 75; //Sửa cart thành công
        public const int StatusCartProductUpdate = 76; //Sửa cart_product thành công
        public const int StatusCartProductUpdateSuccess = 77; //Sửa cart_product thành công
        public const int StatusQuantityCartProductUpdate = 78; //Sửa Quantity trong table cart_product

        public const int StatusDeleteCart = 79; // Xóa cart
        public const int StatusDeleteCartProduct = 80; // Xóa Product trong cart

        #endregion Cart

        #region Order
        public const int StatusOrderInsert = 10; //Thêm Order(Có dơn hàng mới cần sale xác nhận)
        public const int StatusOrderSaleConfirm = 11; //Sale xác nhận tiền cọc và đợn hàng thành công
        public const int StatusOrderNoContact = 12; //Sale không liên hệ được với KH
        public const int StatusOrderDepositAmount = 13; //số tiền cọc không đúng (kế toán)
        public const int StatusOrderKTConfirm = 14; //Kế toán xác nhận đủ tiền cọc
        public const int StatusOrderDelete = 15; //Sale hủy Đơn hàng 
        #endregion

        #region Notifycation
        public const int StatusNVNotify = 20; //thông báo mới cho NV Sale
        public const int StatusKTNotify = 22; //thông báo mới cho kế toán
        public const int StatusKHNotify = 24; //thông báo mới cho KH
        public const int StatusKhoNotify = 26; //thông báo cho nhân viên kho
        #endregion

        public enum JobName
        {
            RejectOrder
        }

        public static Dictionary<string, InfoTable> tables = new Dictionary<string, InfoTable>()
        {
            { "Image", new InfoTable() { TableName = "Image", ColumnName = "ImageCode" } },
            { "Price", new InfoTable() { TableName = "Price", ColumnName = "PriceCode" } },
            { "Supplier", new InfoTable() { TableName = "Supplier", ColumnName = "SupplierCode" } },
            { "Product", new InfoTable() { TableName = "Product", ColumnName = "ProductCode" } },
            { "CategoryProduct", new InfoTable() { TableName = "CategoryProduct", ColumnName = "CategoryCode" } },
            { "WareHouse", new InfoTable() { TableName = "WareHouse", ColumnName = "WareHouseCode" } },
            { "ProductStock", new InfoTable() { TableName = "ProductStock", ColumnName = "ProductStockCode" } }
        };
    }

    public class InfoTable
    {
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}
