using TaskNowSoftware.Models;

namespace TaskNowSoftware.Services
{
    public interface IAuthManager
    {
        string CreateToken(int userId);
    }
}
