﻿using Models.ResponseModels.Product;

namespace Models.ResponseModels
{
    public class OrderResponseModel
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
        public decimal TotalPayment { get; set; } //tổng số tiền cần thanh toán
        public string DepositAmount { get; set; } //số tiền cọc
        public string? Note { get; set; }
        public string Status { get; set; }
        public string? CreateDate { get; set; }
        public List<Product> lstProduct { get; set; }

        public class Product
        {
            public Guid ProductId { get; set; } //ProductID để get list Promote
            public string Name { get; set; }
            //public string? Code { get; set; }
            public int Quantity { get; set; } //quantity trong table OrderProduct (SL đặt hàng)
            //public int QuantityMax { get; set; } // số lượng trong table Product
            public decimal Price { get; set; }
            public decimal ExfactoryPrice { get; set; }
            public Guid WarehouseId { get; set; }
            //public int QuantityMaxInWareHouse { get; } //số lượng tối đa trong kho của productID
            public Guid PromoteId { get; set; }
            public List<PriceResponseModel> lstPromote { get; set; }
        }
    }
}