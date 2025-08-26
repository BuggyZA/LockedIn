using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Models;
using LockedIn.Managers;
using System.Security.Cryptography;
namespace LockedIn
{
    public partial class frmAdmin : Form
    {
        string password = "";
        bool isAddingPrisoner = false;
        bool isDeletingPrisoner = false;
        bool isupdatingPrisoner = false;


        int skillID;
        int prsnrID;
        int selectedSkillID; // this is for editing and deleting tracking of skills

        /*This flag checks if the comboboxes are currently busy populating (Because when they are, their indices change 
         * and they invoke the various indexchanged events and this causes several errors)*/
        bool isPopulating = false;
        public frmAdmin()
        {
            InitializeComponent();
            // Setting the two panels to edit and add skills to false 
            panel19.Visible = false;
            panel20.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 3;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isPopulating = true;
            //Popuating combo boxes.
            if (tcPrisoner.SelectedIndex == 4)
            {
                ComboBoxHelper.PopulateJobNames(cbxJobName);
            }
            else if (tcPrisoner.SelectedIndex == 6) //Maintain skills page
            {
                ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
            }
            //Maintain Prisoner Page
            else if (tcPrisoner.SelectedIndex == 8)
            {
                ComboBoxHelper.PopulatePrisonerNumbers(cmbMPPrisonerNo);
                pnlPInfo.Visible = false;
                pnlMPSkills.Visible = false;
                cmbMPPrisonerNo.Visible = false;
                pnlMPChoices.Visible = true;
                pnlModifyPPassword.Visible = false;
            }
            else if (tcPrisoner.SelectedIndex == 2)
            {
                ComboBoxHelper.PopulateJobNames(cmbJobID);
                ComboBoxHelper.PopulatePrisonerNumbers(cmbPrisonerID);
                calDateCompleted.MaxDate = DateTime.Today;
            }
            else if(tcPrisoner.SelectedIndex == 5)
            {
                ComboBoxHelper.PopulateJobNames(cbxASJJobName);

            }


                isPopulating = false;
        }


        private void cmbMPPrisonerNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            if (isDeletingPrisoner)
            {
                pnlMPSkills.Enabled = false;
                pnlPInfo.Enabled = false;
                cmbMPPrisonerNo.Enabled = true;
            }


            if (cmbMPPrisonerNo.SelectedIndex != -1)
            {
                prsnrID = ComboBoxHelper.GetSelectedID(cmbMPPrisonerNo);

                isPopulating = true;
                ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, prsnrID);
                ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, prsnrID);
                isPopulating = false;

