using FluentValidation;
using Models.RequestModel.Product;

namespace Validator.Product
{
    public class CreateProductRequestModelValidator: AbstractValidator<CreateOnlyProductRequestModel>
    {
        public CreateProductRequestModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tên sản phẩm không được trống.");
            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Giá không được trống.");
        }
    }
}
