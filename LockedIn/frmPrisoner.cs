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
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockedIn
{
    public partial class frmPrisoner : Form
    {
        int skillID;
        Prisoner thisPrisoner;
        bool isUpdatingPrisoner = false;
        bool isDeletingPrisoner = false;
        bool isPopulating = false;
        String Ppassword = "";

        public frmPrisoner()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void panel14_Paint(object sender, PaintEventArgs e)
        {
                    }

        private void btnAFJapply_Click(object sender, EventArgs e)
        {

        }

        private void panel16_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tcPrisoner.SelectedIndex == 1)
            {

                pnlPInfo.Visible = false;
                pnlMPSkills.Visible = false;
                txtPrisonerNo.Visible = false;
                pnlMPChoices.Visible = true;
                pnlModifyPPassword.Visible = false;
            }
        }

        private void frmPrisoner_Load(object sender, EventArgs e)
        {
            //frmLogIn loginForm = new frmLogIn();                                          //Unnessecary??
            thisPrisoner = PrisonerManager.LoadPrisoner(SessionData.LoggedInPrisonerID);

            //Tab control 
            tcPrisoner.Appearance = TabAppearance.FlatButtons;
            tcPrisoner.ItemSize = new Size(0, 1);
            tcPrisoner.SizeMode = TabSizeMode.Fixed;


            //Calendar
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {                                                                               //Put SQL in word doc if correct
                string query = @"SELECT j.JobID, j.StartDate, j.EndDate             
                                FROM JOB j
                                INNER JOIN PRISONER_ON_JOB poj 
                                    ON j.JobID = poj.JobID
                                WHERE poj.PrisonerID = @PrisonerID
                                  AND poj.[Accepted/Rejeced] = 1
                                 ;";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@PrisonerID", SessionData.LoggedInPrisonerID);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                List<DateTime> activeDates = new List<DateTime>();

                DateTime start = DateTime.Now, end = DateTime.Now;
                while (reader.Read())
                {
                    start = Convert.ToDateTime(reader["StartDate"]);
                    end = Convert.ToDateTime(reader["EndDate"]);

                    // Add all days between start and end
                    for (DateTime date = start; date <= end; date = date.AddDays(1))
                    {
                        activeDates.Add(date);
                    }
                }

                reader.Close();

                // Highlight in calendar
                calHPactiveJobs.RemoveAllBoldedDates();
                calHPactiveJobs.BoldedDates = activeDates.ToArray();
                // Highlight range of a job
                calHPactiveJobs.SelectionStart = start;
                calHPactiveJobs.SelectionEnd = end;

                calHPactiveJobs.UpdateBoldedDates();
            }



        }

        private void btnEditP_Click(object sender, EventArgs e)
        {
            isUpdatingPrisoner = true;
            this.PopulateMaintainControls();


            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            txtPrisonerNo.Visible = true;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = true;
        }

        private void PopulateMaintainControls()
        {
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;

            Ppassword = thisPrisoner.Password;
            txtPrisonerNo.Text = thisPrisoner.PrisonerNum;
            txtFirstName.Text = thisPrisoner.FirstName;
            txtLastName.Text = thisPrisoner.LastName;
            txtID.Text = thisPrisoner.SA_ID;
            txtID.Enabled = false;
            ckbCleared.Checked = thisPrisoner.ClearedToWork == true ? true : false;
        }

        private void ClearMaintainControls()
        {
            txtPrisonerNo.Clear();
            txtFirstName.Clear();
            txtLastName.Clear();
            txtID.Clear();
            txtID.Enabled = false;
            ckbCleared.Checked = false;

            lstMPAllSkills.DataSource = null;
            lstMPAllSkills.Items.Clear();

            lstMPCurrentSkills.DataSource = null;
            lstMPCurrentSkills.Items.Clear();
        }

        private void DisableMaintainControls()
        {
            pnlPInfo.Enabled = false;
            pnlModifyPPassword.Enabled = false;
            pnlMPSkills.Enabled = false;

        }

        private void btnDeleteP_Click(object sender, EventArgs e)
        {
            isDeletingPrisoner = true;
            this.PopulateMaintainControls();
            this.DisableMaintainControls();

            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            txtPrisonerNo.Visible = true;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = true;
        }

        
      

        private void btnMPSave_Click(object sender, EventArgs e)
        {
            if (isUpdatingPrisoner)
            {
                thisPrisoner.LastName = txtLastName.Text.Trim();
                thisPrisoner.FirstName = txtFirstName.Text.Trim();
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

                        frmLogIn loginForm = new frmLogIn();
                        this.Close();
                        loginForm.ShowDialog();
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

            lstMPAllSkills.DataSource = null;
            lstMPAllSkills.Items.Clear();
            lstMPCurrentSkills.DataSource = null;
            lstMPCurrentSkills.Items.Clear();

            pnlPInfo.Visible = false;
            pnlMPSkills.Visible = false;
            pnlMPChoices.Visible = true;
            pnlModifyPPassword.Visible = false;

            
            isDeletingPrisoner = false;
            isUpdatingPrisoner = false;

        }

        private void btnSavePassword_Click(object sender, EventArgs e)
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
                    txtCurrentPassword.Text = "";
                    txtConfirmPassword.Text = "";
                    txtNewPassword.Text = "";
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


            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            pnlModifyPPassword.Visible = true;
            pnlPInfo.Visible = false;
        }

        private void btnMPBack_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to return to the main menu?", "Confirm Return", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                this.ClearMaintainControls();

                frmLogIn myLoginform = new frmLogIn();
                this.Close();
                myLoginform.ShowDialog();
            }
            else
            {
                this.ClearMaintainControls();

                lstMPAllSkills.DataSource = null;
                lstMPAllSkills.Items.Clear();
                lstMPCurrentSkills.DataSource = null;
                lstMPCurrentSkills.Items.Clear();

                pnlPInfo.Visible = false;
                pnlMPSkills.Visible = false;
                pnlMPChoices.Visible = true;
                pnlModifyPPassword.Visible = false;


                isDeletingPrisoner = false;
                isUpdatingPrisoner = false;

            }
        }

        private void btnAddSkill_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.AddPrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }

        private void btnRemoveSkill_Click(object sender, EventArgs e)
        {
            PrisonerSkillManager.RemovePrisonerSkill(thisPrisoner.PrisonerID, skillID);
            isPopulating = true;
            ListboxHelper.PopulateAvailablePSkills(lstMPAllSkills, thisPrisoner.PrisonerID);
            ListboxHelper.PopulatePrisonerSkills(lstMPCurrentSkills, thisPrisoner.PrisonerID);
            isPopulating = false;
        }

        private void lstMPAllSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstMPAllSkills);
        }

        private void lstMPCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            skillID = ListboxHelper.GetSelectedSkillID(lstMPCurrentSkills);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            pnlModifyPPassword.Visible = false;
            pnlPInfo.Visible = true;

            txtCurrentPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtNewPassword.Text = "";
        }

        private void btnHPapplyForJobs_Click(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 2;
        }

        //START OF LOGOUT AND BACK BUTTON
        private void button1_Click(object sender, EventArgs e)
        {
            frmLogIn login = new frmLogIn();
            login.logOut();
            this.Hide();
        }

        private void frmPrisoner_Shown(object sender, EventArgs e)
        {
            tcPrisoner.SelectedIndex = 0;
        }

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
                tcPrisoner.SelectedIndex = 0;
            }
            else if (SessionData.SelectedUser == 3)
            {
                frmEmployer employer = new frmEmployer();
                employer.Show();
                this.Hide();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            BackButton();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            BackButton();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            BackButton();
        }

        private void lstAFJskillsRequired_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void rtxjobDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAFJworkersNeeded_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAFJjobLocation_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAFJjobDuration_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






        //END OF LOGOUT AND BACK BUTTON
    }
}
