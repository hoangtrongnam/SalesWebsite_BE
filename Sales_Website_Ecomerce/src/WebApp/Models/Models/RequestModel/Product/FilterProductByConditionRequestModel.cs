namespace Models.RequestModel.Product;

public class FilterProductByConditionRequestModel
{
    public int? PageSize { get; set; }
    public int? PageNumber { get; set; }
    public Guid? ProductID { get; set; }
    public string? ProductCode { get; set; }
    public Guid? CategoryID { get; set; }
    public string? Name { get; set; }
    public int? Status { get; set; }

    public FilterProductByConditionRequestModel(int? pageSize = null, int? pageNumber = null,
        Guid? productID = null, string? productCode = null, Guid? categoryID = null,
        string? name = null, int? status = null)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        ProductID = productID;
        ProductCode = productCode;
        CategoryID = categoryID;
        Name = name;
        Status = status;
    }
}
