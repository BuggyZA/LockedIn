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
        String EmployerID;
        string EmpID; 
        public frmEmployer()
        {
            InitializeComponent();
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



        private void frmEmployer_Load(object sender, EventArgs e)
        {
            EmpID = SessionData.LoggedInEmployerID.ToString();
            MessageBox.Show(EmpID);
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
                if (drv != null)
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
            tcEmployer.SelectedIndex = 0;
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

        private void btnHpMJ_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 1;

        }

        private void btnHpASTJ_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 2;
        }

        private void btnHpARJ_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 4;
        }

        private void imgProfile_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 3;

            PnlEmpInfo.Visible = true;

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            frmLogIn logInForm = new frmLogIn();
            logInForm.Show();
            this.Hide();
        }

        private void btnMJBack_Click(object sender, EventArgs e)
        {
            BackButton();
        }

        private void ASJbtnBack_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;
        }

        private void EPbtnBack_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;
        }

        private void btnEmpChangePass_Click(object sender, EventArgs e)
        {
            pnlEmpPass.Visible = true;
        }

        private void btnEmpCancel_Click(object sender, EventArgs e)
        {
            pnlEmpPass.Visible = false;
            txtEmpConfirm.Clear();
            txtEmpNew.Clear();
        }

        private void btnEmpEdit_Click(object sender, EventArgs e)
        {
            txtEmpFName.Enabled = true;
            txtEmpLName.Enabled = true;
            btnEmpSave.Visible = true;
            btnEmpEdit.Visible = false;
        }

        private void PnlEmpInfo_VisibleChanged(object sender, EventArgs e)
        {
            EmployerID = EmpID;


            //show panel to edit employer details with data auto filled in
            if (EmployerID == null)
            {
                MessageBox.Show("Invalid Employer number");
                return;
            }
            else
            {

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
                {
                    conn.Open();

                    string sql = "SELECT EmployerID, FirstName, LastName FROM EMPLOYER WHERE EmployerID = @EmployerID";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", EmployerID);

                        try
                        {

                            SqlDataReader reader = cmd.ExecuteReader();

                            if (reader.Read())
                            {
                                // Fill textboxes
                                txtEmpID.Text = reader["EmployerID"].ToString();
                                txtEmpFName.Text = reader["FirstName"].ToString();
                                txtEmpLName.Text = reader["LastName"].ToString();
                            }
                            else
                            {
                                MessageBox.Show("No employer found with that ID.");
                            }

                            reader.Close();

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error loading employer details: " + ex.Message);
                        }
                    }
                    conn.Close();

                }
            }
        }

        private void btnEmpSave_Click(object sender, EventArgs e)
        {
            //save edited employer details

            //Update employer details
            string empId = txtEmpID.Text;
            string fName = txtEmpFName.Text;
            string lName = txtEmpLName.Text;


            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                conn.Open();
                string sql = "UPDATE EMPLOYER SET LastName = @lName, FirstName = @fName WHERE EmployerID = @empId";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@lName", lName);
                    cmd.Parameters.AddWithValue("@fName", fName);
                    cmd.Parameters.AddWithValue("@empId", @empId);


                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Employer Updated successfully!");
                        txtEmpFName.Enabled = false;
                        txtEmpLName.Enabled = false;
                        btnEmpEdit.Visible = true;
                        btnEmpSave.Visible = false;
                        PnlEmpInfo.Visible = false;
                        PnlEmpInfo.Visible = true;

                    }
                }
            }
        }

        private void btnEmpSavePass_Click(object sender, EventArgs e)
        {
            string empId = txtEmpID.Text;
            string newPass = txtEmpNew.Text;
            string confirmPass = txtEmpConfirm.Text;

            if (newPass != confirmPass)
            {
                MessageBox.Show("passwords do not match!");
                return;
            }
            else
            {
                newPass = PasswordHasser.HashPassword(newPass);

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
                {
                    conn.Open();
                    string sql = "UPDATE EMPLOYER SET Password = @new WHERE EmployerID = @empId";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@new", newPass);
                        cmd.Parameters.AddWithValue("@empId", @empId);


                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Password Updated successfully!");

                            pnlEmpPass.Visible = false;
                            txtEmpNew.Clear();
                            txtEmpConfirm.Clear();

                        }


                    }
                }
            }
        }

        private void frmEmployer_Shown(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;
        }

        //START OF LOGOUT AND BACK BUTTON
        private void BackButton()
        {
            if (SessionData.SelectedUser == 1)
            {
               frmAdmin admin = new frmAdmin();
                admin.Show();
                this.Hide();
            }
            else if (SessionData.SelectedUser == 2)
            {
                frmPrisoner prisoner = new frmPrisoner();
                prisoner.Show();
                this.Hide();
            }
            else if (SessionData.SelectedUser == 3)
            {
                tcEmployer.SelectedIndex = 0;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmLogIn login = new frmLogIn();
            login.logOut();
            this.Hide();
        }

        private void btnMEmpBack_Click_1(object sender, EventArgs e)
        {
            BackButton();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BackButton();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BackButton();
            PnlEmpInfo.Visible = false;
            pnlEmpPass.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BackButton();
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 3;
            PnlEmpInfo.Visible = true;
        }

        private void btnEmpDel_Click(object sender, EventArgs e)
        {
            string employerId = EmpID;
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                conn.Open();

                string sql1 = "SELECT JobID FROM JOB WHERE EmployerID = @ID";
                using (SqlCommand cmd1 = new SqlCommand(sql1, conn))
                {
                    cmd1.Parameters.AddWithValue("@ID", employerId);
                    SqlDataReader reader = cmd1.ExecuteReader();
                    List<int> jobIds = new List<int>();
                    while (reader.Read())
                    {
                        jobIds.Add(reader.GetInt32(0));
                    }
                    reader.Close();
                    // Delete associated records in PRISONER_ON_JOB for each job
                    foreach (int jobId in jobIds)
                    {
                        string sql2 = "DELETE FROM PRISONER_ON_JOB WHERE JobID = @JobID";
                        using (SqlCommand cmd2 = new SqlCommand(sql2, conn))
                        {
                            cmd2.Parameters.AddWithValue("@JobID", jobId);
                            cmd2.ExecuteNonQuery();
                        }

                        string sql3 = "DELETE FROM SKILL_OF_JOB WHERE JobID = @JobID";
                        using (SqlCommand cmd3 = new SqlCommand(sql3, conn))
                        {
                            cmd3.Parameters.AddWithValue("@JobID", jobId);
                            cmd3.ExecuteNonQuery();
                        }
                    }
                }
                string sql4 = "DELETE FROM JOB Where EmployerID = @ID";
                using (SqlCommand cmd4 = new SqlCommand(sql4, conn))
                {
                    cmd4.Parameters.AddWithValue("@ID", employerId);
                    cmd4.ExecuteNonQuery();
                }
                string sql5 = "DELETE FROM EMPLOYER WHERE EmployerID = @ID";
                using (SqlCommand cmd5 = new SqlCommand(sql5, conn))
                {
                    cmd5.Parameters.AddWithValue("@ID", employerId);

                    int rowsAffected = cmd5.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Employer deleted successfully!");
                    else
                        MessageBox.Show("No matching employer found!");
                }
                conn.Close();
                frmLogIn login = new frmLogIn();
                login.logOut();
                this.Hide();
            }
        }

        private void panel6_VisibleChanged(object sender, EventArgs e)
        {
            ListboxHelper.PopulateUpcomingJobs(redDisplay, EmpID);
        }
    }
}