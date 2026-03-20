using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int SerialNumber { get; set; } = 2000;
        public string AckNo { get; set; }

        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; }

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DateOfBirth { get; set; }

       [Required(ErrorMessage = "Address Line 1 is required")]
        [StringLength(200, ErrorMessage = "Address Line 1 cannot exceed 200 characters")]
        public string AddressLine1 { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Address Line 2 cannot exceed 200 characters")]
        public string? AddressLine2 { get; set; }


        [Required(ErrorMessage = "District is required")]
        public string District { get; set; }

        [Required(ErrorMessage = "Taluk is required")]
        public string Taluk { get; set; }

        [Required(ErrorMessage = "Village is required")]
        public string Village { get; set; }

     
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pincode")]
        public string Pincode { get; set; }
    }
}
