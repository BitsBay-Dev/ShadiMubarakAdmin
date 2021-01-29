using Google.Cloud.Firestore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShadiMubarak.Models
{
    [FirestoreData]
    public class Admin
    {
        [FirestoreProperty]
        public string FirstName { get; set; }
        [FirestoreProperty]
        public string Id { get; set; }
        [FirestoreProperty] 
        public string LastName { get; set; }
        [FirestoreProperty] 
        public bool IsDeleted { get; set; }
        [FirestoreProperty] 
        public bool isActivated { get; set; }
        [FirestoreProperty] 
        public string PhoneNumber { get; set; }
        [FirestoreProperty]
        [Required(ErrorMessage = "Please Enter Correct Email.")]
        [RegularExpression("^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$", ErrorMessage = "E-mail is not valid")]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [FirestoreProperty]
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [FirestoreProperty]
        public string Image { get; set; }
    }
}