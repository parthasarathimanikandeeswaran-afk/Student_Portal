//using System.ComponentModel.DataAnnotations;

//namespace Student_Entry_Form.Models
//{
//    public class StudentEntryModel
//    {
//        [Key]
//        public int Id { get; set; }


//        public string AcknowledgementNo { get; set; }

//        public int SerialNumber { get; set; }
//        public string Name { get; set; }
//        public string Email { get; set; }
//        public string Mobile { get; set; }
//        public DateTime DateOfBirth { get; set; }


//        public string Address { get; set; }
//        public string District { get; set; }
//        public string Taluk { get; set; }
//        public string Village { get; set; }
//        public string Pincode { get; set; }


//        public int Tamil { get; set; }
//        public int English { get; set; }
//        public int Maths { get; set; }
//        public int Science { get; set; }
//        public int Social { get; set; }
//    }
//}










namespace Student_Entry_Form.Models
{


    public class StudentEntryModel
    {
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string Mobile { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

     
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }

        public string District { get; set; } = string.Empty;
        public string Taluk { get; set; } = string.Empty;
        public string Village { get; set; } = string.Empty;
        public string Pincode { get; set; } = string.Empty;

        // Marks
        public int Tamil { get; set; }
        public int English { get; set; }
        public int Maths { get; set; }
        public int Science { get; set; }
        public int Social { get; set; }

        public int SerialNumber { get; set; }
        public string AcknowledgementNo { get; set; } = string.Empty;
    }

}