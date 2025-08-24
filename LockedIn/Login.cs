using LockedIn.Data;
using LockedIn.Managers;
using LockedIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockedIn
{
    public partial class frmLogIn : Form
    {
        private const String ADMINUSER = "admin", ADMINPASS = "admin";
        int SelectedUser = 0;
        String conStr = DatabaseConfig.GetConStr();

        public frmLogIn()
        {
            InitializeComponent();
        }

        private void Login_Load(object sender, EventArgs e)
        {
            pnlLogIn.Hide();
            pnlSignUp.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectedUser = 1;
            btnLISignUp.Visible = false;
            frmAdmin adminForm = new frmAdmin();
            adminForm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelectedUser = 3;
            btnLISignUp.Visible = false;
            frmEmployer employerForm = new frmEmployer();
            employerForm.Show();

            pnlAPESelect.Hide();

            lblUsername.Visible = false;
            txtUsername.Visible = false;
            btnLISignUp.Visible = false;

            lblEmployerID.Visible = true;
            cbxEmployerID.Visible = true;

            

            pnlLogIn.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SelectedUser = 2;
            pnlAPESelect.Hide();

            lblUsername.Text = "Prisoner Number: ";
            lblUsername.Visible = true;
            txtUsername.Visible = true;

            lblEmployerID.Visible = false;
            cbxEmployerID.Visible = false;

            btnLISignUp.Visible = true;

            pnlLogIn.Show();








            frmPrisoner prisonerForm = new frmPrisoner();
            prisonerForm.Show();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            pnlLogIn.Hide();
            pnlSignUp.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pnlSignUp.Hide();
            pnlLogIn.Show();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            pnlLogIn.Hide();
            pnlAPESelect.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pnlSignUp.Hide();
            pnlAPESelect.Show();
        }

        private void btnSignUp_Click_1(object sender, EventArgs e)
        {
            if (txtSUFirstName.Text == "" || txtSULastName.Text == "" || txtSUID.Text == "" || txtSUPassword.Text == "")
            {
                MessageBox.Show("Please fill in all fields.");              //Change to real time validation checks? Text on change
                return;
            }

            if (txtSUPassword.Text.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters long.");
                return;
            }

            if (!txtSUID.Text.All(char.IsDigit) || txtSUID.Text.Length != 13)
            {
                MessageBox.Show("Please enter a valid South African ID number.");
                return;
            }

            if (txtSUPassword.Text != txtSUConfirmPass.Text)
            {
                MessageBox.Show("Passwords do not match.");
                return;
            }

            if (PrisonerManager.SAIDExists(txtSUID.Text))
            {
                MessageBox.Show("A prisoner with this South African ID already exists.");
                return;
            }

            Prisoner prisoner = new Prisoner
            {
                PrisonerNum = Prisoner.GeneratePrisonerNum(),
                LastName = txtSULastName.Text,
                FirstName = txtSUFirstName.Text,
                SA_ID = txtSUID.Text,
                Password = txtSUPassword.Text,
                ClearedToWork = false // New prisoners are not cleared to work by default
            };
            if (PrisonerManager.AddPrisoner(prisoner) == true)
            {
                MessageBox.Show($"Sign up successful! Your prisoner number is " + prisoner.PrisonerNum + ". Please log in.");
                pnlSignUp.Hide();
                pnlAPESelect.Show();
                // Clear sign up fields
                txtSUID.Clear();
                txtSUFirstName.Clear();
                txtSULastName.Clear();
                txtSUPassword.Clear();
                txtSUConfirmPass.Clear();
            }
            else
            {
                MessageBox.Show("Sign up failed. Please try again.");

            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            //^^^ Validation of inputs
            using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
            {
                conn.Open();
                SqlCommand cmd = null;
                //=============================
                //Hashing password concern!
                //=============================
                if (SelectedUser == 1) // Admin
                {


                    /*
                    cmd = new SqlCommand("SELECT * FROM ADMINISTRATOR WHERE Username = @username AND Password = @password", conn);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    */
                }
                else if (SelectedUser == 2) // Prisoner
                {
                    cmd = new SqlCommand("SELECT * FROM PRISONER WHERE PrisonerNum = @prisonerNum AND Password = @password", conn);
                    cmd.Parameters.AddWithValue("@prisonerNum", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                }
                else if (SelectedUser == 3) // Employer
                {
                    cmd = new SqlCommand("SELECT * FROM EMOPLOYER WHERE EmployerID = @employerID AND Password = @password", conn);
                    cmd.Parameters.AddWithValue("@employerID", cbxEmployerID.SelectedItem);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                }
                else
                {
                    MessageBox.Show("System Error: User type not selected.");
                    pnlLogIn.Hide();
                    pnlAPESelect.Show();
                    return;
                }
                SqlDataReader reader = cmd.ExecuteReader();
                if (SelectedUser == 2 || SelectedUser == 3)
                {
                    if (reader.HasRows)
                    {
                        MessageBox.Show("Login successful!");
                        if(SelectedUser == 2)
                        {
                            frmPrisoner prisonerForm = new frmPrisoner();
                            prisonerForm.Show();
                        }
                        else if(SelectedUser == 3)
                        {
                            frmEmployer employerForm = new frmEmployer();
                            employerForm.Show();
                        }
                        this.Hide();

                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                        txtPassword.Clear();
                        txtUsername.Clear();
                        cbxEmployerID.SelectedIndex = -1;
                    }
                }
                else
                {
                    if (txtUsername.Text == ADMINUSER && txtPassword.Text == ADMINPASS)
                    {
                        MessageBox.Show("Login successful!");
                        frmAdmin adminForm = new frmAdmin();
                        adminForm.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Invalid username or password.");
                        txtPassword.Clear();
                        txtUsername.Clear();
                    }
                }
                conn.Close();
            }
           
               
        }
    }
}