                Prisoner thisPrisoner = PrisonerManager.LoadPrisoner(prsnrID);
                txtPNo.Text = thisPrisoner.PrisonerNum;
                txtFirstName.Text = thisPrisoner.FirstName;
                txtLastName.Text = thisPrisoner.LastName;
                txtID.Text = thisPrisoner.SA_ID;
                ckbCleared.Checked = thisPrisoner.ClearedToWork == true ? true : false;
            }
            else
            {
                MessageBox.Show("Please select a Prisoner Number");
            }
        }

        private void lstMPAllSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstMPAllSkills);
        }

        private void btnMPAdd_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.AddPrisonerSkill(prsnrID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, prsnrID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, prsnrID);
            isPopulating = false;
        }

        private void btnMPRemove_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.RemovePrisonerSkill(prsnrID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, prsnrID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, prsnrID);
            isPopulating = false;
        }

        private void lstMPCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstMPCurrentSkills);
        }

        private void btnMPNext_Click(object sender, EventArgs e)
        {

        }

        //Edit Prisoner
        private void button46_Click(object sender, EventArgs e)
        {
            isupdatingPrisoner = true;
            
            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            cmbMPPrisonerNo.Visible = true;
            txtPNo.Visible = false;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = true;
        }

        //Delete Prisoner
        private void button47_Click(object sender, EventArgs e)
        {
            isDeletingPrisoner = true;
            
            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            cmbMPPrisonerNo.Visible = true;
            txtPNo.Visible = false;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = true;
        }

        //Add Prisoner
        private void button53_Click(object sender, EventArgs e)
        {
            isAddingPrisoner = true;

            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            cmbMPPrisonerNo.Visible = false;
            txtPNo.Visible = true;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = false;

            isPopulating = true;
            ListboxHelper.PopulateAllPSkills(lstMPAllSkills);
            isPopulating = false;

            pnlMPSkills.Enabled = false;
        }

        private void btnMPSave_Click(object sender, EventArgs e)
        {
            if (isAddingPrisoner && pnlMPSkills.Enabled ==  false)
            {
                Prisoner thisPrisoner = new Prisoner();

                thisPrisoner.PrisonerNum = Prisoner.GeneratePrisonerNum();
                thisPrisoner.LastName = txtLastName.Text.Trim();
                thisPrisoner.FirstName = txtFirstName.Text.Trim();
                thisPrisoner.SA_ID = txtID.Text.Trim();
                thisPrisoner.Password = password;
                thisPrisoner.ClearedToWork = ckbCleared.Checked;

                if (thisPrisoner.ValidateNewPrisoner())
                {
                    prsnrID =  PrisonerManager.AddPrisoner(thisPrisoner);
                    MessageBox.Show(prsnrID.ToString());
                    MessageBox.Show("Prisoner "+thisPrisoner.PrisonerNum+" added successfully! Now you can manage their skills.");
                }
                else
                {
                    return;
                }

                pnlMPSkills.Enabled = true;
                return;
            }
            pnlPInfo.Visible = false;
            pnlMPSkills.Visible = false;
            cmbMPPrisonerNo.Visible = false;
            pnlMPChoices.Visible = true;

            isAddingPrisoner = false;
            isDeletingPrisoner = false;
            isupdatingPrisoner = false;
            password = "";

        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            pnlModifyPPassword.Visible = true;
            pnlPInfo.Visible = false;
        }

        private void btnSavePassword_Click(object sender, EventArgs e)
        {
            if (isAddingPrisoner)
            {                
                if (txtNewPassword.Text == txtConfirmPassword.Text && txtNewPassword.Text != "")
                {
                    if (txtNewPassword.Text.Trim().Length < 6)
                    {
                        MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    MessageBox.Show("Saving new password");
                    password = txtNewPassword.Text;
                }
                else
                {
                    MessageBox.Show("Please enter your password in both open spaces","Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;

           
        }

        private void btnCompleteJobs_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 2;
        }

        private void btnMaintainSkills_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 6;
        }

        private void btnMSEditExisSkil_Click(object sender, EventArgs e)
        {
            // Displayng the relevant panel to edit skills
            panel19.Visible = true;

            // preventing a skill to be automatically selected.
            isPopulating = true;
            ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
            cbxMSSkillName.SelectedIndex = -1;  // ensuring no skill is selected
            isPopulating = false;

            // keeping the fields clear until the user selects a skill
            txtMSSkillName.Clear();
            txtMSSkillDesc.Clear();
            selectedSkillID = -1;
        }


        private void cbxMSSkillName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating || cbxMSSkillName.SelectedIndex == -1) return;

            selectedSkillID = ComboBoxHelper.GetSelectedID(cbxMSSkillName);

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                string query = "SELECT SkillName, SkillDescription FROM SKILL WHERE SkillID = @SkillID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SkillId", selectedSkillID);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        txtMSSkillName.Text = reader["SkillName"].ToString();
                        txtMSSkillDesc.Text = reader["SkillDescription"].ToString();
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error receiving skill: " + ex.Message);
                }
            }
            panel20.Visible = true;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            string skillName = txtMSSkillName.Text.Trim();
            string skillDesc = txtMSSkillDesc.Text.Trim();

            if (string.IsNullOrEmpty(skillName))
            {
                MessageBox.Show("Please enter a name for the skill!");
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                SqlCommand cmd;

                if (selectedSkillID == -1) // new skills 
                {
                    cmd = new SqlCommand("INSERT INTO SKILL (SkillName, SkillDescription) OUTPUT INSERTED.SkillID VALUES (@Name, @Desc)", conn);
                    cmd.Parameters.AddWithValue("@Name", skillName);
                    cmd.Parameters.AddWithValue("@Desc", skillDesc);

                    try
                    {
                        conn.Open();
                        int newSkillID = (int)cmd.ExecuteScalar();

                        // optionally linking the skill to the prisoner.
                        int defaultPrisonerID = prsnrID != 0 ? prsnrID : 1;
                        SqlCommand linkCmd = new SqlCommand("INSERT INTO SKILL_OF_PRISONER (PrisonerID, SkillID) VALUES (@PrisonerID, @SkillID)", conn);
                        linkCmd.Parameters.AddWithValue("@PrisonerID", defaultPrisonerID);
                        linkCmd.Parameters.AddWithValue("@SkillID", newSkillID);
                        linkCmd.ExecuteNonQuery();

                        MessageBox.Show("Skill added successfully!");

                        // ensuring the textboxes are clear after a skill has been added.
                        txtMSSkillName.Clear();
                        txtMSSkillDesc.Clear();
                        selectedSkillID = -1;

                        // preventing the auto trigerring
                        isPopulating = true;
                        ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
                        cbxMSSkillName.SelectedIndex = -1;
                        isPopulating = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error saving skill: " + ex.Message);
                    }
                }
                else // updating the existing skill.
                {
                    cmd = new SqlCommand("UPDATE SKILL SET SkillName = @Name, SkillDescription = @Desc WHERE SkillID = @SkillID", conn);
                    cmd.Parameters.AddWithValue("@Name", skillName);
                    cmd.Parameters.AddWithValue("@Desc", skillDesc);
                    cmd.Parameters.AddWithValue("@SkillID", selectedSkillID);

                    try
                    {
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Skill updated successfully!");

                        // clearing the textboxes after the skill has been updated.
                        txtMSSkillName.Clear();
                        txtMSSkillDesc.Clear();
                        selectedSkillID = -1;

                        isPopulating = true;
                        ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
                        cbxMSSkillName.SelectedIndex = -1;
                        isPopulating = false;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating skill: " + ex.Message);
                    }
                }
            }
        }


        private void btnMSDelExisSkill_Click(object sender, EventArgs e)
        {
            // Show panels so the user can select a skill
            panel19.Visible = true;
            panel20.Visible = true;

            // If no skill is selected yet... population takes place
            if (cbxMSSkillName.SelectedIndex == -1 || selectedSkillID == -1)
            {
                isPopulating = true;
                ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
                cbxMSSkillName.SelectedIndex = -1; // ensuring that there is no skill auto selected.
                isPopulating = false;

                // Clearing textboxes
                txtMSSkillName.Clear();
                txtMSSkillDesc.Clear();
                selectedSkillID = -1;
                return;
            }

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                try
                {
                    conn.Open();

                    // first deleting the skill from SKILL_OF_PRISONER
                    SqlCommand linkCmd = new SqlCommand("DELETE FROM SKILL_OF_PRISONER WHERE SkillID = @SkillID", conn);
                    linkCmd.Parameters.AddWithValue("@SkillID", selectedSkillID);
                    linkCmd.ExecuteNonQuery();

                    // secondly deleting skill from SKILL_OF_JOB
                    SqlCommand linkCmd2 = new SqlCommand("DELETE FROM SKILL_OF_JOB WHERE SkillID = @SkillID", conn);
                    linkCmd2.Parameters.AddWithValue("@SkillID", selectedSkillID);
                    linkCmd2.ExecuteNonQuery();

                    // Delete from SKILL
                    SqlCommand cmd = new SqlCommand("DELETE FROM SKILL WHERE SkillID = @SkillID", conn);
                    cmd.Parameters.AddWithValue("@SkillID", selectedSkillID);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Skill deleted successfully!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting skill: " + ex.Message);
                }
            }

            // clearing textboxes after skill has been deleted.
            txtMSSkillName.Clear();
            txtMSSkillDesc.Clear();
            selectedSkillID = -1;

            isPopulating = true;
            ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
            cbxMSSkillName.SelectedIndex = -1; // ensuring that nothing is auto selected after the skill is deleted...
            isPopulating = false;
        }



        private void btnMSNext_Click(object sender, EventArgs e)
        {
            panel20.Visible = true;
        }

        private void btnMSAddNewSkill_Click(object sender, EventArgs e)
        {
            panel19.Visible = false;
            panel20.Visible = true;
            txtMSSkillName.Clear();
            txtMSSkillDesc.Clear();
            selectedSkillID = -1;
        }

        private void btnEmployerDetails_Click(object sender, EventArgs e)
        {

            if(cmbEmpDetails.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Employer to edit details.");
                return;
            }
            else
            {
                pnlEmpDetails.Visible = true;
                PnlEmpInfo.Visible = true;
                btnEmpChangePass.Visible = true;
                btnAddNewEmp.Visible = false;
                btnSaveEmp.Visible = true;
                pnlEmpDetails.Visible = false;
            }
               
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            pnlEmpDetails.Visible = false;
            PnlEmpInfo.Visible = true;
            btnAddNewEmp.Visible = true;
            btnEmpChangePass.Visible = false;
            btnSaveEmp.Visible = false;
            pnlEmpMenu.Visible = false;
            btnBackEmp.Visible = true;
        }

        private void btnEditEmp_Click(object sender, EventArgs e)
        {
            pnlEmpDetails.Visible = true;
            pnlEmpMenu.Visible = false;
            btnEmployerDetails.Visible = true;
            btnEmpDel.Visible = false;
            btnBackEmp.Visible = true;
        }

        private void btnDelEmployer_Click(object sender, EventArgs e)
        {
            pnlEmpDetails.Visible = true;
            btnSaveEmp.Visible = false;
            pnlEmpMenu.Visible = false;
            btnEmpDel.Visible = true;
            btnEmployerDetails.Visible = false;
            btnBackEmp.Visible = true;
        }

        private void btnBackEmp_Click(object sender, EventArgs e)
        {
            pnlEmpMenu.Visible = true;
            PnlEmpInfo.Visible = false;
            pnlEmpDetails.Visible = false;
            btnSaveEmp.Visible = false;
            pnlEmpPass.Visible = false;
            btnBackEmp.Visible = false;
        }

        private void btnEmpChangePass_Click(object sender, EventArgs e)
        {
            pnlEmpPass.Visible = true;
        }

        private void btnEmpCancel_Click(object sender, EventArgs e)
        {
            pnlEmpPass.Visible = false;

        }

      

        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clearing all the Job completed details
            cmbPrisonerID.SelectedIndex = -1;
            cmbJobID.SelectedIndex = -1;
            calDateCompleted.SetDate(DateTime.Today);
            lstNotes.Items.Clear();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbJobID.SelectedIndex != -1)
                {
                    if (cmbPrisonerID.SelectedIndex != -1)
                    {
                        DateTime selectedDate = calDateCompleted.SelectionStart;
                        if (selectedDate <= DateTime.Today)
                        {
                            string connStr = DatabaseConfig.GetConStr();

                            SqlConnection conn = new SqlConnection(connStr);

                            conn.Open();
                            string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                            SqlCommand getIdCmd = new SqlCommand(getIdSql, conn);
                            getIdCmd.Parameters.AddWithValue("@JobName", cmbJobID.Text);
                            int jobId = (int)getIdCmd.ExecuteScalar();


                            string sql = "UPDATE JOB SET EndDate = @EndDate, Completed = 1 WHERE JobID = @JobID; UPDATE PRISONER_ON_JOB SET JobFinished = 1 WHERE JobID = @JobID AND PrisonerID = @PrisonerID";
                            SqlCommand comm = new SqlCommand(sql, conn);      

                            comm.Parameters.AddWithValue("@EndDate", selectedDate);
                            comm.Parameters.AddWithValue("@JobID", jobId);          
                            comm.Parameters.AddWithValue("@PrisonerID", cmbPrisonerID.SelectedValue); 
                            comm.ExecuteNonQuery();
                            
                            MessageBox.Show("Job completed has been updated.");


                            conn.Close();

                        }
                        else
                        {
                            MessageBox.Show("Please select an date that is either today or in the past.");
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please select an Prisoner Number.");
                    }

                }
                else
                {
                    MessageBox.Show("Please select an Job.");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void btnAssignGuardsToJobs_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 4;
        }

        private void btnAssignSkillToPris_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 4;
        }

        private void btnAssignSkillToJob_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 5;
        }

        private void btnMaintainEmp_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 1;
        }

        private void btnMaintainPris_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 8;
        }

        private void btnReqReports_Click(object sender, EventArgs e)
        {
            frmReports ReportForm = new frmReports();
            ReportForm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;
        }

        private void btnMPBack_Click(object sender, EventArgs e) { }
        private void frmAdmin_Load(object sender, EventArgs e)
        {

        }

        private void btnASPAdd_Click(object sender, EventArgs e)
        {

        }

        private void btnASJAdd_Click(object sender, EventArgs e)
        {
            JobSkillManager.AddJobSkill((int)cbxASJJobName.SelectedValue, ListboxHelper.GetSelectedSkillID(lstASJAllSkills));

            isPopulating = true;
            ListboxHelper.PopulatAvailableJobSkills(lstASJAllSkills, (int)cbxASJJobName.SelectedValue);
            ListboxHelper.PopulateCurrentJobSkills(lstASJCurrentSkillOfJob, (int)cbxASJJobName.SelectedValue);
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
        private void pnlEmpDetails_VisibleChanged(object sender, EventArgs e)
        {
            //Populating the combobox with employer names on the visible change event of the pnlEmpDetails panel
            ComboBoxHelper.PopulateEmployerNames(cmbEmpDetails);
        }

        private void btnEmpDel_Click(object sender, EventArgs e)
        {
            if (cmbEmpDetails.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an employer!");
                return;
            }

            // Get EmployerID directly from ValueMember
            int employerId = Convert.ToInt32(cmbEmpDetails.SelectedValue);

            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                conn.Open();
                string sql = "DELETE FROM EMPLOYER WHERE EmployerID = @ID";
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", employerId);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Employer deleted successfully!");
                    else
                        MessageBox.Show("No matching employer found!");
                }
            }

            // Refresh the ComboBox after deletion
            ComboBoxHelper.PopulateEmployerNames(cmbEmpDetails);

            cmbEmpDetails.SelectedIndex = -1;
            btnEmpDel.Visible = false;
            pnlEmpDetails.Visible = false;
            pnlEmpMenu.Visible = true;  
        }



        private void btnAddNewEmp_Click(object sender, EventArgs e)
        {
            //Add new employer

            string firstName = txtEmpFName.Text;
            string lastName = txtEmpLName.Text;
            string password = txtEmpPass.Text.GetHashCode().ToString();

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter all the details for the employer!");
                return;
            }

            SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr());
            conn.Open();
            string sql = "INSERT INTO EMPLOYER (LastName, FirstName,Password) VALUES (@FName, @LName, @password)";
            using (SqlCommand cmd = new SqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FName", firstName);
                cmd.Parameters.AddWithValue("@LName", lastName);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Employer added successfully!");
            conn.Close();
            txtEmpFName.Clear();
            txtEmpLName.Clear();
            txtEmpPass.Clear();
            btnAddNewEmp.Visible = false;
            PnlEmpInfo.Visible = false;
            pnlEmpMenu.Visible = true;


        }

        private void cmbJobID_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
