﻿namespace Models.RequestModel.Supplier
{
    public class CreateSupplierRepositoryRequestModel
    {
        public Guid SupplierID { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
    }
}
