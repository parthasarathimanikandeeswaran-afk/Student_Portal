using Microsoft.Data.SqlClient;
using Student_Entry_Form.Models;
using System.Data;

namespace Student_Entry_Form.Models.Repository
{
    public class StudentRepository
    {
        private readonly string connectionString;

        public StudentRepository(IConfiguration config)
        {
            connectionString = config.GetConnectionString("DbConnection");    
        }


        public int GetLastSerialNumber()
        {
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            SqlCommand cmd = new SqlCommand("SELECT ISNULL(MAX(SerialNumber), 2000) FROM Students", con);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }


        public int StudentEntryMethod(StudentEntryModel model)
        {
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string sql = @"
INSERT INTO Students
(AckNo, FullName, Email, Mobile, DateOfBirth, AddressLine1,AddressLine2, District, Taluk, Village, Pincode, SerialNumber)
VALUES
(@AckNo,@Name,@Email,@Mobile,@DOB,@Addr1,@Addr2,@District,@Taluk,@Village,@Pincode,@Serial);

SELECT StudentId FROM Students WHERE AckNo = @AckNo;
";

            using SqlCommand cmd = new SqlCommand(sql, con);

       
            DateTime dob = model.DateOfBirth < new DateTime(1753, 1, 1)
                ? new DateTime(2000, 1, 1)
                : model.DateOfBirth;

            cmd.Parameters.AddWithValue("@AckNo", model.AcknowledgementNo);
            cmd.Parameters.AddWithValue("@Name", model.Name);
            cmd.Parameters.AddWithValue("@Email", model.Email ?? "");
            cmd.Parameters.AddWithValue("@Mobile", model.Mobile);
            cmd.Parameters.AddWithValue("@DOB", dob);
            cmd.Parameters.AddWithValue("@Addr1", model.AddressLine1 ?? "");
            cmd.Parameters.AddWithValue("@Addr2", model.AddressLine2 ?? "");
            cmd.Parameters.AddWithValue("@District", model.District ?? "");
            cmd.Parameters.AddWithValue("@Taluk", model.Taluk ?? "");
            cmd.Parameters.AddWithValue("@Village", model.Village ?? "");
            cmd.Parameters.AddWithValue("@Pincode", model.Pincode ?? "");
            cmd.Parameters.AddWithValue("@Serial", model.SerialNumber);

            int studentId = Convert.ToInt32(cmd.ExecuteScalar());

            return studentId;
        }





        public int GetNextSerialNumber()
        {
            using var con = new SqlConnection(connectionString);
            con.Open();

            using var cmd = new SqlCommand(
                "SELECT ISNULL(MAX(SerialNumber), 20000) + 1 FROM Students WITH (TABLOCKX)", con);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }



        public void StoreStudentMarks(int studentId, StudentEntryModel model)
        {
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();


            string sql = @"INSERT INTO StudentMarks (StudentId, SubjectId, Marks)
VALUES (@StudentId, @SubjectId, @Marks)";


            using SqlCommand cmd = new SqlCommand(sql, con);


            void InsertMark(int subjectId, int marks)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@SubjectId", subjectId);
                cmd.Parameters.AddWithValue("@Marks", marks);
                cmd.ExecuteNonQuery();
            }


            InsertMark(1, model.Tamil);
            InsertMark(2, model.English);
            InsertMark(3, model.Maths);
            InsertMark(4, model.Science);
            InsertMark(5, model.Social);
        }


        public void InsertStudentWithMarks(StudentEditViewModel model)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();

            using var tran = con.BeginTransaction(IsolationLevel.Serializable);

            try
            {
               

                string studentSql = @"
        INSERT INTO Students
        (AckNo, FullName, Email, Mobile, DateOfBirth, AddressLine1, AddressLine2,
         District, Taluk, Village, Pincode, SerialNumber)
        VALUES
        (@AckNo,@Name,@Email,@Mobile, @Addr1,@Addr2,@Dob,@Addr,
         @Dist,@Taluk,@Village,@Pin,@Serial);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using var cmd = new SqlCommand(studentSql, con, tran);

                var s = model.Student;

                cmd.Parameters.Add("@AckNo", SqlDbType.VarChar, 20).Value = s.AckNo;
                cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = s.FullName;
                cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = (object?)s.Email ?? DBNull.Value;
                cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 15).Value = s.Mobile;
                cmd.Parameters.Add("@Dob", SqlDbType.Date).Value = s.DateOfBirth;
                cmd.Parameters.AddWithValue("@Addr1", s.AddressLine1);
                cmd.Parameters.AddWithValue("@Addr2", s.AddressLine2 ?? "");
                cmd.Parameters.Add("@Dist", SqlDbType.VarChar, 100).Value = s.District;
                cmd.Parameters.Add("@Taluk", SqlDbType.VarChar, 100).Value = s.Taluk;
                cmd.Parameters.Add("@Village", SqlDbType.VarChar, 100).Value = s.Village;
                cmd.Parameters.Add("@Pin", SqlDbType.VarChar, 10).Value = s.Pincode;
                cmd.Parameters.Add("@Serial", SqlDbType.Int).Value = s.SerialNumber;

