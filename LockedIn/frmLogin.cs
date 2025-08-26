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
/* To Do List:
 * Exception handeling
 * Change to real time input validation for signup
 * Add Admin table to switch from const username and passwords
 * Migrate form navigation to login instead of APE panel
 * Prisoner.cs has validation method, use or not?
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 */
namespace LockedIn
{
    public partial class frmLogIn : Form
    {
        private const String ADMINUSER = "admin", ADMINPASS = "admin";
        int SelectedUser = 0;
        int PrisonerID, EmployerID;     //Variables of ID's to be used on other forms to fetch currently logged in ID

        public frmLogIn()
        {
            InitializeComponent();
        }

        public int getLogedInPrisonerID()           //Used to return the currently logged in Prisoners ID
        {
            return PrisonerID;
        }

        public int getLogedInEmployerID()          //Used to return the currently logged in Employers ID
        {
            return EmployerID;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            pnlLogIn.Hide();
            pnlSignUp.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectedUser = 1;
            frmAdmin adminForm = new frmAdmin();
            adminForm.Show();
            lblUsername.Text = "Username: ";
        
            lblUsername.Visible = true;
            txtUsername.Visible = true;

            lblEmployerID.Visible = false;
            cbxEmployerID.Visible = false;
            btnLISignUp.Visible = false;

            pnlAPESelect.Hide();
            pnlLogIn.Show();


        }

        private void button4_Click(object sender, EventArgs e)
        {
            SelectedUser = 3;
            
            ComboBoxHelper.PopulateEmployerNames(cbxEmployerID);
           
            lblUsername.Visible = false;
            txtUsername.Visible = false;
            btnLISignUp.Visible = false;

            lblEmployerID.Visible = true;
            cbxEmployerID.Visible = true;

            pnlAPESelect.Hide();
            pnlLogIn.Show();

            frmEmployer employerForm = new frmEmployer();
            employerForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SelectedUser = 2;

            lblUsername.Text = "Prisoner Number: ";
            lblUsername.Visible = true;
            txtUsername.Visible = true;
            btnLISignUp.Visible = true;

            lblEmployerID.Visible = false;
            cbxEmployerID.Visible = false;

            pnlAPESelect.Hide();
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

            if (PrisonerManager.AddPrisoner(prisoner) >= 1)
            {
                MessageBox.Show("Sign up successful! Your prisoner number is " + prisoner.PrisonerNum + ". Please log in.");
             
                // Clear sign up fields
                txtSUID.Clear();
                txtSUFirstName.Clear();
                txtSULastName.Clear();
                txtSUPassword.Clear();
                txtSUConfirmPass.Clear();
                txtUsername.Text = prisoner.PrisonerNum;
                pnlSignUp.Hide();
                pnlLogIn.Show();
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
                if (SelectedUser == 1) // Admin
                {
                    /*
                    cmd = new SqlCommand("SELECT * FROM ADMINISTRATORS WHERE Username = @username AND Password = @password", conn);
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
                    cmd = new SqlCommand("SELECT * FROM EMPLOYER WHERE EmployerID = @employerID AND Password = @password", conn);
                    cmd.Parameters.AddWithValue("@employerID", cbxEmployerID.SelectedValue);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                }
                else
                {
                    MessageBox.Show("System Error: User type not selected.");
                    pnlLogIn.Hide();
                    pnlAPESelect.Show();
                    return;
                }
                
                if (SelectedUser == 2 || SelectedUser == 3)
                {
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Close();
                        if (SelectedUser == 2)
                        {
                            cmd = new SqlCommand("SELECT PrisonerID FROM PRISONER WHERE PrisonerNum = @prisonerNum", conn);
                            cmd.Parameters.AddWithValue("@prisonerNum", txtUsername.Text);
                            PrisonerID = (int)cmd.ExecuteScalar();
                            reader.Close();
                            //PrisonerManager.LoadPrisoner(PrisonerID);                       //Loaded PrisonerID into PrisonerManager class
                            
                            frmPrisoner prisonerForm = new frmPrisoner();
                            prisonerForm.Show();
                        }
                        else if (SelectedUser == 3)
                        {
                            cmd = new SqlCommand("SELECT EmployerID FROM EMPLOYER WHERE EmployerID = @employerID", conn);
                            cmd.Parameters.AddWithValue("@employerID", cbxEmployerID.SelectedValue);
                            EmployerID = (int)cmd.ExecuteScalar();                           //EmployerID is ready to be parsed
                            reader.Close();

                            frmEmployer employerForm = new frmEmployer();
                            employerForm.Show();
                        }
                        MessageBox.Show("Login successful!");
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
