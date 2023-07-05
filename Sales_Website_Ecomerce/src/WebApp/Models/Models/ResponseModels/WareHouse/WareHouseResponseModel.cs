namespace Models.ResponseModels.WareHouse
{
    public class WareHouseResponseModel
    {
        public Guid WareHouseId { get; set; }
        public string WareHouseCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
