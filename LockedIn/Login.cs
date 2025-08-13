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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Login_Load(object sender, EventArgs e)
        {
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmAdmin adminForm = new frmAdmin();
            adminForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmEmployer employerForm = new frmEmployer();
            employerForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmPrisoner prisonerForm = new frmPrisoner();
            prisonerForm.Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