                int studentId = Convert.ToInt32(cmd.ExecuteScalar());

            

                string markSql = @"INSERT INTO StudentMarks (StudentId, SubjectId, Marks)
                           VALUES (@Sid,@SubId,@Marks)";

                foreach (var m in model.Marks)
                {
                    using var cmdMark = new SqlCommand(markSql, con, tran);

                    cmdMark.Parameters.Add("@Sid", SqlDbType.Int).Value = studentId;
                    cmdMark.Parameters.Add("@SubId", SqlDbType.Int).Value = m.SubjectId;
                    cmdMark.Parameters.Add("@Marks", SqlDbType.Int).Value = m.Marks;

                    cmdMark.ExecuteNonQuery();
                }


                int total = model.Marks.Sum(x => x.Marks);
                int subjectCount = model.Marks.Count;

                decimal percent = subjectCount == 0 ? 0 :
                                  Math.Round(total * 100m / (subjectCount * 100), 2);

                string status = percent >= 35 ? "Pass" : "Fail";


                string resultSql = @"INSERT INTO StudentResults
        (StudentId, TotalMarks, Percentage, ResultStatus)
        VALUES (@Sid,@Total,@Percent,@Status)";

                using var cmdResult = new SqlCommand(resultSql, con, tran);

                cmdResult.Parameters.Add("@Sid", SqlDbType.Int).Value = studentId;
                cmdResult.Parameters.Add("@Total", SqlDbType.Int).Value = total;
                cmdResult.Parameters.Add("@Percent", SqlDbType.Decimal).Value = percent;
                cmdResult.Parameters.Add("@Status", SqlDbType.VarChar, 10).Value = status;

                cmdResult.ExecuteNonQuery();

