﻿namespace Models.RequestModel.ProductStock
{
    public class CreateProductStockRequestModel
    {
        public Guid ProductID { get; set; }
        public Guid SupplierID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Status { get; set; }
        public Guid WareHouseID { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
    }
}
