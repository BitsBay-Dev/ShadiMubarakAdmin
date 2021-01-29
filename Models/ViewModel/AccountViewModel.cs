using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models.ViewModel
{
    public class AccountViewModel
    {

        public class ManageUserViewModel
        {
            public string Id { get; set; }

            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Email")]
            public string UserName { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string role { get; set; }

            public HttpPostedFileBase ImageFile { get; set; }
        }


        [NotMapped]
        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Please Enter First Name.")]
            [Display(Name = "First Name")]
            public string firstname { get; set; }

            [Required(ErrorMessage = "Please Enter Last Name.")]
            [Display(Name = "Last Name")]
            public string lastname { get; set; }

            [Required(ErrorMessage = "Please Enter Correct Email.")]
            [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
            [Display(Name = "Email")]
            public string UserName { get; set; }

            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }

            public string Role { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public HttpPostedFileBase ImageFile { get; set; }
        }
    }
}