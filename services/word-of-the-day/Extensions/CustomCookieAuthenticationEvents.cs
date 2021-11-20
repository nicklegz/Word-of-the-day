using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;

namespace word_of_the_day.Extensions
{
   public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents 
   {
      private CookieSigningInContext _context;
      public override Task SigningIn(CookieSigningInContext context)
      {
         _context = context;
         bool sameSiteCompatible = CheckSameSiteCompatibility();
         if(sameSiteCompatible == true)
         {
            _context.CookieOptions.SameSite = SameSiteMode.None;
         }

         else{
            _context.CookieOptions.SameSite = SameSiteMode.Unspecified;
         }

         _context.CookieOptions.Secure = true;
         _context.CookieOptions.HttpOnly = true;
         return Task.CompletedTask;
      }

      private bool CheckSameSiteCompatibility()
      {
         var userAgent = _context.Request.Headers["User-Agent"].ToString();
         if (userAgent.Contains("CPU iPhone OS 12")
               || userAgent.Contains("iPad; CPU OS 12"))
            {
               return false;
            }

            if (userAgent.Contains("Safari")
               && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
               && userAgent.Contains("Version/"))
            {
               return false;
            }

            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6"))
            {
               return false;
            }

            return true;
      }
   }
}
