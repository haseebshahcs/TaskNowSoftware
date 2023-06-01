using System.ComponentModel.DataAnnotations;

namespace TaskNowSoftware.Models
{
    public class UserDTO : LoginUserDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class LoginUserDTO
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Password is limited to {2} to {1} characters", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string Device { get; set; }
        public string IpAddress { get; set; }
    }

}
