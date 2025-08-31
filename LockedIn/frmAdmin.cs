using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Managers;
using LockedIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices.ComTypes;
using System.Xml.Linq;
namespace LockedIn
{

    public partial class frmAdmin : Form
    {
        Prisoner thisPrisoner = null;

        string Ppassword = "";
        bool isAddingPrisoner = false;
        bool isDeletingPrisoner = false;
        bool isupdatingPrisoner = false;


        int skillID;
        int prsnrID;
        int selectedSkillID; // this is for editing and deleting tracking of skills

        string action = "";// this for maintain jobs to hold say if data needs to be edited, addded or deleted
        int selectedJobID = -1; // This will hold the selected JobID for Edit/Delete


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
            tcAdmin.SelectedIndex = 3;
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isPopulating = true;
            //Popuating combo boxes.
            if (tcAdmin.SelectedIndex == 4)
            {
                cbxJobName.SelectedIndexChanged -= cbxJobName_SelectedIndexChanged;
                ComboBoxHelper.PopulateJobNames(cbxJobName);
                cbxJobName.SelectedIndexChanged += cbxJobName_SelectedIndexChanged;


                pnlJobDetails.Visible = true;
                pnlGuardDetails.Visible = false;
            }
            else if (tcAdmin.SelectedIndex == 6) //Maintain skills page
            {
                ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
            }
            else if (tcAdmin.SelectedIndex == 7)
            {
                cmbPrisonerName.SelectedIndexChanged -= cmbPrisonerName_SelectedIndexChanged;
                ComboBoxHelper.PopulatePrisonerNumbers(cmbPrisonerName);
                cmbPrisonerName.SelectedIndexChanged += cmbPrisonerName_SelectedIndexChanged;
            }
            //Maintain Prisoner Page
            else if (tcAdmin.SelectedIndex == 8)
            {
                ComboBoxHelper.PopulatePrisonerNumbers(cmbMPPrisonerNo);
                pnlPInfo.Visible = false;
                pnlMPSkills.Visible = false;
                cmbMPPrisonerNo.Visible = false;
                pnlMPChoices.Visible = true;
                pnlModifyPPassword.Visible = false;
            }
            else if (tcAdmin.SelectedIndex == 2)
            {
                ComboBoxHelper.PopulateJobNames(cbxCJJobName);
                calDateCompleted.MaxDate = DateTime.Today;
            }
            else if (tcAdmin.SelectedIndex == 5)
            {
                ComboBoxHelper.PopulateJobNames(cbxASJJobName);

            }
            else if (tcAdmin.SelectedIndex == 3)
            {
                pnladJobDetails.Visible = false;
                pnlAdDate.Visible = false;
                pnlAdSkills.Visible = false;
                pnlAdFunctions.Visible = true;
                
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

                thisPrisoner = PrisonerManager.LoadPrisoner(prsnrID);
                Ppassword = thisPrisoner.Password;
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
            PrisonerSkillManager.AddPrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }

        private void btnMPRemove_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.RemovePrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }

