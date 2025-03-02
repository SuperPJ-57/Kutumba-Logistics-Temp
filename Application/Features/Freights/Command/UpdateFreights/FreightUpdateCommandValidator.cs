using Application.Features.Freights.Command.UpdateFreights;
using FluentValidation;

namespace Application.Features.Freights.Command.AddFreights;

public class FreightUpdateCommandValidator : AbstractValidator<FreightUpdateCommand>
{
    public FreightUpdateCommandValidator()
    {
        RuleFor(x => x.FreightRate).NotEmpty().WithMessage("Freight Rate is Required");
        //RuleFor(x => x.FreightRate)
        //    .GreaterThanOrEqualTo(0).WithMessage("Freight Rate Cannot be Negative")
        //    .When(x => x.FreightRate.HasValue);


        RuleFor(x => x.AdvancedPaid).NotEmpty().WithMessage("AdvancedPaid is Required");
        //RuleFor(x => x.AdvancedPaid)
        //    .GreaterThanOrEqualTo(0).WithMessage("AdvancedPaidCannot be Negative")
        //    .When(x => x.AdvancedPaid.HasValue);

        RuleFor(x => x.RemainingAmount).NotEmpty().WithMessage("Remaining Amount is Required");
        //RuleFor(x => x.RemainingAmount)
        //    .GreaterThanOrEqualTo(0).WithMessage("Remaining Amount Cannot be Negative")
        //    .When(x => x.RemainingAmount.HasValue);
        RuleFor(x => x.PaymentMode).NotEmpty().WithMessage("Payment Mode is Required.");
    }
}