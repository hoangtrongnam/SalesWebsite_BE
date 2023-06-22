using AutoMapper;
using Models.RequestModel;
using Models.RequestModel.Category;
using Models.RequestModel.Product;
using Models.RequestModel.Supplier;
using Models.RequestModel.WareHouse;
using Models.ResponseModels.Product;

namespace Services.Automapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Ánh xạ từ UpdateInfoUserRequestModel sang UpdateUserCommonRequestModel
            CreateMap<UpdateInfoUserRequestModel, UpdateUserCommonRequestModel>();         
            // Ánh xạ từ ChangePasswordRequestModel sang UpdateUserCommonRequestModel
            CreateMap<ChangePasswordRequestModel, UpdateUserCommonRequestModel>();
            CreateMap<CreateSupplierRequestModel, CreateSupplierRepositoryRequestModel>();
            CreateMap<CreateOnlyProductRequestModel, CreateOnlyProductRepositoryRequestModel>();
            CreateMap<CreateCategoryRequestModel, CreateCategoryRepositoryRequestModel>();
            CreateMap<CreateWareHouseRequestModel, CreateWareHouseRepositoryRequestModel>();

            // Ánh xạ từ ImageRequestModel sang ImageRepositoryRequestModel
            CreateMap<ImageRequestModel, ImageRepositoryRequestModel>()
               .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
               .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
               .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
               .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
               .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
               .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder))
               .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy));

            // Ánh xạ từ PriceRequestModel sang PriceRepositoryRequestModel
            CreateMap<PriceRequestModel, PriceRepositoryRequestModel>()
               .ForMember(dest => dest.ProductID, opt => opt.MapFrom(src => src.ProductID))
               .ForMember(dest => dest.PromotePrice, opt => opt.MapFrom(src => src.PromotePrice))
               .ForMember(dest => dest.PromotPercent, opt => opt.MapFrom(src => src.PromotPercent))
               .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
               .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate))
               .ForMember(dest => dest.CreateBy, opt => opt.MapFrom(src => src.CreateBy))
               .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

        }
    }
}
