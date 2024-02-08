using AutoMapper.Configuration.Annotations;
using FluentValidation;
using Helper.Extensions;
using Helper.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UserAccess.Domain.Factories;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Dtos;
using UserAccess.Infrastructure.Repositories;

namespace UserAccess.Application.Services
{
    public class UserService : IUserService
    {
        private const int SALTLENGTH = 32;

        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IValidator<UserRequest> _userRegistrationValidator;
        private readonly IValidator<UserLoginRequest> _userLoginValidator;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserFactory _userFactory;
        private readonly IUnitOfWork _uowRepository;
        private readonly IEmailSender _emailSender;
        private const string RESET_PASSWORD_ROUTE = "/api/v1/ResetPassword";
        private const string VERIFY_EMAIL_ADDRESS_ROUTE = "/api/v1/VerifyEmail";
        public UserService(IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IValidator<UserRequest> userRegistrationValidator,
            IValidator<UserLoginRequest> userLoginValidator,
            IAuthenticationService authenticationService,
            IUserFactory userFactory,
            IUnitOfWork uowRepository,
            IEmailSender emailSender)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _userRegistrationValidator = userRegistrationValidator;
            _userLoginValidator = userLoginValidator;
            _authenticationService = authenticationService;
            _userFactory = userFactory;
            _uowRepository = uowRepository;
            _emailSender = emailSender;
        }

        public async Task<Result<string>> Registration(UserRequest userRequest)
        {

            var result = await _userRegistrationValidator.ValidateAsync(userRequest);  
            if(!result.IsValid)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                return Result<string>.Failure(new Error("Registration",  errorMessage));
            }

            var user = CreateUser(userRequest);

            var userDto = _userFactory.CreateFrom(user);

            _uowRepository.UserRepository.Add(userDto);

            if (await _uowRepository.SaveAsync())
            {
                var verifyEmailAddressURI = BuildAbsoluteURI(VERIFY_EMAIL_ADDRESS_ROUTE, $"?verificationToken={userDto.VerificationToken}");

                // Send email with email verification link link
                var forgotEmailBody = $"Please use the following link to verify your email address: {verifyEmailAddressURI}";
                var to = userDto.Email;
                var subject = "Verify Email - ControlHubApp";
                var body = forgotEmailBody;
                await _emailSender.SendEmailAsync(to, subject, body);
                return Result<string>.Success("Registration successful! Please check your email for the verification link. Thank you.");
            }

