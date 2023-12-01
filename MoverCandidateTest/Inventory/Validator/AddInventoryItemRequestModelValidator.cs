using FluentValidation;
using MoverCandidateTest.Inventory.Model;

namespace MoverCandidateTest.Inventory.Validator;

public class AddInventoryItemRequestModelValidator : AbstractValidator<AddInventoryItemRequestModel>
{
    public AddInventoryItemRequestModelValidator()
    {
        RuleFor(x => x.Sku).NotNull()
            .WithMessage("SKU cannot be null.");
        RuleFor(x => x.Sku).NotEmpty()
            .WithMessage("SKU cannot be empty.");
        RuleFor(x => x.Sku).Matches("^[a-zA-Z0-9-_\\.]{3,20}$")
            .WithMessage("Please enter a valid SKU. It should consist of 3 to 20 characters, including letters (both uppercase and lowercase), numbers, hyphens, underscores, and periods");

        RuleFor(x => x.Description).NotNull()
            .WithMessage("Description cannot be null.");
        RuleFor(x => x.Description).NotEmpty()
            .WithMessage("Description cannot be null.");

        RuleFor(x => x.Quantity).NotNull()
            .WithMessage("Quantity cannot be null.");
        RuleFor(x => x.Quantity).NotEmpty()
            .WithMessage("Quantity cannot be null.");
        RuleFor(x => x.Quantity).GreaterThan((uint)0)
            .WithMessage("Quantity must be greater than 0.");
    }
}
