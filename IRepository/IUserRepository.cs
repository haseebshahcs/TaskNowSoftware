using System.Linq.Expressions;
using TaskNowSoftware.Models;

namespace TaskNowSoftware.IRepository
{
    public interface IUserRepository
    {
        Task<RegisterUserResponseDTO> CreateAsync(UserDTO userDTO);
        Task<ValidateUserResponseModel> ValidateUser(LoginUserDTO userDTO);
    }
}
