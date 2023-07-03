namespace Models.RequestModel.Product;

public class FilterProductByConditionRequestModel
{
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
    public Guid? ProductId { get; set; }
    public string? ProductCode { get; set; }
    public Guid? CategoryId { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }

    public FilterProductByConditionRequestModel(int? pageSize = null, int? pageNumber = null,
        Guid? productID = null, string? productCode = null, Guid? categoryID = null,
        string? name = null, int? status = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        ProductId = productID;
        ProductCode = productCode;
        CategoryId = categoryID;
        Name = name;
        Status = status;
    }
}
