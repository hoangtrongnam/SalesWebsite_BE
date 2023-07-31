namespace Models.RequestModel.AtributeProduct
{
    public class ProductColorImageRepositoryRequestModel
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid ColorId { get; set; }
        public Guid ImageId { get; set; }
    }
}
