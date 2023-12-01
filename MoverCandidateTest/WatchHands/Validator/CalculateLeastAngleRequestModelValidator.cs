using FluentValidation;
using MoverCandidateTest.WatchHands.Model;

namespace MoverCandidateTest.WatchHands.Validator;

public class CalculateLeastAngleRequestModelValidator : AbstractValidator<CalculateLeastAngleRequestModel>
{
    public CalculateLeastAngleRequestModelValidator()
    {
        RuleFor(x => x.DateTime).NotNull()
            .WithMessage("DateTime cannot be null.");

        RuleFor(x => x.DateTime).NotEmpty()
            .WithMessage("DateTime cannot be empty.");

        RuleFor(x => x.DateTime.Hour).InclusiveBetween(0, 23)
            .WithMessage("Hour must be between 0 and 23.");

        RuleFor(x => x.DateTime.Minute).InclusiveBetween(0, 59)
            .WithMessage("Minute must be between 0 and 59.");
        
        RuleFor(x => x.DateTime.Second).InclusiveBetween(0, 59)
            .WithMessage("Second must be between 0 and 59.");
    }
}