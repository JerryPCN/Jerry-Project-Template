using System.ComponentModel.DataAnnotations;

namespace JerryPlat.Models.Dto
{
    public class PasswordDto
    {
        [Required]
        public string Original { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}