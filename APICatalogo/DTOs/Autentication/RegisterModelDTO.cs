using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs.Autentication
{
    public class RegisterModelDTO
    {
        [Required(ErrorMessage = "User name is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "User name is required")]
        public string Password { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "EmailAddress is invalid")]
        public string Email { get; set; }
    }
}