        private void lstMPCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstMPCurrentSkills);
        }



        //Edit Prisoner
        private void button46_Click(object sender, EventArgs e)
        {
            isupdatingPrisoner = true;

            txtID.Enabled = false;
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

            txtID.Enabled = false;
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

            txtID.Enabled = true;
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
            if (isAddingPrisoner && pnlMPSkills.Enabled == false)
            {
                thisPrisoner = new Prisoner();

                thisPrisoner.PrisonerNum = Prisoner.GeneratePrisonerNum();
                thisPrisoner.LastName = txtLastName.Text.Trim();
                thisPrisoner.FirstName = txtFirstName.Text.Trim();
                thisPrisoner.SA_ID = txtID.Text.Trim();
                thisPrisoner.Password = Ppassword;
                thisPrisoner.ClearedToWork = ckbCleared.Checked;

                if (thisPrisoner.ValidateNewPrisoner())
                {
                    thisPrisoner.PrisonerID = PrisonerManager.AddPrisoner(thisPrisoner);
                    if (thisPrisoner.PrisonerID > 0)
                    {
                        MessageBox.Show("Prisoner " + thisPrisoner.PrisonerNum + " successfully added! Now you can manage their skills");

                        lstMPAllSkills.SelectedIndexChanged -= lstMPAllSkills_SelectedIndexChanged;
                        ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
                        lstMPAllSkills.SelectedIndexChanged += lstMPAllSkills_SelectedIndexChanged;
                    }
                }
                else
                {
                    return;
                }

                pnlMPSkills.Enabled = true;
                return;
            }
            else if (isupdatingPrisoner)
            {
                thisPrisoner.LastName = txtLastName.Text.Trim();
                thisPrisoner.FirstName = txtFirstName.Text.Trim();
                thisPrisoner.SA_ID = txtID.Text.Trim();
                thisPrisoner.Password = Ppassword;
                thisPrisoner.ClearedToWork = ckbCleared.Checked;

                if (thisPrisoner.ValidateNewPrisoner())
                {
                    bool pUpdated = PrisonerManager.UpdatePrisoner(thisPrisoner);
                    if (pUpdated)
                    {
                        MessageBox.Show("Prisoner " + thisPrisoner.PrisonerNum + " updated successfully!");
                    }
                }
                else
                {
                    return;
                }
            }
            else if (isDeletingPrisoner)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this prisoner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    // User clicked Yes - proceed with deletion
                    if (PrisonerManager.DeletePrisoner(thisPrisoner.PrisonerID))
                    {
                        MessageBox.Show("Prisoner " + thisPrisoner.PrisonerNum + " deleted successfully.");
                        pnlPInfo.Enabled = true;
                        pnlModifyPPassword.Enabled = true;
                        pnlMPSkills.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Deletion could not proceed. Try again");
                    }
                }
                else
                {
                    // User clicked No - cancel deletion
                    MessageBox.Show("Delete cancelled.");
                }
            }

            if (isAddingPrisoner)
            {
                MessageBox.Show("Prisoner data saved!!");
            }

            lstASJAllSkills.DataSource = null;
            lstASJAllSkills.Items.Clear();
            lstMPCurrentSkills.DataSource = null;
            lstMPCurrentSkills.Items.Clear();

            pnlPInfo.Visible = false;
            pnlMPSkills.Visible = false;
            cmbMPPrisonerNo.Visible = false;
            pnlMPChoices.Visible = true;

            cmbMPPrisonerNo.SelectedIndexChanged -= cmbMPPrisonerNo_SelectedIndexChanged;
            ComboBoxHelper.PopulatePrisonerNumbers(cmbMPPrisonerNo);
            cmbMPPrisonerNo.SelectedIndexChanged += cmbMPPrisonerNo_SelectedIndexChanged;

            isAddingPrisoner = false;
            isDeletingPrisoner = false;
            isupdatingPrisoner = false;
            Ppassword = "";

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
                    Ppassword = txtNewPassword.Text;
                }
                else
                {
                    MessageBox.Show("Please enter the same password in both open spaces", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            else if (isupdatingPrisoner)
            {
                if (txtCurrentPassword.Text.Trim() == thisPrisoner.Password)
                {
                    if (txtNewPassword.Text == txtConfirmPassword.Text && txtNewPassword.Text != "")
                    {
                        if (txtNewPassword.Text.Trim().Length < 6)
                        {
                            MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        MessageBox.Show("Saving new password");
                        Ppassword = txtNewPassword.Text;
                    }
                    else
                    {
                        MessageBox.Show("Please enter the same password in both open spaces", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Wrong password. Password Change denied", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

            }

            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;


        }

        private void btnCompleteJobs_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 2;
        }

        private void btnMaintainSkills_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 6;
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
            //show panel to edit employer details with data auto filled in
            if (cmbEmpDetails.SelectedIndex == -1)
            {
                MessageBox.Show("Please select an Employer to edit details.");
                return;
            }
            else
            {



                int employerId = Convert.ToInt32(cmbEmpDetails.SelectedValue);

                using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
                {
                    conn.Open();

                    string sql = "SELECT EmployerID, FirstName, LastName FROM EMPLOYER WHERE EmployerID = @EmployerID";
                    using (SqlCommand cmd = new SqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", employerId);

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

                PnlEmpInfo.Visible = true;
                btnEmpChangePass.Visible = true;
                btnAddNewEmp.Visible = false;
                txtEmpFName.Enabled = false;
                txtEmpLName.Enabled = false;

                pnlEmpDetails.Visible = false;
                txtEmpPass.Visible = false;
                lblEmpPass.Visible = false;
                btnEmpEdit.Visible = true;
                imgMEPass.Visible = false;


            }
        }



        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            //show panel to ley user insert new employer 
            pnlEmpDetails.Visible = false;
            PnlEmpInfo.Visible = true;
            btnAddNewEmp.Visible = true;
            btnEmpChangePass.Visible = false;
            pnlEmpMenu.Visible = false;
            btnBackEmp.Visible = true;
            txtEmpID.Clear();
            txtEmpFName.Clear();
            txtEmpLName.Clear();
            txtEmpPass.Clear();
            txtEmpFName.Enabled = true;
            txtEmpLName.Enabled = true;
            lblEmpPass.Visible = true;
            txtEmpPass.Visible = true;
            imgMEPass.Visible = true;
        }

        private void btnEditEmp_Click(object sender, EventArgs e)
        {
            //show panel for user to select employer to edit

            pnlEmpDetails.Visible = true;
            pnlEmpMenu.Visible = false;
            btnEmployerDetails.Visible = true;
            btnEmpDel.Visible = false;
            btnBackEmp.Visible = true;
        }

        private void btnDelEmployer_Click(object sender, EventArgs e)
        {
            //show panel for user to select employer to delete
            pnlEmpDetails.Visible = true;

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
            btnEmpEdit.Visible = false;
            pnlEmpPass.Visible = false;
           btnBackEmp.Visible = false;
            btnEmpSave.Visible = false;
        }

        private void btnEmpChangePass_Click(object sender, EventArgs e)
        {
            //show the panel to change password
            pnlEmpPass.Visible = true;
        }

        private void btnEmpCancel_Click(object sender, EventArgs e)
        {
            pnlEmpPass.Visible = false;
            txtEmpNew.Clear();
            txtEmpConfirm.Clear();

        }



        private void btnClear_Click(object sender, EventArgs e)
        {
            //Clearing all the Job completed details
            cbxCJJobName.SelectedIndex = -1;
            calDateCompleted.SetDate(DateTime.Today);
            lstNotes.Items.Clear();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                if (cbxCJJobName.SelectedIndex != -1)
                {
                    DateTime selectedDate = calDateCompleted.SelectionStart;
                    if (selectedDate <= DateTime.Today)
                    {
                        string connStr = DatabaseConfig.GetConStr();

                        SqlConnection conn = new SqlConnection(connStr);

                        conn.Open();
                        string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                        SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                        getIdcomm.Parameters.AddWithValue("@JobName", cbxCJJobName.Text);
                        int jobId = (int)getIdcomm.ExecuteScalar();


                        string sql = "UPDATE JOB SET EndDate = @EndDate, Completed = 1 WHERE JobID = @JobID; UPDATE PRISONER_ON_JOB SET JobFinished = 1 WHERE JobID = @JobID";
                        SqlCommand comm = new SqlCommand(sql, conn);

                        comm.Parameters.AddWithValue("@EndDate", selectedDate);
                        comm.Parameters.AddWithValue("@JobID", jobId);
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
            tcAdmin.SelectedIndex = 4;
        }

        private void btnAssignSkillToPris_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 7;
        }

        private void btnAssignSkillToJob_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 5;
        }

        private void btnMaintainEmp_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 1;
        }

        private void btnMaintainPris_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 8;
        }

        private void btnReqReports_Click(object sender, EventArgs e)
        {
            this.Hide();
            frmReports ReportForm = new frmReports();
            ReportForm.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;
        }

        private void btnASPAdd_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.AddPrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstASPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstASPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }



        private void pnlEmpDetails_VisibleChanged(object sender, EventArgs e)
        {
            //Populating the combobox with employer names on the visible change event of the pnlEmpDetails panel
            ComboBoxHelper.PopulateEmployerNames(cmbEmpDetails);
        }

        private void btnEmpDel_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete this prisoner?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
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
                }

                // Refresh the ComboBox after deletion
                ComboBoxHelper.PopulateEmployerNames(cmbEmpDetails);

                cmbEmpDetails.SelectedIndex = -1;
                btnEmpDel.Visible = false;
                pnlEmpDetails.Visible = false;
                pnlEmpMenu.Visible = true;
                btnBackEmp.Visible = false;
            }
            else
            {
                // User clicked No - cancel deletion
                MessageBox.Show("Delete cancelled.");
            }//delete employer
            
        }



        private void btnAddNewEmp_Click(object sender, EventArgs e)
        {
            //Add new employer
            if (txtEmpFName.Text == "" || txtEmpLName.Text == "" || txtEmpPass.Text == "")
            {
                MessageBox.Show("Please enter all the details for the employer!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else if (txtEmpPass.Text.Trim().Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string firstName = txtEmpFName.Text;
            string lastName = txtEmpLName.Text;
            string password = PasswordHasser.HashPassword(txtEmpPass.Text);

            

            SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr());
            conn.Open();
            string sql = "INSERT INTO EMPLOYER ( FirstName, LastName,Password) VALUES (@FName, @LName, @password)";
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
            btnBackEmp.Visible = false;
            imgMEPass.Visible = false;


        }

        private void pnlMPChoices_VisibleChanged(object sender, EventArgs e)
        {
            if (pnlMPChoices.Visible == true)
            {
                txtPNo.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtID.Clear();
                ckbCleared.Checked = false;
                lstMPCurrentSkills.ClearSelected();
                lstMPAllSkills.ClearSelected();
                txtCurrentPassword.Clear();
                txtNewPassword.Clear();
                txtConfirmPassword.Clear();

                cmbMPPrisonerNo.SelectedIndexChanged -= cmbMPPrisonerNo_SelectedIndexChanged;
                cmbMPPrisonerNo.SelectedIndex = -1;
                cmbMPPrisonerNo.SelectedIndexChanged += cmbMPPrisonerNo_SelectedIndexChanged;
                pnlPInfo.Enabled = true;
                pnlModifyPPassword.Enabled = true;
                pnlMPSkills.Enabled = true;
            }
        }

        private void cmbPrisonerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            prsnrID = ComboBoxHelper.GetSelectedID(cmbPrisonerName);
            thisPrisoner = PrisonerManager.LoadPrisoner(prsnrID);

            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstASPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstASPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;

        }

        private void btnASPRemove_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.RemovePrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstASPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstASPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }

        private void lstASPAllSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstASPAllSkills);
        }

        private void button40_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Skills of Prisoner saved!");

            cmbPrisonerName.SelectedIndexChanged -= cmbPrisonerName_SelectedIndexChanged;
            cmbPrisonerName.SelectedIndex = -1;
            cmbPrisonerName.SelectedIndexChanged += cmbPrisonerName_SelectedIndexChanged;

            lstASPAllSkills.DataSource = null;
            lstASPAllSkills.Items.Clear();
            lstASPCurrentSkills.DataSource = null;
            lstASPCurrentSkills.Items.Clear();

        }

        private void button41_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to return to the main menu?", "Confirm Return", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                cmbPrisonerName.SelectedIndexChanged -= cmbPrisonerName_SelectedIndexChanged;
                cmbPrisonerName.SelectedIndex = -1;
                cmbPrisonerName.SelectedIndexChanged += cmbPrisonerName_SelectedIndexChanged;

                lstASPAllSkills.DataSource = null;
                lstASPAllSkills.Items.Clear();
                lstASPCurrentSkills.DataSource = null;
                lstASPCurrentSkills.Items.Clear();

                frmLogIn loginForm = new frmLogIn();
                this.Close();
                loginForm.ShowDialog();
            }
            else
            {

            }
        }

        private void lstASPCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstASPCurrentSkills);
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            //Add job button

            try
            {
                //making panels visible
                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdSkills.Visible = true;
                pnlAdFunctions.Visible = false;
                cmbJobNAme.Visible = false;

                selectedJobID = -1;
                action = "ADD";

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }

        private void button30_Click_1(object sender, EventArgs e)
        {
            //Edit existing job
            try
            {
                //making panels visible
                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdSkills.Visible = true;
                pnlAdFunctions.Visible = false;
                txtMJJobName.Visible = false;
                ComboBoxHelper.PopulateJobNames(cmbJobNAme);

                action = "EDIT";

                string sql = "SELECT * FROM JOB WHERE JobID = @JobID";
                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                    SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                    getIdcomm.Parameters.AddWithValue("@JobName", cbxCJJobName.SelectedItem.ToString());
                    int jobId = (int)getIdcomm.ExecuteScalar();
                    ListboxHelper.PopulateAllPSkills(listBox1);
                    ListboxHelper.PopulateCurrentJobSkills(listBox2,jobId);
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@JobID", jobId);

                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populating the form tools
                            cmbJobNAme.Text = reader["JobName"].ToString();
                            rtbJobDescription.Text = reader["JobDescription"].ToString();
                            rtbJobLocation.Text = reader["JobLocation"].ToString();
                            calStartDate.SelectionStart = Convert.ToDateTime(reader["StartDate"]);
                            calEndDate.SelectionStart = Convert.ToDateTime(reader["EndDate"]);
                            nudDuration.Value = Convert.ToDecimal(((TimeSpan)reader["DurationPerDay"]).TotalHours);
                            cbJobCompleted.Checked = Convert.ToBoolean(reader["Completed"]);
                            numericUpDown1.Value = Convert.ToDecimal(reader["PrisonersNeeded"]);

                        
                        }
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //save button  

            try
            {
                //setting values into variables
                //string employer_name = ;
                string job_name = txtMJJobName.Text;
                string job_description = rtbJobDescription.Text;
                string job_location = rtbJobLocation.Text;
                DateTime startDate = calStartDate.SelectionStart;
                DateTime endDate = calEndDate.SelectionStart;
                string skills = listBox2.Text;
                decimal duration = nudDuration.Value;
                bool job_completed = cbJobCompleted.Checked;
                decimal prisoners = numericUpDown1.Value;

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
                            comm.Parameters.AddWithValue("@prisonersNeeded", prisoners);
                            comm.Parameters.AddWithValue("@completed", job_completed);
                            comm.Parameters.AddWithValue("@durationPerDay", duration);

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
                            getIdcomm.Parameters.AddWithValue("@JobName", cmbJobNAme.Text);
                            int selectedJobId = (int)getIdcomm.ExecuteScalar();

                            comm.Parameters.AddWithValue("@jobID", selectedJobID);
                            comm.Parameters.AddWithValue("@jobName", cmbJobNAme.Text);
                            comm.Parameters.AddWithValue("@jobDescription", job_description);
                            comm.Parameters.AddWithValue("@jobLocation", job_location);
                            comm.Parameters.AddWithValue("@startDate", startDate);
                            comm.Parameters.AddWithValue("@endDate", endDate);
                            comm.Parameters.AddWithValue("@prisonersNeeded", prisoners);
                            comm.Parameters.AddWithValue("@completed", job_completed);
                            comm.Parameters.AddWithValue("@durationPerDay", duration);


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
                            getIdcomm.Parameters.AddWithValue("@JobName", cmbJobNAme.Text);
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

        private void button31_Click(object sender, EventArgs e)
        {
            //Delete existing job
            try
            {
                //making panels visible
                pnladJobDetails.Visible = true;
                pnlAdDate.Visible = true;
                pnlAdSkills.Visible = true;
                pnlAdFunctions.Visible = false;
                txtMJJobName.Visible = false;
                ComboBoxHelper.PopulateJobNames(cmbJobNAme);

                action = "DELETE";
                string sql = "SELECT * FROM JOB WHERE JobID = @JobID";
                string connStr = DatabaseConfig.GetConStr();
                using (SqlConnection conn = new SqlConnection(connStr))
                {
                    conn.Open();
                    string getIdSql = "SELECT JobID FROM JOB WHERE JobName = @JobName";
                    SqlCommand getIdcomm = new SqlCommand(getIdSql, conn);
                    getIdcomm.Parameters.AddWithValue("@JobName", cbxCJJobName.SelectedItem.ToString());
                    int jobId = (int)getIdcomm.ExecuteScalar();
                    ListboxHelper.PopulateAllPSkills(listBox1);
                    ListboxHelper.PopulateCurrentJobSkills(listBox2, jobId);
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@JobID", jobId);

                        SqlDataReader reader = comm.ExecuteReader();

                        if (reader.Read())
                        {
                            // Populating the form tools
                            cmbJobNAme.Text = reader["JobName"].ToString();
                            rtbJobDescription.Text = reader["JobDescription"].ToString();
                            rtbJobLocation.Text = reader["JobLocation"].ToString();
                            calStartDate.SelectionStart = Convert.ToDateTime(reader["StartDate"]);
                            calEndDate.SelectionStart = Convert.ToDateTime(reader["EndDate"]);
                            nudDuration.Value = Convert.ToDecimal(((TimeSpan)reader["DurationPerDay"]).TotalHours);
                            cbJobCompleted.Checked = Convert.ToBoolean(reader["Completed"]);
                            numericUpDown1.Value = Convert.ToDecimal(reader["PrisonersNeeded"]);

                          
                        }
                    }



                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }

        }
        private void btnEmpEdit_Click(object sender, EventArgs e)
        {
            //make textboxes editable
            txtEmpFName.Enabled = true;
            txtEmpLName.Enabled = true;
            btnEmpSave.Visible = true;
            btnEmpEdit.Visible = false;


        }

        private void btnEmpSave_Click(object sender, EventArgs e)
        {
            //Update employer details
            if (txtEmpFName.Text == "" || txtEmpLName.Text == "" )
            {
                MessageBox.Show("Please enter all the details for the employer!");
                return;
            }
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
                        pnlEmpMenu.Visible = true;
                        PnlEmpInfo.Visible = false;
                        pnlEmpDetails.Visible = false;
                        btnEmpEdit.Visible = false;
                        pnlEmpPass.Visible = false;
                        btnBackEmp.Visible = false;
                        btnEmpSave.Visible = false;

                    }


                }
            }
        }

        private void btnEmpSavePass_Click(object sender, EventArgs e)
        {
            //Update employer password
            string empId = txtEmpID.Text;
            string newPass = txtEmpNew.Text;
            string confirmPass = txtEmpConfirm.Text;

            if (newPass != confirmPass)
            {
                MessageBox.Show("passwords do not match!");
                return;
            }
            else if (txtEmpPass.Text.Trim().Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

                newPass = PasswordHasser.HashPassword(newPass);
                MessageBox.Show(newPass);

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

        private void cbxJobName_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlJobDetails.Visible = false;
            int jobID = ComboBoxHelper.GetSelectedID(cbxJobName);
            Job thisJob = JobManager.LoadJob(jobID);

            lblGuardDetails.Text = JobManager.CountPrisonersOnJob(thisJob) + " prisoners are assigned to " + thisJob.JobName + ". Send " + thisJob.AssignedGuards() + " guard(s) to job site";
            pnlGuardDetails.Visible = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            pnlGuardDetails.Visible = false;

            cbxJobName.SelectedIndexChanged -= cbxJobName_SelectedIndexChanged;
            cbxJobName.SelectedIndex = -1;
            cbxJobName.SelectedIndexChanged += cbxJobName_SelectedIndexChanged;

            pnlJobDetails.Visible = true;
        }

      

        //LOGOUT & BACK BUTTONS

        private void button3_Click(object sender, EventArgs e)
        {
            frmLogIn login = new frmLogIn();
            login.logOut();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }
        private void btnMEmpBack_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            tcAdmin.SelectedIndex = 0;;
        }

        //END OF LOGOUT & BACK BUTTONS
        private void btnEmpBack_Click(object sender, EventArgs e)
        {
            pnlEmpMenu.Visible = true;
            PnlEmpInfo.Visible = false;
            pnlEmpDetails.Visible = false;
            btnEmpEdit.Visible = false;
            pnlEmpPass.Visible = false;
            btnBackEmp.Visible = false;
            btnEmpSave.Visible = false;
        }

        private void cbxASJJobName_SelectedIndexChanged_1(object sender, EventArgs e)
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

        private void button3_Click_1(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to return to the main menu?", "Confirm Return", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                txtPNo.Clear();
                txtFirstName.Clear();
                txtLastName.Clear();
                txtID.Clear();
                ckbCleared.Checked = false;
                lstMPCurrentSkills.ClearSelected();
                lstMPAllSkills.ClearSelected();

                cmbMPPrisonerNo.SelectedIndexChanged -= cmbMPPrisonerNo_SelectedIndexChanged;
                cmbMPPrisonerNo.SelectedIndex = -1;
                cmbMPPrisonerNo.SelectedIndexChanged += cmbMPPrisonerNo_SelectedIndexChanged;
                pnlPInfo.Enabled = true;
                pnlModifyPPassword.Enabled = true;
                pnlMPSkills.Enabled = true;

                frmLogIn myLoginform = new frmLogIn();
                this.Close();
                myLoginform.ShowDialog();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string skill = "";

            skill = listBox1.SelectedItem.ToString();
            listBox2.Items.Add(skill);
            listBox1.SelectedItem = "";
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            string skill = "";
            skill = listBox2.SelectedItem.ToString();
            listBox1.Items.Add(skill);
            listBox2.SelectedItem = "";
        }

        private void imgMEPass_MouseDown(object sender, MouseEventArgs e)
        {
            txtEmpPass.UseSystemPasswordChar = false;
            imgMEPass.Image = Properties.Resources.hide;
        }

        private void imgMEPass_MouseUp(object sender, MouseEventArgs e)
        {
            txtEmpPass.UseSystemPasswordChar = true;
            imgMEPass.Image = Properties.Resources.view;
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

        private void imgMPpass_MouseDown(object sender, MouseEventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = false;
            imgMPpass.Image = Properties.Resources.hide;
        }

        private void imgMPpass_MouseUp(object sender, MouseEventArgs e)
        {
            txtNewPassword.UseSystemPasswordChar = true;
            imgMPpass.Image = Properties.Resources.view;
        }
    }
}
    

