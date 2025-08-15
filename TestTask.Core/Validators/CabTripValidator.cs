using FluentValidation;
using TestTask.Core.Models;

namespace TestTask.Core.Validators;

public class CabTripValidator : AbstractValidator<CabTripModel>
{
    public CabTripValidator()
    {
        RuleFor(x => x.PickUpDateTime)
            .NotEmpty().WithMessage("PickUpDateTime is required.");

        RuleFor(x => x.DropoffDateTime)
            .NotEmpty().WithMessage("DropoffDateTime is required.")
            .GreaterThan(x => x.PickUpDateTime)
            .WithMessage("DropoffDateTime must be after PickUpDateTime.");

        RuleFor(x => x.PassengerCount)
            .NotNull().WithMessage("PassengerCount is required.")
            .GreaterThanOrEqualTo(0).WithMessage("PassengerCount must be non-negative.");

        RuleFor(x => x.StoreAndFwdFlag)
            .NotEmpty().WithMessage("StoreAndFwdFlag is required.");

        RuleFor(x => x.TripDistance)
             .NotNull().WithMessage("TripDistance is required.")
            .GreaterThanOrEqualTo(0).WithMessage("TripDistance must be non-negative.");

        RuleFor(x => x.PULocationID)
             .NotNull().WithMessage("PULocationIDe is required.");

        RuleFor(x => x.DOLocationID)
             .NotNull().WithMessage("DOLocationIDe is required.");

        RuleFor(x => x.FareAmount)
            .NotNull().WithMessage("FareAmount is required.")
            .GreaterThanOrEqualTo(0).WithMessage("FareAmount must be non-negative.");

        RuleFor(x => x.TipAmount)
            .NotNull().WithMessage("TipAmount is required.")
            .GreaterThanOrEqualTo(0).WithMessage("TipAmount must be non-negative.");

    }
}
