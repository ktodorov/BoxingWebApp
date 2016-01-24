using Boxing.Contracts.Dto;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boxing.Contracts.Validators
{
    public class UserValidator  : AbstractValidator<UserDto>
    {
        public UserValidator()
        {
            RuleFor(x => x.Password).NotEmpty();
            RuleFor(x => x.Username).NotEmpty();
        }
    }
}
