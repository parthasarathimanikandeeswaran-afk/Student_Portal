using System.ComponentModel.DataAnnotations;

namespace Student_Entry_Form.Models
{
    public class User
    {
        public int UserId { get; set; }


        [Required]
        [StringLength(50)]
        public string Username { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        [Required]
        public string Role { get; set; }


        public string CreatedAt { get; set; }
    }
}
