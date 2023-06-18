using Models.ResponseModels.Product;

namespace Models.ResponseModels.Cart
{
    public class CartModel
    {
        public int ProductID{ get; set; } //ProductID để get list Promote
        public string? Name { get; set; }
        public int Quantity { get; set; } //số lượng trong table cartProduct
        public int QuantityMax { get; set; } // số lượng trong table Product
        public decimal Price { get; set; }
        //public decimal TotalPrice { get; set; }
        public int WareHouseID { get; set; }
        public int QuantityMaxInWareHouse { get;}
        public int PromoteID { get; set; }
        public List<PriceResponseModel> lstPromote { get; set; }
    }
}
