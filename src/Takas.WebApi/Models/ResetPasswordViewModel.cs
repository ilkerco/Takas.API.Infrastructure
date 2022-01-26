using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Takas.WebApi.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(17,MinimumLength =7)]
        public string NewPassword { get; set; }

        [Required]
        [StringLength(17, MinimumLength = 7)]
        public string ConfirmPassword { get; set; }
    }
}
