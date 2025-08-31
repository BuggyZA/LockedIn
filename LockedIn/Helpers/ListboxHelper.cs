using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LockedIn.Helpers
{
    public static class ListboxHelper
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlDataAdapter adapter;
        static String query;

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

        public static void PopulateUpcomingJobs(RichTextBox richTextBox, string EmpID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // SQL query to get upcoming or incomplete jobs for the logged-in employer
                    string sql = @"SELECT JobID, JobName, StartDate, EndDate, JobLocation, PrisonersNeeded, 
                          CASE WHEN StartDate > GETDATE() THEN 'Upcoming' 
                               WHEN Completed = 0 THEN 'In Progress' 
                               ELSE 'Unknown' END as Status
                          FROM JOB 
                          WHERE EmployerID = @EmployerID 
                            AND (StartDate > GETDATE() OR Completed = 0)
                          ORDER BY StartDate";

                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.Add("@EmployerID", SqlDbType.Int).Value = Convert.ToInt32(EmpID);

                        SqlDataAdapter adapter = new SqlDataAdapter(comm);
                        DataTable jobsTable = new DataTable();
                        adapter.Fill(jobsTable);

                        // Clear the RichTextBox first
                        richTextBox.Clear();

                        if (jobsTable.Rows.Count > 0)
                        {
                            // Create formatted text display
                            FormatRichTextBoxDisplay(richTextBox, jobsTable);
                        }
                        else
                        {
                            // Add a message if no jobs found
                            richTextBox.Text = "No upcoming or incomplete jobs found.";
                            richTextBox.ForeColor = Color.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading jobs: " + ex.Message);
            }
        }


        public static void PopulatePrisonersUpcomingJobs(RichTextBox richTextBox)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // SQL query to get upcoming or incomplete jobs for the logged-in employer
                    string sql = @"SELECT JobID, JobName, StartDate, EndDate, JobLocation, PrisonersNeeded, 
                          CASE WHEN StartDate > GETDATE() THEN 'Upcoming' 
                               WHEN Completed = 0 THEN 'In Progress' 
                               ELSE 'Unknown' END as Status
                          FROM JOB 
                          WHERE (StartDate > GETDATE() OR Completed = 0)
                          ORDER BY StartDate";

                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(comm);
                        DataTable jobsTable = new DataTable();
                        adapter.Fill(jobsTable);

                        // Clear the RichTextBox first
                        richTextBox.Clear();

                        if (jobsTable.Rows.Count > 0)
                        {
                            // Create formatted text display
                            FormatRichTextBoxDisplay(richTextBox, jobsTable);
                        }
                        else
                        {
                            // Add a message if no jobs found
                            richTextBox.Text = "No upcoming or incomplete jobs found.";
                            richTextBox.ForeColor = Color.Gray;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading jobs: " + ex.Message);
            }
        }

        // Helper method to format the RichTextBox display
        private static void FormatRichTextBoxDisplay(RichTextBox richTextBox, DataTable jobsTable)
        {
            StringBuilder content = new StringBuilder();

            // Add header
            content.AppendLine("UPCOMING & IN-PROGRESS JOBS");
            content.AppendLine(new string('=', 50));
            content.AppendLine();

            foreach (DataRow row in jobsTable.Rows)
            {
                string jobName = row["JobName"].ToString();
                DateTime startDate = Convert.ToDateTime(row["StartDate"]);
                DateTime? endDate = row["EndDate"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["EndDate"]) : null;
                string status = row["Status"].ToString();
                string location = row["JobLocation"].ToString();
                int prisonersNeeded = Convert.ToInt32(row["PrisonersNeeded"]);

                // Build job information
                content.AppendLine($"Job: {jobName}");
                content.AppendLine($"Status: {status}");
                content.AppendLine($"Start Date: {startDate.ToShortDateString()}");
                if (endDate.HasValue)
                    content.AppendLine($"End Date: {endDate.Value.ToShortDateString()}");
                content.AppendLine($"Location: {location}");
                content.AppendLine($"Prisoners Needed: {prisonersNeeded}");
                content.AppendLine(new string('-', 30));
                content.AppendLine();
            }

            // Set the content
            richTextBox.Text = content.ToString();

           
        }

        // Helper method to apply rich text formatting
        



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
                    adapter.Fill(ds, "SKILLS_OF_JOB");

                    listBox.DataSource = ds.Tables["SKILLS_OF_JOB"];
                    listBox.DisplayMember = "SkillName";
                    listBox.ValueMember = "SkillID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job skills: " + ex.Message);
            }
        }

        public static void PopulatAvailableJobSkills(ListBox listBox, int jobID)     //Added for Assign Skill to job
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    query = @"SELECT SkillID, SkillName FROM SKILL WHERE SkillID NOT IN (SELECT SkillID FROM SKILL_OF_JOB
                         WHERE JobID = @JobID) ORDER BY SkillName";
                    

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@JobID", jobID);

                    adapter = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "SKILLS_OF_JOB");

                    listBox.DataSource = ds.Tables["SKILLS_OF_JOB"];
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
