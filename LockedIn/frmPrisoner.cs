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
        Prisoner thisPrisoner;
        bool isUpdatingPrisoner = false;
        bool isDeletingPrisoner = false;
        public frmPrisoner()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {

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

            pnlPInfo.Visible = true;
            pnlMPSkills.Visible = true;
            txtPrisonerNo.Visible = true;
            pnlMPChoices.Visible = false;
            txtCurrentPassword.Enabled = true;
        }

        private void btnDeleteP_Click(object sender, EventArgs e)
        {
            isDeletingPrisoner = true;

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
    }
}
