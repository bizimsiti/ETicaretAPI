using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;


namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VMCreateProduct>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen ürün adını boş geçmeyiniz.")
                .MaximumLength(150)
                .MinimumLength(1)
                    .WithMessage("Lütfen ürün adını 1 karakterden fazla olduğuna dikkat ediniz.");

            RuleFor(p => p.Stock)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen stok bilgisi giriniz.")
                .Must(s => s >= 0)
                    .WithMessage("Stok bilgisi giriniz.");

            RuleFor(p => p.Price)
                .NotEmpty()
                .NotNull()
                    .WithMessage("Lütfen fiyat bilgisi giriniz.")
                .Must(p => p >= 0)
                    .WithMessage("Fiyat bilgisi giriniz.");
        }
    }
}
