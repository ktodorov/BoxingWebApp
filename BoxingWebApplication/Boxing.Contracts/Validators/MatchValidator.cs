using Boxing.Contracts.Dto;
using FluentValidation;
using System;

namespace Boxing.Contracts.Validators
{
    public class MatchValidator : AbstractValidator<MatchDto>
    {
        public MatchValidator()
        {
            RuleFor(x => x.Boxer1Id).NotNull();
            RuleFor(x => x.Boxer2Id).NotNull();
            RuleFor(x => x.Time).GreaterThan(DateTime.Now);
        }
    }
}
