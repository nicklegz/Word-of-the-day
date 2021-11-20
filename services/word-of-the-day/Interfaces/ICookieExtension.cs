using System.Security.Claims;
using System.Threading.Tasks;

namespace word_of_the_day.Interfaces
{
    public interface ICookieExtension
    {
        bool CheckSameSiteCompatibility();
    }
}