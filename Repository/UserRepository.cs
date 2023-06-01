using Dapper;
using System.Data;
using TaskNowSoftware.DbContext;
using TaskNowSoftware.IRepository;
using TaskNowSoftware.Models;

namespace TaskNowSoftware.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;
        public UserRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<RegisterUserResponseDTO> CreateAsync(UserDTO userDTO)
        {
            RegisterUserResponseDTO obj = new RegisterUserResponseDTO();
            try
            {
                var query = "INSERT INTO USERS (USER_NAME, PASSWORD, FIRST_NAME, LAST_NAME)" +
                    " VALUES (@USER_NAME, @PASSWORD, @FIRST_NAME, @LAST_NAME)";

                var parameters = new DynamicParameters();
                parameters.Add("USER_NAME", userDTO.UserName, DbType.String);
                parameters.Add("PASSWORD", userDTO.Password, DbType.String);
                parameters.Add("FIRST_NAME", userDTO.FirstName, DbType.String);
                parameters.Add("LAST_NAME", userDTO.LastName, DbType.String);

                using (var connection = _context.CreateConnection())
                {
                    if (await connection.ExecuteAsync(query, parameters) > 0)
                    {
                        obj.IsValid = true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }

        public async Task<ValidateUserResponseModel> ValidateUser(LoginUserDTO userDTO)
        {
            ValidateUserResponseModel obj = new();
            try
            {
                string _query = $@"SELECT TOP 1 ID, FIRST_NAME, LAST_NAME FROM dbo.USERS WHERE USER_NAME = @UserName AND PASSWORD = @Password";

                using (var connection = _context.CreateConnection())
                {
                    connection.Open();

                    var result = await connection.QueryAsync<LoginResponseModel>(_query, new { UserName = userDTO.UserName, Password = userDTO.Password });

                    if (result != null && result.First() != null)
                    {
                        var _user = result.First();

                        obj.FirstName = _user.FIRST_NAME;
                        obj.LastName = _user.LAST_NAME;
                        obj.UserId = _user.ID;
                        obj.IsValid = true;
                    }
                    connection.Close();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return obj;
        }
    }
}
