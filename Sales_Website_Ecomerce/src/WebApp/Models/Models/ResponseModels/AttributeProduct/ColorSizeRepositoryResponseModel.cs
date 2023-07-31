namespace Models.ResponseModels.AtributeProduct
{
    public class ColorSizeRepositoryResponseModel
    {
        public List<ColorRepositoryModel> colors { get; set; }
        public List<SizeRepositoryModel> sizes { get; set; }
    }

    public class ColorRepositoryModel
    {
        public Guid ColorID { get; set; }
        public string ColorCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid SizeID { get; set; }
        public string Value { get; set; }
        public int TotalStock { get; set; }
    }

    public class SizeRepositoryModel
    {
        public Guid SizeID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public Guid ColorID { get; set; }
        public string ColorCode { get; set; }
        public string Name { get; set; }
        public int TotalStock { get; set; }
    }
}
