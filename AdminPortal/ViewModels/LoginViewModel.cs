using AdminPortal.Controllers;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;


namespace AdminPortal.ViewModels
{

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
