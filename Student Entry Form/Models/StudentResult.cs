using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class StudentResult
    {
        public int ResultId { get; set; }


        [Required]
        public int StudentId { get; set; }


        [Required]
        [Range(0, 500)]
        public int TotalMarks { get; set; }


        [Required]
        [Range(0, 100)]
        public decimal Percentage { get; set; }


        [Required]
        public string ResultStatus { get; set; } // Pass / Fail
    }
}
