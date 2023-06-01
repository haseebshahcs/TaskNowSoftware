using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TaskNowSoftware.IRepository;
using TaskNowSoftware.Models;
using TaskNowSoftware.Services;

namespace TaskNowSoftware.Controllers.User
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IAuthManager _authManager;
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository,
            ILogger<UsersController> logger, IMapper mapper, IAuthManager authManager)
        {
            _userRepository = userRepository;
            _logger = logger;
            _authManager = authManager;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="firstName">firstName</param>
        /// <param name="lastName">lastName</param>
        /// <param name="userName">lastName</param>
        /// <param name="password">password</param>
        /// <param name="device">device</param>
        /// <param name="ipaddress">ipaddress</param>
        /// <returns>Ok, when user get created successfully</returns> 
        [HttpPost]
        [Route("Signup")]
        public async Task<ActionResult> Signup([FromBody] UserDTO userDTO)
        {
            _logger.LogInformation($"Registration attempt for {userDTO.UserName} from device {userDTO.Device}, IP: {userDTO.IpAddress}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                RegisterUserResponseDTO result = await _userRepository.CreateAsync(userDTO); //here password is applied to be hashed and saved in db

                if (!result.IsValid)
                {
                    return BadRequest(ModelState);
                }
                return Accepted();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Something went wrong in {nameof(Signup)}");
                return Problem($"Something went wrong in {nameof(Signup)}", statusCode: 500); //its is also a way of returning error Problem method
            }
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="userName">lastName</param>
        /// <param name="password">password</param>
        /// <param name="device">device</param>
        /// <param name="ipaddress">ipaddress</param>
        /// <returns>Ok, when user get created successfully</returns> 

        [HttpPost]
        [Route("Authenticate")]
        public async Task<ActionResult> Authenticate([FromBody] LoginUserDTO userDTO)
        {
            _logger.LogInformation($"Login attempt for {userDTO.UserName} from device {userDTO.Device}, IP: {userDTO.IpAddress}");
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                ValidateUserResponseModel result = await _userRepository.ValidateUser(userDTO);
                if (!result.IsValid)
                {
                    return Unauthorized();
                }
                return Accepted(new { FirstName = result.FirstName, LastName = result.LastName, Token = _authManager.CreateToken(result.UserId) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, $"Something went wrong in {nameof(Authenticate)}");
                return Problem($"Something went wrong in {nameof(Authenticate)}", statusCode: 500); //its is also a way of returning error Problem method
            }
        }
    }
}
