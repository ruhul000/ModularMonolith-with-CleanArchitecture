using FluentValidation;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Repositories;


namespace UserAccess.Application.Validators
{
    internal class UserLoginValidator : AbstractValidator<UserLoginRequest>
    {
        private readonly IUserRepository _userRepository;
        public UserLoginValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
                .Matches("^[a-zA-Z0-9_]*$").WithMessage("Username can only contain letters, numbers, and underscores");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");

        }

    }
}
