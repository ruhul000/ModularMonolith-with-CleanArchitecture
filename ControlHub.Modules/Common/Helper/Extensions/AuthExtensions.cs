using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Helper.Extensions
{
    public static class AuthExtensions
    {
        public static IEnumerable<Claim> GetIdentityClaims(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;
            
            if (identity != null)
            {
                return identity.Claims;
            }

            return Enumerable.Empty<Claim>();
        }

        public static int GetClaimsUserId(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            int userId = identity.Claims.Any() ? identity.Claims.Where(o => o.Type == "UserId").Select(o => o.Value).Single().ToInt32() : 0;

            return userId;
        }

        public static string GetClaimsUserName(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            var userName = identity.Claims.Any() ? identity.Claims.Where(o => o.Type == "UserName").Select(o => o.Value).Single().ToString() : string.Empty;

            return userName;
        }

        public static string GetClaimsFullName(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            var fullName = identity.Claims.Any() ? identity.Claims.Where(o => o.Type == "FullName").Select(o => o.Value).Single().ToString() : string.Empty;

            return fullName;
        }

        public static string GetClaimsPassword(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            var password = identity.Claims.Any() ? identity.Claims.Where(o => o.Type == "Password").Select(o => o.Value).Single().ToString() : string.Empty;

            return password;
        }

        public static string GetClaimsSalt(HttpContext context)
        {
            var identity = context.User.Identity as ClaimsIdentity;

            var salt = identity.Claims.Any() ? identity.Claims.Where(o => o.Type == "Salt").Select(o => o.Value).Single().ToString() : string.Empty;

            return salt;
        }
    }
}
