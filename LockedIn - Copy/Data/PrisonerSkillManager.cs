using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LockedIn.Data
{
    public static class PrisonerSkillManager
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlCommand cmd;
        static String query;

        public static void AddPrisonerSkill(int prisonerID, int skillID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = "INSERT INTO SKILL_OF_PRISONER (PrisonerID, SkillID) VALUES (@PrisonerID, @SkillID)";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PrisonerID", prisonerID);
                    cmd.Parameters.AddWithValue("@SkillID", skillID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding skill to prisoner: " + ex.Message);
            }
        }

        public static void RemovePrisonerSkill(int prisonerID, int skillID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = "DELETE FROM SKILL_OF_PRISONER WHERE PrisonerID = @PrisonerID AND SkillID = @SkillID";

                    cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PrisonerID", prisonerID);
                    cmd.Parameters.AddWithValue("@SkillID", skillID);

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error removing skill from prisoner: " + ex.Message);
            }
        }
    }
}