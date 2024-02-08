using FluentValidation;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Repositories;

namespace UserAccess.Application.Validators
{
    public class UserRegistrationValidator : AbstractValidator<UserRequest>
    {
        private readonly IUserRepository _userRepository;
        public UserRegistrationValidator(IUserRepository userRepository) 
        {
            _userRepository = userRepository;

            RuleFor(x => x.FullName).NotEmpty().WithMessage("FullName is required.");
            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("Username is required")
                .MinimumLength(3).WithMessage("Username must be at least 3 characters long")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
                .Matches("^[a-zA-Z0-9_]*$").WithMessage("Username can only contain letters, numbers, and underscores")
                .Must(UserNameUnique).WithMessage("Username already exist! Please try different Username.");
            RuleFor(x => x.Email)
                .NotEmpty().EmailAddress().WithMessage("Invalid email address.")
                .Must(EmailAddressUnique).WithMessage("Email already exist! Please try different email address.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.");
        }

        private bool UserNameUnique(string UserName) 
            => !_userRepository.IsUserNameExist(UserName).Result;

        private bool EmailAddressUnique(string Email) 
            => !_userRepository.IsEmailAddressExist(Email).Result;
    }
}
