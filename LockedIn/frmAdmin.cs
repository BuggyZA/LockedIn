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
namespace LockedIn
{
    public partial class frmAdmin : Form
    {
        int skillID;
        int prsnrID;
        /*This flag checks if the comboboxes are currently busy populating (Because when they are, their indices change 
         * and they invoke the various indexchanged events and this causes several errors)*/
        bool isPopulating = false;
        public frmAdmin()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            isPopulating = true;

            if(tcPrisoner.SelectedIndex == 4)
            {
                ComboBoxHelper.PopulateJobNames(cbxJobName);
            }
            else if (tcPrisoner.SelectedIndex == 6)
            {
                ComboBoxHelper.PopulateSkillNames(cbxMSSkillName);
            }
            else if (tcPrisoner.SelectedIndex == 8)
            {
                ComboBoxHelper.PopulatePrisonerNumbers(cmbMPPrisonerNo);
            }

            isPopulating = false;
        }

        private void cmbMPPrisonerNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isPopulating) return;

            if(cmbMPPrisonerNo.SelectedIndex != -1)
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
    }
}
