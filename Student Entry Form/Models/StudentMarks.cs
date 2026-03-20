using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class StudentMarks
    {
        public int MarkId { get; set; }


        [Required]
        public int StudentId { get; set; }


        [Required]
        public int SubjectId { get; set; }


        [Required]
        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
        public int Marks { get; set; }
    }
}
