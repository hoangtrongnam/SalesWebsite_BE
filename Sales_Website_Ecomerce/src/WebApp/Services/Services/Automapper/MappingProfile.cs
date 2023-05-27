using AutoMapper;
using Models.RequestModel;
using Models.RequestModel.Category;
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
            CreateMap<GetCategoryByID_ParentTenantRequestModel, GetCategoryCommonRequestModel>();
            CreateMap<ImageResponseModel, ProductResponseModel>()
                .ForMember(dest => dest.ImageName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.Url))
                .ForMember(dest => dest.DescriptionImage, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.SortOrder, opt => opt.MapFrom(src => src.SortOrder));
            CreateMap<PriceResponseModel, ProductResponseModel>()
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.PromotePrice, opt => opt.MapFrom(src => src.PromotePrice))
                .ForMember(dest => dest.PromotPercent, opt => opt.MapFrom(src => src.PromotPercent))
                .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
                .ForMember(dest => dest.EffectiveDate, opt => opt.MapFrom(src => src.EffectiveDate));
        }
    }
}
