using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Managers;
using LockedIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            tabControl1.SelectedIndex = 1;
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
            if (tabControl1.SelectedIndex == 1)
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
            frmLogIn loginForm = new frmLogIn();
            thisPrisoner = PrisonerManager.LoadPrisoner(loginForm.getLogedInPrisonerID());
            MessageBox.Show(thisPrisoner.PrisonerID.ToString());
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

        
        private void button1_Click(object sender, EventArgs e)
        {
            frmLogIn login = new frmLogIn();
            login.logOut();
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
            tabControl1.SelectedIndex = 2;
        }
    }
}
