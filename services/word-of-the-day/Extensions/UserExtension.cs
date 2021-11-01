using System.Linq;
using System.Security.Claims;

public static class UserExtension
{
    public static string GetUserId(ClaimsPrincipal user)
    {
        var userId = ((ClaimsIdentity)user.Identity).Claims.Select(x => x.Value).FirstOrDefault();
        return userId;
    }   
}