            return Result<string>.Failure(new Error("Registration", "Something went wrong! Please try again."));
        }
        public async Task<Result<AuthInformation>> UserLogin(UserLoginRequest loginRequest)
        {
            var result = await _userLoginValidator.ValidateAsync(loginRequest);
            if (!result.IsValid)
            {
                var errorMessage = string.Join(", ", result.Errors.Select(e => e.ErrorMessage));
                return Result<AuthInformation>.Failure(new Error("Login", errorMessage));
            }

            var userDto = await _uowRepository.UserRepository.GetByUserName(loginRequest.UserName);
            
            if (userDto == null)
            {
                return Result<AuthInformation>.Failure(new Error("Login", "Invalid username. Please check your username and try again."));
            }

            if (!userDto.IsVerified)
            {
                await ResendVerificationToken(userDto);
                return Result<AuthInformation>.Failure(new Error("Login", "Your email address is not verified. Another verification link has been sent to your email. Please check your inbox."));
            }

            var user = _userFactory.CreateFrom(userDto);

            if(!VerifyPassword(user, loginRequest.Password))
            {
                return Result<AuthInformation>.Failure(new Error("Login", "Incorrect password. Please check your password and try again."));
            }

            AuthInformation authInfo = new AuthInformation();
            authInfo.Token = _authenticationService.GenerateJWT(user);
            authInfo.RefreshToken = _authenticationService.GenerateRefreshToken();
            
            if(await SaveRefreshToken(user.UserName, authInfo.RefreshToken))
            {
                return Result<AuthInformation>.Success(authInfo);
            }

            return Result<AuthInformation>.Failure(new Error("Login", "Something went wrong! Please try again."));
        }
        public async Task<AuthInformation> RefreshToken(AuthInformation authInfo)
        {
            AuthInformation refreshToken = new AuthInformation();

            // Get principal after token validation        
            var principal = _authenticationService.GetPrincipalFromTokenValidation(authInfo.Token);
            
            if (principal?.Identity?.Name is null)
            {
                return refreshToken;
            }

            var username = principal.Identity.Name;

            var refreshTokenDto = await _uowRepository.RefreshTokenRepository.GetByUserName(username);

            if (refreshTokenDto == null)
            {
                return refreshToken;
            }

            var userDto = await _uowRepository.UserRepository.GetByUserName(username);

            refreshToken.Token = _authenticationService.GenerateJWT(_userFactory.CreateFrom(userDto));
            refreshToken.RefreshToken = _authenticationService.GenerateRefreshToken();

            await SaveRefreshToken(username, refreshToken.RefreshToken);

            return refreshToken;
        }
        public async Task<Result<string>> VerifyEmailAddress(string verificationToken)
        {
            var userDto = await _uowRepository.UserRepository.GetUserByVerificationToken(verificationToken);

            if (userDto == null || userDto?.IsDeleted == true)
            {
                return Result<string>.Failure(new Error("EmailVerification", "Invalid verification token"));
            }

            userDto.IsVerified = true;
            userDto.VerificationToken = null;

            _uowRepository.UserRepository.Update(userDto);

            if (await _uowRepository.SaveAsync())
            {
                return Result<string>.Success("Congratulations! Email verified successfully. Please try login.");
            }

            return Result<string>.Failure(new Error("Registration", "Something went wrong! Please try again."));
        }
        public async Task<Result<string>> ForgotPassword(string email)
        {
            var userDto = await _uowRepository.UserRepository.GetByUserName(email);

            if (userDto == null || userDto?.IsDeleted == true)
            {
                return Result<string>.Failure(new Error("ForgotPassword", "User not found for the provided email address."));
            }

            userDto.PasswordResetToken = _authenticationService.GenerateUniqueToken();
            userDto.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(1);

            _uowRepository.UserRepository.Update(userDto);

            if (await _uowRepository.SaveAsync())
            {
                var resetPasswordUri = BuildAbsoluteURI(RESET_PASSWORD_ROUTE, $"?passwordResetToken={userDto.PasswordResetToken}");

                // Send email with reset password link
                var forgotEmailBody = $"Please use the following link to reset your password: {resetPasswordUri}\r\nThis link will expire within an hour";
                var to = email;
                var subject = "Forgot Password - ControlHubApp";
                var body = forgotEmailBody;
                await _emailSender.SendEmailAsync(to,subject, body);

                return Result<string>.Success("A password reset link has been generated for your account. Please use it to change your password. This link will expire within an hour.");
            }

            return Result<string>.Failure(new Error("ForgotPassword", "Something went wrong! Please try again."));
        }
        public async Task<Result<string>> ResetPassword(string passwordResetToken, string newPassword)
        {
            var userDto = await _uowRepository.UserRepository.GetUserByPasswordResetToken(passwordResetToken);

            if (userDto == null || userDto?.IsDeleted == true)
            {
                return Result<string>.Failure(new Error("ResetPassword", "Invalid token."));
            }

            if (userDto?.PasswordResetTokenExpiry < DateTime.UtcNow)
            {
                return Result<string>.Failure(new Error("ResetPassword", "Password reset token expired. Please try ''Forget Password'' again."));
            }


            userDto.Password = _authenticationService.EncryptPassword(newPassword, userDto.Salt);
            userDto.PasswordResetToken = null;
            userDto.PasswordResetTokenExpiry = null;

            _uowRepository.UserRepository.Update(userDto);

            if (await _uowRepository.SaveAsync())
            {
                return Result<string>.Success("Password updated successfully. Please log in with your new password.");
            }

            return Result<string>.Failure(new Error("ForgotPassword", "Something went wrong! Please try again."));
        }
        public async Task<bool> ResendVerificationToken(UserDto userDto)
        {
            userDto.VerificationToken = _authenticationService.GenerateUniqueToken();
           

            // Send email with email verification link link
            var verifyEmailAddressURI = BuildAbsoluteURI(VERIFY_EMAIL_ADDRESS_ROUTE, $"?verificationToken={userDto.VerificationToken}");
            var forgotEmailBody = $"Please use the following link to verify your email address: {verifyEmailAddressURI}";
            var to = userDto.Email;
            var subject = "Verify Email - ControlHubApp";
            var body = forgotEmailBody;
            await _emailSender.SendEmailAsync(to, subject, body);
           
            _uowRepository.UserRepository.Update(userDto);
            return await _uowRepository.SaveAsync();
        }
        private User CreateUser(UserRequest userRequest)
        {
            var salt = _authenticationService.GenerateSalt();
            var passwordHash = _authenticationService.EncryptPassword(userRequest.Password, salt);
            var verificationToken = _authenticationService.GenerateUniqueToken();
            User user = new User
            {
                FullName = userRequest.FullName,
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                Password = passwordHash,
                Salt = salt,
                VerificationToken = verificationToken,
                IsVerified = false,
                IsActive = true,
                IsDeleted = false
            };
            return user;
        }
        private bool VerifyPassword(User user, string password)
        {
            var passwordHash = _authenticationService.EncryptPassword(password, user.Salt);
            return  passwordHash.Equals(user.Password) && user.IsActive == true;
        }
        private async Task<bool> SaveRefreshToken(string username, string refreshToken)
        {
            var refreshTokenDto = await _uowRepository.RefreshTokenRepository.GetByUserName(username);

            if (refreshTokenDto != null)
            {
                refreshTokenDto.RefreshToken = refreshToken;
            }
            else
            {
                RefreshTokenDto newRefreshTokenDto = new RefreshTokenDto
                {
                    UserName = username,
                    TokenID = new Random().Next().ToString(),
                    RefreshToken = refreshToken,
                    IsActive = true
                };

                _uowRepository.RefreshTokenRepository.Add(newRefreshTokenDto);
            }

            return await _uowRepository.SaveAsync();
        }
        private string BuildAbsoluteURI(string route, string queryString)
        {
            return UriHelper.BuildAbsolute(
                   _httpContextAccessor.HttpContext.Request.Scheme,
                   _httpContextAccessor.HttpContext.Request.Host,
                   _httpContextAccessor.HttpContext.Request.PathBase,
                   route,
                   new QueryString(queryString)
               );
        }

    }
}
