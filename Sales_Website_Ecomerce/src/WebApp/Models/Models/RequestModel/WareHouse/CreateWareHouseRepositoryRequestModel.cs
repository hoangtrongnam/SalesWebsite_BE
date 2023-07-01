namespace Models.RequestModel.WareHouse
{
    public class CreateWareHouseRepositoryRequestModel
    {
        public Guid WareHouseID { get; set; }
        public string WareHouseCode { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
