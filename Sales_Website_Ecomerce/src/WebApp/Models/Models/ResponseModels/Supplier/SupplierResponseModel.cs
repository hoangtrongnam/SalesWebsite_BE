namespace Models.ResponseModels.Supplier
{
    public class SupplierResponseModel
    {
        public Guid SupplierId { get; set; }
        public string SupplierCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; } 
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