                tran.Commit();
            }
            catch
            {
                tran.Rollback();
                throw;
            }
        }


        public StudentResultViewModel? GetStudentResult(string ackNo, string mobile)
        {
            string query = @"
                SELECT 
                    s.AckNo,
                    s.FullName,
                    s.Email,
                    s.Mobile,
                    sub.SubjectName,
                    m.Marks,
                    r.TotalMarks,
                    r.Percentage,
                    r.ResultStatus
                FROM Students s
                JOIN StudentMarks m ON s.StudentId = m.StudentId
                JOIN Subjects sub ON m.SubjectId = sub.SubjectId
                JOIN StudentResults r ON s.StudentId = r.StudentId
                WHERE s.AckNo = @AckNo AND s.Mobile = @Mobile
                ORDER BY sub.SubjectId";

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@AckNo", ackNo);
            cmd.Parameters.AddWithValue("@Mobile", mobile);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            if (!reader.HasRows)
                return null;

            StudentResultViewModel result = new StudentResultViewModel
            {
                Marks = new List<StudentMarkViewModel>()
            };

            while (reader.Read())
            {
                if (string.IsNullOrEmpty(result.AckNo))
                {
                    result.AckNo = reader.GetString(0);
                    result.FullName = reader.GetString(1);
                    result.Email = reader.IsDBNull(2) ? "" : reader.GetString(2);
                    result.Mobile = reader.GetString(3);
                    result.TotalMarks = reader.GetInt32(6);
                    result.Percentage = reader.GetDecimal(7);
                    result.ResultStatus = reader.GetString(8);
                }

                result.Marks.Add(new StudentMarkViewModel
                {
                    SubjectName = reader.GetString(4),
                    Marks = reader.GetInt32(5)
                });
            }

            return result;
        }




        public List<StudentListViewModel> GetStudents(string? ackNo = null)
        {
            List<StudentListViewModel> list = new();

            string query = @"
        SELECT StudentId, AckNo, FullName, Mobile, District
        FROM Students
        WHERE (@AckNo IS NULL OR AckNo = @AckNo)
        ORDER BY CreatedAt DESC";

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@AckNo",
                string.IsNullOrWhiteSpace(ackNo) ? DBNull.Value : ackNo);

            con.Open();
            using SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new StudentListViewModel
                {
                    StudentId = reader.GetInt32(0),
                    AckNo = reader.GetString(1),
                    FullName = reader.GetString(2),
                    Mobile = reader.GetString(3),
                    District = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return list;
        }






        public Student? GetStudentById(int studentId)
        {
            string query = @"SELECT StudentId,AckNo,FullName,Email,Mobile,DateOfBirth,
                     AddressLine1, AddressLine2,District,Taluk,Village,Pincode
                     FROM Students WHERE StudentId=@StudentId";

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            con.Open();
            using var reader = cmd.ExecuteReader();

            if (!reader.Read()) return null;

            return new Student
            {
                StudentId = reader.GetInt32(0),
                AckNo = reader.GetString(1),
                FullName = reader.GetString(2),
                Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Mobile = reader.GetString(4),
                DateOfBirth = reader.GetDateTime(5),
                AddressLine1 = reader.GetString(6),
                AddressLine2 = reader.IsDBNull(7) ? null : reader.GetString(7),

                District = reader.GetString(8),
                Taluk = reader.GetString(9),
                Village = reader.GetString(10),
                Pincode = reader.GetString(11)
            };
        }



        public StudentEditViewModel? GetStudentWithMarks(int studentId)
        {
            var model = new StudentEditViewModel();

         
            model.Student = GetStudentById(studentId)!;

            if (model.Student == null) return null;

          
            string query = @"
        SELECT m.SubjectId, s.SubjectName, m.Marks
        FROM StudentMarks m
        JOIN Subjects s ON m.SubjectId = s.SubjectId
        WHERE m.StudentId = @StudentId";

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);
            cmd.Parameters.AddWithValue("@StudentId", studentId);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                model.Marks.Add(new StudentMarkViewModel
                {
                    SubjectId = reader.GetInt32(0),
                    SubjectName = reader.GetString(1),
                    Marks = reader.GetInt32(2)
                });
            }

            return model;
        }






        public List<StudentMarkViewModel> GetStudentMarks(int studentId)
        {
            List<StudentMarkViewModel> list = new();

            string query = @"
        SELECT sub.SubjectId, sub.SubjectName, m.Marks
        FROM StudentMarks m
        JOIN Subjects sub ON m.SubjectId = sub.SubjectId
        WHERE m.StudentId = @StudentId";

            using SqlConnection con = new SqlConnection(connectionString);
            using SqlCommand cmd = new SqlCommand(query, con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);

            con.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new StudentMarkViewModel
                {
                    SubjectId = reader.GetInt32(0),
                    SubjectName = reader.GetString(1),
                    Marks = reader.GetInt32(2)
                });
            }

            return list;
        }




        public void UpdateStudentWithMarks(StudentEditViewModel model)
        {
            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            int studentId = model.Student.StudentId;

          
            string studentSql = @"
UPDATE Students SET
    FullName=@FullName,
    Email=@Email,
    Mobile=@Mobile,
    DateOfBirth=@DOB,
   AddressLine1=@Addr1,
   AddressLine2=@Addr2,
    District=@District,
    Taluk=@Taluk,
    Village=@Village,
    Pincode=@Pincode
WHERE StudentId=@StudentId";

            using (var cmd = new SqlCommand(studentSql, con))
            {
                cmd.Parameters.AddWithValue("@StudentId", studentId);
                cmd.Parameters.AddWithValue("@FullName", model.Student.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Student.Email ?? "");
                cmd.Parameters.AddWithValue("@Mobile", model.Student.Mobile);
                cmd.Parameters.AddWithValue("@DOB", model.Student.DateOfBirth ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Addr1", model.Student.AddressLine1 ?? "");
                cmd.Parameters.AddWithValue("@Addr2", model.Student.AddressLine2 ?? "");
                cmd.Parameters.AddWithValue("@District", model.Student.District ?? "");
                cmd.Parameters.AddWithValue("@Taluk", model.Student.Taluk ?? "");
                cmd.Parameters.AddWithValue("@Village", model.Student.Village ?? "");
                cmd.Parameters.AddWithValue("@Pincode", model.Student.Pincode ?? "");

                cmd.ExecuteNonQuery();
            }

           
            foreach (var m in model.Marks)
            {
                string sql = @"
IF EXISTS (SELECT 1 FROM StudentMarks WHERE StudentId=@Sid AND SubjectId=@Sub)
    UPDATE StudentMarks SET Marks=@Marks
    WHERE StudentId=@Sid AND SubjectId=@Sub
ELSE
    INSERT INTO StudentMarks(StudentId, SubjectId, Marks)
    VALUES (@Sid,@Sub,@Marks)";

                using var cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@Sid", studentId);
                cmd.Parameters.AddWithValue("@Sub", m.SubjectId);
                cmd.Parameters.AddWithValue("@Marks", m.Marks);
                cmd.ExecuteNonQuery();
            }


            int total = model.Marks.Sum(x => x.Marks);
            decimal percentage = Math.Round(total * 100m / 500, 2);

            bool hasFail = model.Marks.Any(m => m.Marks < 35);
            string status = hasFail ? "Fail" : "Pass";

            string updateSql = @"
UPDATE StudentResults
SET TotalMarks = @Total,
    Percentage = @Percent,
    ResultStatus = @Status
WHERE StudentId = @StudentId";

            int rows;

            using (SqlCommand cmd = new SqlCommand(updateSql, con))
            {
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
                cmd.Parameters.Add("@Total", SqlDbType.Int).Value = total;
                cmd.Parameters.Add("@Percent", SqlDbType.Decimal).Value = percentage;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 10).Value = status;

                rows = cmd.ExecuteNonQuery();
            }

            
            if (rows == 0)
            {
                string insertSql = @"
INSERT INTO StudentResults
(StudentId, TotalMarks, Percentage, ResultStatus)
VALUES (@StudentId, @Total, @Percent, @Status)";

                using SqlCommand cmd = new SqlCommand(insertSql, con);
                cmd.Parameters.Add("@StudentId", SqlDbType.Int).Value = studentId;
                cmd.Parameters.Add("@Total", SqlDbType.Int).Value = total;
                cmd.Parameters.Add("@Percent", SqlDbType.Decimal).Value = percentage;
                cmd.Parameters.Add("@Status", SqlDbType.VarChar, 10).Value = status;

                cmd.ExecuteNonQuery();
            }


        }






        public void InsertStudent(Student student)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();

            string query = @"
