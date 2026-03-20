namespace Student_Entry_Form.Models
{
    public class StudentResultViewModel
    {
        public string AckNo { get; set; } = "";
        public string FullName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Mobile { get; set; } = "";

        public int TotalMarks { get; set; }
        public decimal Percentage { get; set; }
        public string ResultStatus { get; set; } = "";

        public List<StudentMarkViewModel> Marks { get; set; }
            = new List<StudentMarkViewModel>();
    }
}
