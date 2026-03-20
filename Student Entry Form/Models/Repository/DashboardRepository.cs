using Microsoft.Data.SqlClient;

namespace Student_Entry_Form.Models.Repository
{

    public class DashboardRepository
    {
        private readonly string? _connectionString;
        public DashboardRepository(IConfiguration config)
        {
            _connectionString =config.GetConnectionString("DbConnection");
        }
        
        public DashboardStatsViewModel GetDashboardStats()
{
    string query = @"
        SELECT
            COUNT(*) AS TotalStudents,
            SUM(CASE WHEN ResultStatus = 'Pass' THEN 1 ELSE 0 END) AS Passed,
            SUM(CASE WHEN ResultStatus = 'Fail' THEN 1 ELSE 0 END) AS Failed,
            AVG(CAST(Percentage AS DECIMAL(5,2))) AS AvgPercentage
        FROM StudentResults";

    using SqlConnection con = new SqlConnection(_connectionString);
    using SqlCommand cmd = new SqlCommand(query, con);

    con.Open();
    using SqlDataReader reader = cmd.ExecuteReader();

    DashboardStatsViewModel stats = new DashboardStatsViewModel();

    if (reader.Read())
    {
        stats.TotalStudents = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
        stats.Passed = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
        stats.Failed = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
        stats.AveragePerformance = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);
    }

    return stats;
}
    }
}
