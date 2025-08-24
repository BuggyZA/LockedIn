using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LockedIn.Helpers
{
    public static class ListboxHelper
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlDataAdapter adapter;
        static String query;
        static DataSet ds;

        //Methods to fill listBoxes with skills of Prisoners
        public static void PopulateAllPSkills(ListBox listBox)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = "SELECT SkillID, SkillName FROM SKILL";
                    adapter = new SqlDataAdapter(query, conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "SKILL");
                    listBox.DataSource = ds.Tables["SKILL"];
                    listBox.DisplayMember = "SkillName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading skills: " + ex.Message);
            }
        }

        public static void PopulatePrisonerSkills(ListBox listBox, int prisonerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = @"SELECT SkillID, SkillName FROM SKILL WHERE SkillID IN (SELECT SkillID FROM SKILL_OF_PRISONER 
                         WHERE PrisonerID = @PrisonerID) ORDER BY SkillName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PrisonerID", prisonerID);

                    adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "SKILLS_OF_PRISONER");

                    listBox.DataSource = ds.Tables["SKILLS_OF_PRISONER"];
                    listBox.DisplayMember = "SkillName";
                    listBox.ValueMember = "SkillID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading prisoner skills: " + ex.Message);
            }
        }
        public static void PopulateCurrentJobSkills(ListBox listBox, int jobID)     //Added for Assign Skill to job
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = @"SELECT SkillID, SkillName FROM SKILL WHERE SkillID IN (SELECT SkillID FROM SKILL_OF_JOB
                         WHERE JobID = @JobID) ORDER BY SkillName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JobID", jobID);

                    adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "SKILL_OF_JOB");

                    listBox.DataSource = ds.Tables["SKILL_OF_JOB"];
                    listBox.DisplayMember = "SkillName";
                    listBox.ValueMember = "SkillID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job skills: " + ex.Message);
            }
        }

        public static void PopulateAvailablePSkills(ListBox listBox, int prisonerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = @"SELECT SkillID, SkillName FROM SKILL WHERE SkillID NOT IN (SELECT SkillID FROM SKILL_OF_PRISONER 
                         WHERE PrisonerID = @PrisonerID) ORDER BY SkillName";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@PrisonerID", prisonerID);

                    adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "AVAILABLE_SKILLS");

                    listBox.DataSource = ds.Tables["AVAILABLE_SKILLS"];
                    listBox.DisplayMember = "SkillName";
                    listBox.ValueMember = "SkillID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading available skills: " + ex.Message);
            }
        }

        //Method to retrieve the ID of the selected skill
        public static int GetSelectedSkillID(ListBox listBox)
        {
            if (listBox.SelectedItem is DataRowView selectedRow)
            {
                object value = selectedRow[listBox.ValueMember];
                if (value == null || value == DBNull.Value)
                    return -1;
                return Convert.ToInt32(value);
            }
            // Fallback
            return listBox.SelectedValue != null ? Convert.ToInt32(listBox.SelectedValue) : -1;
        }
    }
}
