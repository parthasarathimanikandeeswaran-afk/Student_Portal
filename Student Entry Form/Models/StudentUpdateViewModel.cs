using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class StudentUpdateViewModel
    {

        // 🔑 Identity
        public int StudentId { get; set; }

        // 🔒 Read-only
        public string AckNo { get; set; } = string.Empty;

        // 👤 Personal Details
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid Mobile Number")]
        public string Mobile { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        public DateTime? DateOfBirth { get; set; }

        // 📍 Address
        [Required(ErrorMessage = "Address is required")]
        public string AddressLine1 { get; set; } = string.Empty;

        [Required(ErrorMessage = "District is required")]
        public string District { get; set; } = string.Empty;

        [Required(ErrorMessage = "Taluk is required")]
        public string Taluk { get; set; } = string.Empty;

        [Required(ErrorMessage = "Village is required")]
        public string Village { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pincode is required")]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "Invalid Pincode")]
        public string Pincode { get; set; } = string.Empty;

        // 📚 Marks (REQUIRED)
        [Required(ErrorMessage = "Tamil marks required")]
        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
        public int Tamil { get; set; }

        [Required(ErrorMessage = "English marks required")]
        [Range(0, 100)]
        public int English { get; set; }

        [Required(ErrorMessage = "Maths marks required")]
        [Range(0, 100)]
        public int Maths { get; set; }

        [Required(ErrorMessage = "Science marks required")]
        [Range(0, 100)]
        public int Science { get; set; }

        [Required(ErrorMessage = "Social marks required")]
        [Range(0, 100)]
        public int Social { get; set; }
    }
}
