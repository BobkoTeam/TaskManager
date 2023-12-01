using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Core.DTO_s.User;

namespace TaskManager.Core.Validation.User
{
    public class RegisterValidation:AbstractValidator<RegisterDto>
    {
        public RegisterValidation()
        {
            RuleFor(r => r.Name).NotEmpty().MaximumLength(64).MinimumLength(2);
            RuleFor(r => r.Email).NotEmpty().EmailAddress().MaximumLength(128);
            RuleFor(r => r.Password).NotEmpty().MinimumLength(6);
            RuleFor(r => r.ConfirmPassword).NotEmpty().MinimumLength(6).Equal(p => p.Password).WithMessage("Password and Confirm password must be equal");
        }
    }
}
