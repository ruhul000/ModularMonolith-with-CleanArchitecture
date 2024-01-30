using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using UserAccess.Domain.Factories;
using UserAccess.Domain.Models;
using UserAccess.Infrastructure.Dtos;
using UserAccess.Infrastructure.Repository;

namespace UserAccess.Application.Services
{
    public class UserService : IUserService
    {
        private const int SALTLENGTH = 32;

        private readonly IConfiguration _configuration;
        private readonly IUserFactory _userFactory;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public UserService(IConfiguration configuration, IUserRepository userRepository, IUserFactory userFactory, IAuthenticationService authenticationService, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _userFactory = userFactory;
            _authenticationService = authenticationService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<UserResponse> Registration(UserRequest userRequest)
        {
            UserResponse? response = null;

            var userDto = _userRepository.GetByUserName(userRequest.UserName).Result;

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

            if (!_userRepository.Add(userDto).Result)
            {
                return response;
            }

            await _userRepository.GetById(userDto.UserId);

            return _userFactory.CreateResponseFrom(userDto);

        }
        public async Task<AuthInformation> UserLogin(UserLoginRequest loginRequest)
        {
            AuthInformation authInfo = new AuthInformation();

            User user = VerifyUser(loginRequest);

            if (user == null) return authInfo;

            authInfo.Token = _authenticationService.GenerateJWT(user);
            authInfo.RefreshToken = _authenticationService.GenerateRefreshToken(user.UserName);

            SaveRefreshToken(user.UserName, authInfo.RefreshToken);

            return authInfo;
        }
        public async Task<AuthInformation> RefreshToken(AuthInformation authInfo)
        {
            AuthInformation refreshToken = new AuthInformation();

            // Validate Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSettings = _configuration.GetSection("JWTSettings").Get<JWTSettings>();
            SecurityToken validatedToken;

            var principal = tokenHandler.ValidateToken(authInfo.Token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key))
            }, out validatedToken);


            var _token = validatedToken as JwtSecurityToken;

            if (_token != null && !_token.Header.Alg.Equals(SecurityAlgorithms.HmacSha256))
            {
                return refreshToken;
            }

            var username = principal.Claims.Where(c => c.Type == "UserName")
                   .Select(c => c.Value).SingleOrDefault();

            var refreshTokenDto = _refreshTokenRepository.Get(username, authInfo.RefreshToken).Result;

            if (refreshTokenDto == null)
            {
                return refreshToken;
            }

            var userDto = await _userRepository.GetByUserName(username);

            refreshToken.Token = _authenticationService.GenerateJWT(_userFactory.CreateFrom(userDto));
            refreshToken.RefreshToken = _authenticationService.GenerateRefreshToken(username);

            SaveRefreshToken(username, refreshToken.RefreshToken);

            return refreshToken;
        }
        private User VerifyUser(UserLoginRequest loginRequest)
        {
            User? response = null;

            var userDto = _userRepository.GetByUserName(loginRequest.UserName).Result;

            if (userDto == null) return response;

            var passwordHash = _authenticationService.EncryptPassword(loginRequest.Password, userDto.Salt);

            if (passwordHash.Equals(userDto.Password) && userDto.IsActive == true)
            {
                response = _userFactory.CreateFrom(userDto);
            }

            return response;
        }
        private void SaveRefreshToken(string username, string refreshToken)
        {
            RefreshTokenDto refreshTokenDto = new RefreshTokenDto
            {
                UserName = username,
                TokenID = new Random().Next().ToString(),
                RefreshToken = refreshToken,
                IsActive = true
            };

            _refreshTokenRepository.Save(refreshTokenDto);

        }

    }
}
