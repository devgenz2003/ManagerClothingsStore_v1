using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CHERRY.BUS.ViewModels.User
{
    public class RegisterUser
    {
        [Required(ErrorMessage = "Username cannot be blank")]
        public string? Username { get; set; }

        [Required(ErrorMessage = "Email cannot be blank")]
        [EmailAddress]
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? SurName { get; set; }
        public string? PhoneNumber { get; set; }
        public int Gender { get; set; }

        [Required(ErrorMessage = "Password cannot be blank")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Confirm password cannot be blank")]
        public string? ConfirmPassword { get; set; }
        public int Status {  get; set; }
    }
}
