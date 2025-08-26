using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LockedIn
{
    
    public partial class frmEmployer : Form
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlDataAdapter adap;
        static String sql;
        static DataSet ds;
        public frmEmployer()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabCont_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void ARJcmbJobName_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            ARJcmbSelectApplication.DataSource = null;
            ARJtxtPrisonerName.Text = "";

           
            if (ARJcmbJobName.SelectedIndex == -1 || ARJcmbJobName.SelectedItem == null)
                return;

           
            int selectedJobId;
            if (ARJcmbJobName.SelectedItem is DataRowView selectedJob)
            {
                selectedJobId = Convert.ToInt32(selectedJob["JobID"]);
            }
            else
            {
                return;
            }

           
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string sql = "SELECT p.PrisonerID, p.PrisonerNum FROM PRISONER p INNER JOIN PRISONER_ON_JOB poj ON p.PrisonerID = poj.PrisonerID WHERE poj.JobID = @JobID";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@JobID", selectedJobId);

                    SqlDataAdapter adapter = new SqlDataAdapter(comm);
                    DataTable filteredPrisoners = new DataTable();
                    adapter.Fill(filteredPrisoners);

                    
                    ARJcmbSelectApplication.DataSource = filteredPrisoners;
                    ARJcmbSelectApplication.ValueMember = "PrisonerID";
                    ARJcmbSelectApplication.DisplayMember = "PrisonerNum";
                    ARJcmbSelectApplication.SelectedIndex = -1;

                  
                }

                conn.Close();
            }
        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void frmEmployer_Load(object sender, EventArgs e)
        {

            ComboBoxHelper.PopulateJobNames(ARJcmbJobName);
             


            ARJtxtPrisonerName.Text = "";
            ARJlsbCurrentSkills.DataSource = null;

            
            ARJcmbSelectApplication.SelectedIndex = -1;

        }

        private void PopulateSkillsHelper(ListBox ARJlsbCurrentSkills, int prisonerID)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"SELECT SkillId, SkillName FROM SKILL WHERE SkillId IN (SELECT SkillId FROM SKILL_OF_PRISONER WHERE PrisonerID = @PrisonerID) ORDER BY SkillName";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@PrisonerID", prisonerID);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(comm);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "SKILL_OF_PRISONER");     
                        ARJlsbCurrentSkills.DataSource = ds.Tables["SKILL_OF_PRISONER"];        
                        ARJlsbCurrentSkills.ValueMember = "SkillId";
                        ARJlsbCurrentSkills.DisplayMember = "SkillName";
                        ListboxHelper.PopulatePrisonerSkills(ARJlsbCurrentSkills, prisonerID);

                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }   

        private void ARJcmbSelectApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            ARJtxtPrisonerName.Text = "";
            ARJlsbCurrentSkills.DataSource = null;

            if (ARJcmbJobName.SelectedIndex == -1 || ARJcmbJobName.SelectedItem == null)
                return;
            

            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                int prisonerId;
                string sql = "SELECT FirstName FROM PRISONER WHERE PrisonerID = @PrisonerID";
                DataRowView drv = (DataRowView)ARJcmbSelectApplication.SelectedItem;
                if(drv != null)
                {
                    prisonerId = Convert.ToInt32(drv["PrisonerID"]);
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {

                        comm.Parameters.AddWithValue("@PrisonerID", prisonerId);
                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            ARJtxtPrisonerName.Text = reader["FirstName"].ToString();
                            ListboxHelper.PopulatePrisonerSkills(ARJlsbCurrentSkills, prisonerId);
                            PopulateSkillsHelper(ARJlsbCurrentSkills, prisonerId);
                        }
                        else
                        {
                            MessageBox.Show("No prisoner found with the selected ID.");
                        }

                    }
                }
                

                


                conn.Close();
            }
        }


        private void UpdateApplicationStatus(int jobId, int prisonerId, bool isApproved)
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                string sql = "UPDATE PRISONER_ON_JOB SET [Accepted/Rejeced] = @Status WHERE JobID = @JobID AND PrisonerID = @PrisonerID";

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {
                    comm.Parameters.AddWithValue("@Status", isApproved ? 1 : 0);
                    comm.Parameters.AddWithValue("@JobID", jobId);
                    comm.Parameters.AddWithValue("@PrisonerID", prisonerId);

                    int rowsAffected = comm.ExecuteNonQuery();

                    string statusText = isApproved ? "approved" : "rejected";

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Application " + statusText + " successfully!");
                        
                    }
                    else
                    {
                        MessageBox.Show("No application found to " + statusText.Replace("ed", "") + ".");
                    }
                }
            }
        }

        private void ARJbtnBack_Click(object sender, EventArgs e)
        {
            tabCont.SelectedIndex = 0;
        }

        private void ARJlsbCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ARJpbl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnHPARJ_Click(object sender, EventArgs e)
        {
            
        }

        private void ARJbtnApprove_Click(object sender, EventArgs e)
        {
            int jobId = Convert.ToInt32(ARJcmbJobName.SelectedValue);
            int prisonerId = Convert.ToInt32(ARJcmbSelectApplication.SelectedValue);
            UpdateApplicationStatus(jobId, prisonerId, true);
        }

        private void ARJbtnReject_Click(object sender, EventArgs e)
        {
            int jobId = Convert.ToInt32(ARJcmbJobName.SelectedValue);
            int prisonerId = Convert.ToInt32(ARJcmbSelectApplication.SelectedValue);
            UpdateApplicationStatus(jobId, prisonerId, false);
        }
    }

        



        
}
