namespace Student_Entry_Form.Models
{
    public class StudentEditViewModel
    {
       
        public Student Student { get; set; } = new();
        public List<StudentMarkViewModel> Marks { get; set; } = new();

    }
}
