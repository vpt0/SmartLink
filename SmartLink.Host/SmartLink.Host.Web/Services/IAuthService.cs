using System.Threading.Tasks;
using SmartLink.Host.Web.Models;

namespace SmartLink.Host.Web.Services
{
    public interface IAuthService
    {
        Task<AuthResult> LoginAsync(string username, string password);
        Task<AuthResult> RegisterAsync(string username, string email, string password);
    }
}
// using Microsoft.IdentityModel.Tokens;
