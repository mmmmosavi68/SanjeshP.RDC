using SanjeshP.RDC.Entities.User;
using System.Threading.Tasks;

namespace SanjeshP.RDC.Services.JWTService
{
    public interface IJwtService
    {
        Task<string> GenerateAsync(User user, UserToken tokens);
    }
}