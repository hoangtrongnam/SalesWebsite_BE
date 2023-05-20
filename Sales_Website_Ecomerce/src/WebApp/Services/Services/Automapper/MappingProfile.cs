using AutoMapper;
using Models.RequestModel;
using Models.RequestModel.Category;

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
        }
    }
}
