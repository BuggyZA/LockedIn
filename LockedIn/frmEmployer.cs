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
using System.Runtime.InteropServices;
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
            MessageBox.Show(EmpID);
            //ComboBoxHelper.PopulateJobNames(ARJcmbJobName);



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

        private void tcEmployer_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            isPopulating = true;
            if (tcEmployer.SelectedIndex == 2)
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
            }
            else if(tcEmployer.SelectedIndex == 4)
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

        private void MJbtnDelete2_Click(object sender, EventArgs e)
        {
            //Delete existing job
            try
            {
                ComboBoxHelper.PopulateJobNames(MJcmbJobName);

                action = "DELETE";
                string sql = "SELECT * FROM JOB WHERE JobID = @JobID";
                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                    SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                    getIdcomm.Parameters.AddWithValue("@JobName", MJcmbJobName.SelectedItem.ToString());
                    int jobId = (int)getIdcomm.ExecuteScalar();
                    ComboBoxHelper.PopulateSkillNames(MJcmbSkillName);
                    ListboxHelper.PopulateCurrentJobSkills(MJlstSkillsNeeded, jobId);
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@JobID", jobId);

                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populating the form tools
                            MJcmbJobName.Text = reader["JobName"].ToString();
                            MJtxtJobDescription.Text = reader["JobDescription"].ToString();
                            MJtxtJobLocation.Text = reader["JobLocation"].ToString();
                            MJcalStartDate.SelectionStart = Convert.ToDateTime(reader["StartDate"]);
                            MJcalEndDate.SelectionStart = Convert.ToDateTime(reader["EndDate"]);
                            //.Value = Convert.ToDecimal(((TimeSpan)reader["DurationPerDay"]).TotalHours);
                            //cbJobCompleted.Checked = Convert.ToBoolean(reader["Completed"]);
                            //numericUpDown1.Value = Convert.ToDecimal(reader["PrisonersNeeded"]);


                        }
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void MJbtnEdit_Click(object sender, EventArgs e)
        {
            //Edit existing job
            try
            {
                //making panels visible
                ComboBoxHelper.PopulateJobNames(MJcmbJobName);

                action = "EDIT";

                string sql = "SELECT * FROM JOB WHERE JobID = @JobID";
                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                    SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                    getIdcomm.Parameters.AddWithValue("@JobName", MJcmbJobName.SelectedItem.ToString());
                    int jobId = (int)getIdcomm.ExecuteScalar();
                    ComboBoxHelper.PopulateSkillNames(MJcmbSkillName);
                    ListboxHelper.PopulateCurrentJobSkills(MJlstSkillsNeeded, jobId);
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@JobID", jobId);

                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populating the form tools
                            MJcmbJobName.Text = reader["JobName"].ToString();
                            MJtxtJobDescription.Text = reader["JobDescription"].ToString();
                            MJtxtJobLocation.Text = reader["JobLocation"].ToString();
                            MJcalStartDate.SelectionStart = Convert.ToDateTime(reader["StartDate"]);
                            MJcalEndDate.SelectionStart = Convert.ToDateTime(reader["EndDate"]);
                            //nudDuration.Value = Convert.ToDecimal(((TimeSpan)reader["DurationPerDay"]).TotalHours);
                            //cbJobCompleted.Checked = Convert.ToBoolean(reader["Completed"]);
                            //numericUpDown1.Value = Convert.ToDecimal(reader["PrisonersNeeded"]);


                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void MJbtnCreate_Click(object sender, EventArgs e)
        {
            //Add job button

            try
            {

                selectedJobID = -1;
                action = "ADD";

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void MJbtnSave_Click(object sender, EventArgs e)
        {
            //save button  

            try
            {
                //setting values into variables
                //string employer_name = ;
                string job_name = MJcmbJobName.SelectedItem.ToString();
                string job_description = MJtxtJobDescription.Text;
                string job_location = MJtxtJobLocation.Text;
                DateTime startDate = MJcalStartDate.SelectionStart;
                DateTime endDate = MJcalEndDate.SelectionStart;
                string skills = MJcmbSkillName.Text;
                //decimal duration = nudDuration.Value;
                //bool job_completed = cbJobCompleted.Checked;
                //decimal prisoners = numericUpDown1.Value;

                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    //add job
                    if (action == "ADD")
                    {
                        string sql = "INSERT INTO JOB(EmployerID,JobName,JobDescription,JobLocation,StartDate,EndDate,PrisonersNeeded,Completed,DurationPerDay) VALUES(@EmployerID,@JobName,@JobDescription,@JobLocation,@StartDate,@EndDate,@PrisonersNeeded,@Completed,@DurationPerDay)";

                        using (SqlCommand comm = new SqlCommand(sql, conn))
                        {
                            comm.CommandType = CommandType.StoredProcedure;

                            //cmd.Parameters.AddWithValue("@employerID", ); 
                            comm.Parameters.AddWithValue("@jobName", job_name);
                            comm.Parameters.AddWithValue("@jobDescription", job_description);
                            comm.Parameters.AddWithValue("@jobLocation", job_location);
                            comm.Parameters.AddWithValue("@startDate", startDate);
                            comm.Parameters.AddWithValue("@endDate", endDate);
                            //comm.Parameters.AddWithValue("@prisonersNeeded", prisoners);
                            //comm.Parameters.AddWithValue("@completed", job_completed);
                            //comm.Parameters.AddWithValue("@durationPerDay", duration);

                            comm.ExecuteNonQuery();
                        }

                        MessageBox.Show("Job added successfully.");
                    }
                    //edit job
                    else if (action == "EDIT")
                    {
                        string sql = "UPDATE JOB SET JobName= @jobName, JobDescription= @jobDescription, JobLocation= @jobLocation, StartDate = @startDate, EndDate = @endDate, PrisonersNeeded = @prisonersNeeded, Completed = @completed, DurationPerDay = @durationPerDay WHERE JobID = @jobID";

                        using (SqlCommand comm = new SqlCommand(sql, conn))
                        {
                            string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                            SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                            getIdcomm.Parameters.AddWithValue("@JobName", MJcmbJobName.Text);
                            int selectedJobId = (int)getIdcomm.ExecuteScalar();

                            comm.Parameters.AddWithValue("@jobID", selectedJobID);
                            comm.Parameters.AddWithValue("@jobName", MJcmbJobName.Text);
                            comm.Parameters.AddWithValue("@jobDescription", job_description);
                            comm.Parameters.AddWithValue("@jobLocation", job_location);
                            comm.Parameters.AddWithValue("@startDate", startDate);
                            comm.Parameters.AddWithValue("@endDate", endDate);
                            //comm.Parameters.AddWithValue("@prisonersNeeded", prisoners);
                            //comm.Parameters.AddWithValue("@completed", job_completed);
                            //comm.Parameters.AddWithValue("@durationPerDay", duration);


                            comm.ExecuteNonQuery();
                        }

                        MessageBox.Show("Job updated successfully.");
                    }
                    //delete job
                    else if (action == "DELETE")
                    {
                        string sql = "DELETE FROM JOB WHERE JobID = @jobID";

                        using (SqlCommand comm = new SqlCommand(sql, conn))
                        {
                            string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                            SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                            getIdcomm.Parameters.AddWithValue("@JobName", MJcmbJobName.Text);
                            int selectedJobID = (int)getIdcomm.ExecuteScalar();

                            comm.Parameters.AddWithValue("@jobID", selectedJobID);
                            comm.ExecuteNonQuery();
                        }

                        MessageBox.Show("Job deleted successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void MJbtnRemoveSkill_Click(object sender, EventArgs e)
        {
            //removing skill from job
            if (MJcmbSkillName.SelectedItem != null)
            {
                string skill = MJcmbSkillName.SelectedItem.ToString();
                MJlstSkillsNeeded.Items.Add(skill);
                MJcmbSkillName.SelectedItem = null;
            }
        }

        private void MJbtnAddSkill_Click(object sender, EventArgs e)
        {
            //adding skill to job
            if (MJlstSkillsNeeded.SelectedItem != null)
            {
                string skill = MJlstSkillsNeeded.SelectedItem.ToString();
                MJcmbJobName.Items.Add(skill);
                MJlstSkillsNeeded.SelectedItem = null;
            }
        }

        private void MJbtnDelete_Click(object sender, EventArgs e)
        {

        }
    }
}