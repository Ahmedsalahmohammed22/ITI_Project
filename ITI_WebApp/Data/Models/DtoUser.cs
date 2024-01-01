using System.ComponentModel.DataAnnotations;

namespace TestRestApi.Data.Models
{
    public class DtoUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
