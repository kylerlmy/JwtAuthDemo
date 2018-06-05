using System;
using System.ComponentModel.DataAnnotations;
namespace JwtAuth.ViewModels
{
   public class LoginViewModel
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Password { get; set; }
    }
}