﻿namespace Models.ResponseModels.Category
{
    public class StatusResponseModel
    {
        public Guid StatusID { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
    }
}