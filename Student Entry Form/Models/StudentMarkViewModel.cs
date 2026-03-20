using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class StudentMarkViewModel
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = "";

        [Required(ErrorMessage = "Marks required")]
        [Range(0, 100, ErrorMessage = "Marks must be between 0 and 100")]
        public int Marks { get; set; }
    }
}
