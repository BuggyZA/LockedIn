using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Managers;
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
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static LockedIn.Data.SessionData;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LockedIn
{

    public partial class frmEmployer : Form
    {
        Job selectedJob = null;

        static String constr = DatabaseConfig.GetConStr();
        static String sql;
        bool isPopulating = false;
        String EmployerID;
        string EmpID;
        string action = "";// this for maintain jobs to hold say if data needs to be edited, addded or deleted
        int selectedJobID = -1; // This will hold the selected JobID for Edit/Delete
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

            tcEmployer.SelectedIndex = 0;
            ListboxHelper.PopulateUpcomingJobs(redDisplay, EmpID);

            ARJtxtPrisonerName.Text = "";
            ARJlsbCurrentSkills.DataSource = null;

            ARJcmbSelectApplication.SelectedIndex = -1;

            tcEmployer.Appearance = TabAppearance.FlatButtons;
            tcEmployer.ItemSize = new Size(0, 1);
            tcEmployer.SizeMode = TabSizeMode.Fixed;

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
                string sql;
                if(isApproved == true)
                {
                     sql = "UPDATE JOB SET PrisonersNeeded = PrisonersNeeded - 1 WHERE JobID = @JobID";
                }
                else
                {
                     sql = "DELETE FROM PRISONER_ON_JOB WHERE JobID = @JobID AND PrisonerID = @PrisonerID";
                }
                   

                using (SqlCommand comm = new SqlCommand(sql, conn))
                {

                    comm.Parameters.AddWithValue("@JobID", jobId);
                    comm.Parameters.AddWithValue("@PrisonerID", prisonerId);
                    comm.ExecuteNonQuery();

                    tcEmployer.SelectedIndex = 4;



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
            tcEmployer.SelectedIndex = 3;

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
            tcEmployer.SelectedIndex = 0;;
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
            if (txtEmpFName.Text == "" || txtEmpLName.Text == "" )
            {
                MessageBox.Show("Please enter all the details for the employer!");
                return;
            }
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
            else if (txtEmpNew.Text.Trim().Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

        private void frmEmployer_Shown(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;
        }

        


        private void button4_Click(object sender, EventArgs e)
        {
            frmLogIn login = new frmLogIn();
            login.logOut();
            this.Hide();
        }

        private void btnMEmpBack_Click_1(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;;
            PnlEmpInfo.Visible = false;
            pnlEmpPass.Visible = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 0;;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            tcEmployer.SelectedIndex = 2;
            PnlEmpInfo.Visible = true;
        }

        private void btnEmpDel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this prisoner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
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
            else
            {
                // User clicked No - cancel deletion
                MessageBox.Show("Delete cancelled.");
            }//delete employer

        }

        

        private void tcEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            isPopulating = true;
            if (tcEmployer.SelectedIndex == 0)
            {
                ListboxHelper.PopulateUpcomingJobs(redDisplay, EmpID);
            }
            else if (tcEmployer.SelectedIndex == 1)
            {
                // Populate employer name from EmployerID

                string empName = "";
                txtMJEmpName.Text = empName;
            }/*
            else if (tcEmployer.SelectedIndex == 2)
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();

                        String query = "SELECT JobID ,JobName FROM JOB WHERE EmployerID = @employerID";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@employerID", SessionData.LoggedInEmployerID);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataSet ds = new DataSet();

                        adapter.Fill(ds, "JOB");

                        cbxASJJobName.DataSource = ds.Tables["JOB"];
                        cbxASJJobName.DisplayMember = "JobName";
                        cbxASJJobName.ValueMember = "JobID";


                        cbxASJJobName.SelectedIndex = -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading skill names: " + ex.Message);
                }
            }*/
            else if (tcEmployer.SelectedIndex == 3)
            {
                ARJcmbJobName.SelectedIndex = -1;
                ARJcmbSelectApplication.SelectedIndex = -1;
                ARJtxtPrisonerName.Text = "";
                ARJlsbCurrentSkills.DataSource = null;
                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();
                        string sql = "SELECT JobID ,JobName FROM JOB WHERE EmployerID = @EmpID";
                        using (SqlCommand comm = new SqlCommand(sql, conn))
                        {
                            comm.Parameters.AddWithValue("@EmpID", Convert.ToInt32(EmpID));


                            SqlDataAdapter adapter = new SqlDataAdapter(comm);
                            DataSet ds = new DataSet();
                            adapter.Fill(ds, "JOB");

                            ARJcmbJobName.DataSource = ds.Tables["JOB"];
                            ARJcmbJobName.DisplayMember = "JobName";
                            ARJcmbJobName.ValueMember = "JobID";

                            ARJcmbJobName.SelectedIndex = -1;
                        }

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading skill names: " + ex.Message);
                }
            }
                isPopulating = false;
        }

        private void cbxASJJobName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating || cbxASJJobName.SelectedIndex == -1)
                return;
            ListboxHelper.PopulateCurrentJobSkills(lstASJCurrentSkillOfJob, (int)cbxASJJobName.SelectedValue);
            ListboxHelper.PopulatAvailableJobSkills(lstASJAllSkills, (int)cbxASJJobName.SelectedValue);
        }

        private void btnASJRemove_Click(object sender, EventArgs e)
        {
            JobSkillManager.RemoveJobSkill((int)cbxASJJobName.SelectedValue, ListboxHelper.GetSelectedSkillID(lstASJCurrentSkillOfJob));

            isPopulating = true;
            ListboxHelper.PopulatAvailableJobSkills(lstASJAllSkills, (int)cbxASJJobName.SelectedValue);
            ListboxHelper.PopulateCurrentJobSkills(lstASJCurrentSkillOfJob, (int)cbxASJJobName.SelectedValue);
            isPopulating = false;
        }
        private void btnASJAdd_Click(object sender, EventArgs e)
        {
            JobSkillManager.AddJobSkill((int)cbxASJJobName.SelectedValue, ListboxHelper.GetSelectedSkillID(lstASJAllSkills));

            isPopulating = true;
            ListboxHelper.PopulatAvailableJobSkills(lstASJAllSkills, (int)cbxASJJobName.SelectedValue);
            ListboxHelper.PopulateCurrentJobSkills(lstASJCurrentSkillOfJob, (int)cbxASJJobName.SelectedValue);
            isPopulating = false;
        }
       

        private void imgFMENewshow_MouseDown(object sender, MouseEventArgs e)
        {
            txtEmpNew.UseSystemPasswordChar = false;
            imgFMENewshow.Image = Properties.Resources.hide;
        }

        private void imgFMENewshow_MouseUp(object sender, MouseEventArgs e)
        {
            txtEmpNew.UseSystemPasswordChar = true;
            imgFMENewshow.Image = Properties.Resources.view;
        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void MJcmbJobName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbJobNAme_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbJobNAme.SelectedIndex != -1 && (action == "EDIT" || action == "DELETE"))
                {
                    selectedJobID = ComboBoxHelper.GetSelectedID(cmbJobNAme);

                    if (selectedJobID > 0)
                    {
                        // Load job details using JobManager
                        selectedJob = JobManager.LoadJob(selectedJobID);

                        if (selectedJob != null)
                        {
                            txtJobname.Text = selectedJob.JobName;
                            rtbJobDescription.Text = selectedJob.JobDescription;
                            rtbJobLocation.Text = selectedJob.JobLocation;
                            calStartDate.SelectionStart = selectedJob.StartDate;
                            calEndDate.SelectionStart = selectedJob.EndDate;
                            nudDuration.Value = selectedJob.DurationPerDay;
                            cbJobCompleted.Checked = selectedJob.Completed;
                            sbxPNeeded.Value = selectedJob.PrisonersNeeded;

                            if (action == "DELETE")
                            {
                                sbxPNeeded.Enabled = false;
                                pnlAdDate.Enabled = false;
                                ListboxHelper.PopulateCurrentJobSkills(lstMJCurrentSkills, selectedJobID);
                                ListboxHelper.PopulateAvailablePSkills(lstMJAllSkills, selectedJobID);
                                pnlAdSkills.Enabled = false;
                                txtJobname.ReadOnly = true;
                                rtbJobDescription.ReadOnly = true;
                                rtbJobLocation.ReadOnly = true;
                                Job newJob = new Job();

                            }
                            else if (action == "EDIT")
                            {
                                ListboxHelper.PopulateCurrentJobSkills(lstMJCurrentSkills, selectedJobID);
                                ListboxHelper.PopulateAvailablePSkills(lstMJAllSkills, selectedJobID);
                                txtJobname.ReadOnly = false;
                                rtbJobDescription.ReadOnly = false;
                                rtbJobLocation.ReadOnly = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading job details: " + ex.Message);
            }
        }

        private string GetEmployerNameById(int employerID)
        {
            try
            {
                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    string query = @"SELECT CONCAT(FirstName, ' ', LastName) AS FullName FROM EMPLOYER WHERE EmployerID = @EmployerID";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerID);
                        conn.Open();

                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return result.ToString();
                        }
                        else
                        {
                            MessageBox.Show($"Employer with ID {employerID} not found.");
                            return string.Empty;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving employer name: {ex.Message}");
                return string.Empty;
            }
        }

        private void txtJobname_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnMJAddJob_Click(object sender, EventArgs e)
        {
            // Add job button
            try
            {
                // Making panels visible
                btnMJSave.Text = "Add";

                pnlAdSkills.Enabled = false;

                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdFunctions.Visible = false;
                pnlAdSkills.Visible = true;
                cmbJobNAme.Visible = false;
                txtJobname.Visible = true;

                // Clear all fields for new job
                txtJobname.Clear();
                rtbJobDescription.Clear();
                rtbJobLocation.Clear();
                calStartDate.SelectionStart = DateTime.Today;
                calEndDate.SelectionStart = DateTime.Today.AddDays(1);
                nudDuration.Value = 8;
                cbJobCompleted.Checked = false;
                sbxPNeeded.Value = 1;

                selectedJobID = -1;
                action = "ADD";

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnMJEditJob_Click(object sender, EventArgs e)
        {
            // Edit existing job
            try
            {
                // Making panels visible

                btnMJSave.Text = "Save";

                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdFunctions.Visible = false;
                pnlAdSkills.Visible = true;
                cmbJobNAme.Visible = true;
                txtJobname.Visible = false;

                ComboBoxHelper.PopulateJobNames(cmbJobNAme);

                action = "EDIT";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnMJDeleteJob_Click(object sender, EventArgs e)
        {
            // Delete existing job
            try
            {
                // Making panels visible
                btnMJSave.Text = "Delete";

                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdFunctions.Visible = false;
                pnlAdSkills.Visible = true;
                cmbJobNAme.Visible = true;
                txtJobname.Visible = false;

                ComboBoxHelper.PopulateJobNames(cmbJobNAme);
                rtbJobDescription.Clear();
                rtbJobLocation.Clear();
                calStartDate.SelectionStart = DateTime.Today;
                calEndDate.SelectionStart = DateTime.Today.AddDays(1);
                nudDuration.Value = 8;
                cbJobCompleted.Checked = false;
                sbxPNeeded.Value = 1;

                action = "DELETE";
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
    }
    
}