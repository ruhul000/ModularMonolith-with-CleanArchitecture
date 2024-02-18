using UserAccess.Domain.Models;

namespace UserAccess.Application.Services
{
    public interface IUserService
    {
        Task<Result<string>> Registration(UserRequest userRequest);
        Task<Result<AuthInformation>> UserLogin(UserLoginRequest loginRequest);
        Task<AuthInformation> RefreshToken(AuthInformation authInfo);
        Task<Result<string>> VerifyEmailAddress(string verificationToken);
        Task<Result<string>> ForgotPassword(string email);
        Task<Result<string>> ResetPassword(string email, string resetCode, string newPassword);
    }
}
