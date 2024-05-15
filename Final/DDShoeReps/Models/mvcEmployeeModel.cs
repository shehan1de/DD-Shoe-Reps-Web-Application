using System;
using System.ComponentModel.DataAnnotations;


namespace DDShoeReps.Models
{
    public class mvcEmployeeModel
    { 
        public int UserID { get; set; }

        [Required(ErrorMessage = "User Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string UserEmail { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "UserPassword is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string UserPassword { get; set; }

        
        [Compare("UserPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "UserType is required")]
        public string UserType { get; set; }

       
        [MaxLength(10, ErrorMessage = "Contact Number must be at least 10 characters long")]
        public Nullable<int> ContactNumber { get; set; }

        public string IDNumber { get; set; }

        public string Address { get; set; }

        public string ProfilePicture { get; set; }

    }
}