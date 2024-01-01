using System.ComponentModel.DataAnnotations;

namespace TestRestApi.Data.Models
{
    public class DtoNewUser
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }

        public string? phoneNumber { get; set; }
    }
}