INSERT INTO Students
(AckNo, Name, Email, Mobile, DateOfBirth, Address, District, Taluk, Village, Pincode)
VALUES
(@AckNo, @Name, @Email, @Mobile, @DOB, @Address, @District, @Taluk, @Village, @Pincode);
SELECT SCOPE_IDENTITY();";

            using var cmd = new SqlCommand(query, con);

            cmd.Parameters.Add("@AckNo", SqlDbType.VarChar, 20).Value = student.AckNo;
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = student.FullName;
            cmd.Parameters.Add("@Email", SqlDbType.VarChar, 100).Value = student.Email ?? string.Empty;
            cmd.Parameters.Add("@Mobile", SqlDbType.VarChar, 15).Value = student.Mobile;
            cmd.Parameters.Add("@DOB", SqlDbType.Date)
                .Value = student.DateOfBirth ?? (object)DBNull.Value;
            cmd.Parameters.Add("@Address", SqlDbType.VarChar, 200).Value = student.AddressLine1 ?? string.Empty;
            cmd.Parameters.Add("@District", SqlDbType.VarChar, 50).Value = student.District ?? string.Empty;
            cmd.Parameters.Add("@Taluk", SqlDbType.VarChar, 50).Value = student.Taluk ?? string.Empty;
            cmd.Parameters.Add("@Village", SqlDbType.VarChar, 50).Value = student.Village ?? string.Empty;
            cmd.Parameters.Add("@Pincode", SqlDbType.VarChar, 10).Value = student.Pincode ?? string.Empty;

            student.StudentId = Convert.ToInt32(cmd.ExecuteScalar());
        }



        public void InsertStudentResult(int studentId, StudentEntryModel model)
        {
            int total =
                model.Tamil +
                model.English +
                model.Maths +
                model.Science +
                model.Social;

            decimal percentage = Math.Round(total * 100m / 500, 2);

            string status = percentage >= 35 ? "Pass" : "Fail";

            using SqlConnection con = new SqlConnection(connectionString);
            con.Open();

            string sql = @"
        INSERT INTO StudentResults (StudentId, TotalMarks, Percentage, ResultStatus)
        VALUES (@StudentId, @Total, @Percentage, @Status)";

            using SqlCommand cmd = new SqlCommand(sql, con);

            cmd.Parameters.AddWithValue("@StudentId", studentId);
            cmd.Parameters.AddWithValue("@Total", total);
            cmd.Parameters.AddWithValue("@Percentage", percentage);
            cmd.Parameters.AddWithValue("@Status", status);

            cmd.ExecuteNonQuery();
        }


    }

    


    }
