using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace LockedIn.Data
{
    public static class JobSkillManager
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlCommand cmd;
        static String query;

        public static void AddJobSkill(int JobID, int skillID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = "INSERT INTO SKILL_OF_JOB (JobID, SkillID) VALUES (@JobID, @SkillID)";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JobID", JobID);
                    cmd.Parameters.AddWithValue("@SkillID", skillID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding skill to job: " + ex.Message);
            }
        }

        public static void RemoveJobSkill(int JobID, int skillID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = "DELETE FROM SKILL_OF_JOB WHERE JobID = @JobID AND SkillID = @SkillID";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JobID", JobID);
                    cmd.Parameters.AddWithValue("@SkillID", skillID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing skill from job: " + ex.Message);
            }
        }
    }
}
