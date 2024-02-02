using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
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
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserFactory _userFactory;
        private readonly IUnitOfWork _uowRepository;

        public UserService(IConfiguration configuration, IAuthenticationService authenticationService, IUserFactory userFactory, IUnitOfWork uowRepository)
        {
            _configuration = configuration;
            _authenticationService = authenticationService;
            _userFactory = userFactory;
            _uowRepository = uowRepository;
        }

        public async Task<UserResponse> Registration(UserRequest userRequest)
        {
            UserResponse? response = null;

            var userDto = await _uowRepository.UserRepository.GetByUserName(userRequest.UserName);

            if (userDto != null) return response;

            var salt = _authenticationService.GenerateSalt();
            var passwordHash = _authenticationService.EncryptPassword(userRequest.Password, salt);

            User user = new User
            {
                FullName = userRequest.FullName,
                UserName = userRequest.UserName,
                Email = userRequest.Email,
                Password = passwordHash,
                Salt = salt,
                IsActive = true
            };

            userDto = _userFactory.CreateFrom(user);

            _uowRepository.UserRepository.Add(userDto);

            if (await _uowRepository.SaveAsync())
            {
                return response;
            }

            //await _uowRepository.UserRepository.GetById(userDto.UserId);

            return _userFactory.CreateResponseFrom(userDto);

        }
        public async Task<AuthInformation> UserLogin(UserLoginRequest loginRequest)
        {
            AuthInformation authInfo = new AuthInformation();

            User user = await VerifyUser(loginRequest);

            if (user == null) return authInfo;

            authInfo.Token = _authenticationService.GenerateJWT(user);
            authInfo.RefreshToken = _authenticationService.GenerateRefreshToken();

            await SaveRefreshToken(user.UserName, authInfo.RefreshToken);

            return authInfo;
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
        private async Task<User> VerifyUser(UserLoginRequest loginRequest)
        {
            User? response = null;

            var userDto = await _uowRepository.UserRepository.GetByUserName(loginRequest.UserName);

            if (userDto == null) return null;

            var passwordHash = _authenticationService.EncryptPassword(loginRequest.Password, userDto.Salt);

            if (passwordHash.Equals(userDto.Password) && userDto.IsActive == true)
            {
                response = _userFactory.CreateFrom(userDto);
            }

            return response;
        }
        private async Task<bool> SaveRefreshToken(string username, string refreshToken)
        {
            var refreshTokenDto = await _uowRepository.RefreshTokenRepository.GetByUserName(username);

            if(refreshTokenDto != null)
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

    }
}
