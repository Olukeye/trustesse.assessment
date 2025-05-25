using System.ComponentModel.DataAnnotations;

namespace Trustesse_Assessment.Dto
{

    public class LoginDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(14, ErrorMessage = "Your Password Is Limited To {2} To {1} Characters", MinimumLength = 4)]
        public string Password { get; set; }
    }
    public record ConfirmEmailDto(
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,

    [Required(ErrorMessage = "Token is required")]
    string Token);
    public class UserDto : LoginDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string phoneNumber { get; set; }

        public ICollection<string> Roles { get; set; }

    }
}
