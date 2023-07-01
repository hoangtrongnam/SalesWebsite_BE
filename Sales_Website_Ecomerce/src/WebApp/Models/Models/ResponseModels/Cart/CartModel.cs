using Models.ResponseModels.Product;

namespace Models.ResponseModels.Cart
{
    public class CartModel
    {
        public Guid ProductID{ get; set; } //ProductID để get list Promote
        public string? Name { get; set; }
        public int Quantity { get; set; } //số lượng trong table cartProduct
        public int QuantityMax { get; set; } // số lượng trong table Product
        public decimal Price { get; set; } //trong table Product
        public decimal TotalPrice { get; set; }
        public Guid WareHouseID { get; set; }
        public int QuantityMaxInWareHouse { get;} //số lượng tối đa trong kho của productID
        public Guid PromoteID { get; set; }
        public List<PriceResponseModel> lstPromote { get; set; }
    }
}
