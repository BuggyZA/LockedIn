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
namespace LockedIn
{
    public partial class frmAdmin : Form
    {
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
            }
            else
            {
                MessageBox.Show("Please select a Prisoner Name");
            }
        }
    }
}
