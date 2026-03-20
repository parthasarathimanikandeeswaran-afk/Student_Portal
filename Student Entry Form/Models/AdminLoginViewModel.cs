using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class AdminLoginViewModel
    {
        [Required]
        public string Username { get; set; } = null;


        [Required]
        public string Password { get; set; }
    }
}
