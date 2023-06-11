namespace Models.ResponseModels
{
    public class OrderResponseModel
    {
        public string OrderID { get; set; }
        public string CustomerID { get; set; }
        public decimal TotalPayment { get; set; } //tổng số tiền cần thanh toán
        public string DepositAmount { get; set; } //số tiền cọc
        public string? Note { get; set; }
        public string Status { get; set; }
        public string? CreateDate { get; set; }
        public List<Product> lstProduct { get; set; }

        public class Product
        {
            public int ProductID { get; set; }
            //public string Name { get; set; }
            //public string? Code { get; set; }
            public int Quantity { get; set; }
            public decimal Price { get; set; }
        }
    }
}