using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace APi.Dtos
{
    public class LoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
