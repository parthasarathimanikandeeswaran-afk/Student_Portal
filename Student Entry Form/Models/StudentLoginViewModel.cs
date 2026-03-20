using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class StudentLoginViewModel
    {
        [Required(ErrorMessage = "Ack No is required")]
        public string AckNo { get; set; }


        [Required(ErrorMessage = "Date of Birth is required")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }
    }
}
