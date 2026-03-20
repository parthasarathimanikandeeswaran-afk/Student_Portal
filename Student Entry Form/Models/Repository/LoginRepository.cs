using Microsoft.Data.SqlClient;
using System.Data;

namespace Student_Entry_Form.Models.Repository
{
    public class LoginRepository
    {
        private readonly string connectionString;
        public LoginRepository(IConfiguration config) {
            connectionString = config.GetConnectionString("DbConnection");
        }

        public bool ValidAdmin(AdminLoginViewModel admin)
        {
            if (admin == null ||
                string.IsNullOrEmpty(admin.Username) ||
                string.IsNullOrEmpty(admin.Password))
                return false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(1) FROM users WHERE Username=@Username AND PasswordHash=@Password";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = admin.Username;
                    cmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = admin.Password;

                    connection.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }



        public StudentResultViewModel? GetStudentResult(string ackNo, DateTime dob)
        {
            using var con = new SqlConnection(connectionString);
            con.Open();

            string sql = @"
SELECT 
    s.FullName,
    s.AckNo,
    s.Mobile,
    r.TotalMarks,
    r.Percentage,
    r.ResultStatus
FROM Students s
JOIN StudentResults r ON s.StudentId = r.StudentId
WHERE s.AckNo = @AckNo
AND s.DateOfBirth = @Dob";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.Add("@AckNo", SqlDbType.VarChar, 20).Value = ackNo;
            cmd.Parameters.Add("@Dob", SqlDbType.Date).Value = dob;

            var dr = cmd.ExecuteReader();
            if (!dr.Read()) return null;

            return new StudentResultViewModel
            {
                FullName = dr["FullName"].ToString(),
                AckNo = dr["AckNo"].ToString(),
                Mobile = dr["Mobile"].ToString(),
                TotalMarks = Convert.ToInt32(dr["TotalMarks"]),
                Percentage = Convert.ToDecimal(dr["Percentage"]),
                ResultStatus = dr["ResultStatus"].ToString()
            };
        }




        public bool ValidStudent(StudentLoginViewModel stu)
        {
            const string query = @"
        SELECT COUNT(1)
        FROM Students
        WHERE AckNo = @AckNo
          AND DateOfBirth = @DOB";

            using (SqlConnection connection = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, connection))
            {
                
                cmd.Parameters.Add("@AckNo", SqlDbType.VarChar, 20)
                   .Value = stu.AckNo.Trim().ToUpper();

                cmd.Parameters.Add("@DOB", SqlDbType.Date)
                   .Value = stu.DateOfBirth;

                connection.Open();

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }



        }

}
