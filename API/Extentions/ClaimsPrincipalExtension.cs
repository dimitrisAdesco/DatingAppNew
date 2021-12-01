using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Extentions
{
    public static class ClaimsPrincipalExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;   //JwtRegisteredClaimNames.UniqueName apo TokenService
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }

        // public static T GetUserId<T>(this ClaimsPrincipal user)
        // {
        //     var loggedInUserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //     if (typeof(T) == typeof(int))
        //     {
        //         return (T)Convert.ChangeType(loggedInUserId, typeof(T));
        //     }
        //     else
        //     {
        //         throw new Exception("Invalid type provided");
        //     }
        // }
    }
}