using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class ResultSearchViewModel
    {
        [Required(ErrorMessage = "Acknowledgement Number is required")]
        public string AckNo { get; set; } = "";


        [Required(ErrorMessage = "Mobile Number is required")]
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Enter valid 10 digit mobile number")]
        public string MobileNo { get; set; } = "";
    }
}
