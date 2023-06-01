namespace TaskNowSoftware.Models
{
    public class ValidateUserResponseModel : UserDTO
    {
        public bool IsValid { get; set; }
        public int UserId { get; set; }
    }

    public class LoginResponseModel
    {
        public int ID { get; set; }
        public string FIRST_NAME { get; set; }
        public string LAST_NAME { get; set; }
    }
}
