namespace Models.ResponseModels.AtributeProduct
{
    public class ColorSizeResponseModel
    {
        public List<ColorModel> listColor { get; set; }
        public List<SizeModel> listSize { get; set; }
    }

    public class ColorModel
    {
        public Guid ColorID { get; set; }
        public string ColorCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TotalStock { get; set; }
        public List<OnlySize> Sizes { get; set; }
    }
    public class SizeModel
    {
        public Guid SizeID { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public int TotalStock { get; set; }
        public List<OnlyColor> Colors { get; set; }
    }


    public class OnlyColor
    {
        public Guid ColorID { get; set; }
        public string ColorCode { get; set; }
        public string Name { get; set; }
    }
    public class OnlySize
    {
        public Guid SizeID { get; set; }
        public string Value { get; set; }
    }
}
