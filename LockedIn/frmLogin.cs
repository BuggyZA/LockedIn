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
 * Add Admin table to switch from const username and passwords
 * Migrate form navigation to login instead of APE panel
 * Use password hasher
 * Change the get's to SessionData
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
        int SelectedUser;
        int PrisonerID, EmployerID;     //Variables of ID's to be used on other forms to fetch currently logged in ID
        bool validSignUp = false;

        public frmLogIn()
        {
            InitializeComponent();
        }

        public void logOut()        //Call this method when loging out of user to reset
        {
            PrisonerID = -1;
            EmployerID = -1;
            SelectedUser = 0;
            pnlAPESelect.Visible= true;
            pnlLogIn.Visible = false;
            pnlSignUp.Visible = false;
            SessionData.LoggedInPrisonerID = -1;
            SessionData.LoggedInEmployerID = -1;
            SessionData.SelectedUser = 0;
            this.Show();
        }

        public int getLogedInPrisonerID()           //Used to return the currently logged in Prisoners ID
        {
            return PrisonerID;
            //return 1;
        }

        public int getLogedInEmployerID()          //Used to return the currently logged in Employers ID
        {
            return EmployerID;
            //return 1;
        }

        private void Login_Load(object sender, EventArgs e)
        {
            SelectedUser = 0;
            pnlLogIn.Hide();
            pnlSignUp.Hide();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SessionData.SelectedUser = 1;
            //frmAdmin adminForm = new frmAdmin();
            //adminForm.Show();
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
            SessionData.SelectedUser = 3;

            ComboBoxHelper.PopulateEmployerNames(cbxEmployerID);
           
            lblUsername.Visible = false;
            txtUsername.Visible = false;
            btnLISignUp.Visible = false;

            lblEmployerID.Visible = true;
            cbxEmployerID.Visible = true;

            pnlAPESelect.Hide();
            pnlLogIn.Show();

            //frmEmployer employerForm = new frmEmployer();
            //employerForm.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SessionData.SelectedUser = 2;

            lblUsername.Text = "Prisoner Number: ";
            lblUsername.Visible = true;
            txtUsername.Visible = true;
            btnLISignUp.Visible = true;

            lblEmployerID.Visible = false;
            cbxEmployerID.Visible = false;

            pnlAPESelect.Hide();
            pnlLogIn.Show();

            //frmPrisoner prisonerForm = new frmPrisoner();
            //prisonerForm.Show();
        }

        private void btnSignUp_Click(object sender, EventArgs e)
        {
            pnlLogIn.Hide();
            pnlSignUp.Show();
            txtSUFirstName.Clear();
            txtSULastName.Clear();
            txtSUID.Clear();
            txtSUPassword.Clear();
            txtSUConfirmPass.Clear();
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

        private void txtSUFirstName_TextChanged(object sender, EventArgs e)
        {
            if(txtSUFirstName.Text == "")
            {
                lblSUFNError.Text = "First name cannot be empty";
                validSignUp = false;
            }
            else
            {
                lblSUFNError.Text = "";
                validSignUp = true;
            }
        }

        private void txtSULastName_TextChanged(object sender, EventArgs e)
        {
            if (txtSULastName.Text == "")
            {
                lblSULNError.Text = "Last name cannot be empty";
                validSignUp = false;
            }
            else
            {
                lblSULNError.Text = "";
                validSignUp = true;
            }
        }

        private void txtSUID_TextChanged(object sender, EventArgs e)
        {
            if (!txtSUID.Text.All(char.IsDigit) || txtSUID.Text.Length != 13)
            {
                lblSUIDError.Text = "Enter a valid RSA ID";
                validSignUp = false;
            }
            else
            {
                lblSUIDError.Text = "";
                validSignUp = true;
            }
        }

        private void txtSUPassword_TextChanged(object sender, EventArgs e)
        {
            if(txtSUPassword.Text.Length < 6)
            {
                lblSUPassError.Text = "Password must be at least 6 characters long";
                validSignUp = false;
            }
            else
            {
                lblSUPassError.Text = "";
                validSignUp = true;
            }

        }

        private void txtSUConfirmPass_TextChanged(object sender, EventArgs e)
        {
            if (txtSUPassword.Text != txtSUConfirmPass.Text)
            {
                lblSUConPassError.Text = "Passwords do not match";
                validSignUp = false;
            }
            else
            {
                lblSUConPassError.Text = "";
                validSignUp = true;
            }
        }

        private void btnSignUp_Click_1(object sender, EventArgs e)
        {
            
            try
            {
                if (!validSignUp)
                {
                    MessageBox.Show("Please correct the errors in the sign-up form before proceeding.");
                    return;
                }

                if (PrisonerManager.SAIDExists(txtSUID.Text))
                {
                    MessageBox.Show("A prisoner with this South African ID already exists.");
                    return;
                }

                if(txtSUPassword.Text != txtSUConfirmPass.Text)
                {
                    MessageBox.Show("Passwords do not match.");
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
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while signing up:\n" + ex.Message, "Sign Up Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
           
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Password cannot be empty.");
                return;
            }
            if(txtUsername.Text == "" && SessionData.SelectedUser == 2)
            {
                MessageBox.Show("Prisoner number cannot be empty.");
                return;
            }
            if(cbxEmployerID.SelectedIndex == -1 && SessionData.SelectedUser == 3)
            {
                MessageBox.Show("Please select an Employer ID.");
                return;
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(DatabaseConfig.GetConStr()))
                {
                    conn.Open();
                    SqlCommand cmd = null;

                    if (SessionData.SelectedUser == 1) // Admin
                    {
                        // Example query for admin if needed
                        // cmd = new SqlCommand("SELECT * FROM ADMINISTRATORS WHERE Username = @username AND Password = @password", conn);
                        // cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                        // cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    }
                    else if (SessionData.SelectedUser == 2) // Prisoner
                    {
                        cmd = new SqlCommand("SELECT * FROM PRISONER WHERE PrisonerNum = @prisonerNum AND Password = @password", conn);
                        cmd.Parameters.AddWithValue("@prisonerNum", txtUsername.Text);
                        cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    }
                    else if (SessionData.SelectedUser == 3) // Employer
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

                    if (SessionData.SelectedUser == 2 || SessionData.SelectedUser == 3)
                    {
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (reader.HasRows)
                        {
                            reader.Close();
                            if (SessionData.SelectedUser == 2)
                            {
                                cmd = new SqlCommand("SELECT PrisonerID FROM PRISONER WHERE PrisonerNum = @prisonerNum", conn);
                                cmd.Parameters.AddWithValue("@prisonerNum", txtUsername.Text);
                                PrisonerID = (int)cmd.ExecuteScalar();
                                SessionData.LoggedInPrisonerID = PrisonerID;


                                frmPrisoner prisonerForm = new frmPrisoner();
                                prisonerForm.Show();
                            }
                            else if (SessionData.SelectedUser == 3)
                            {
                                cmd = new SqlCommand("SELECT EmployerID FROM EMPLOYER WHERE EmployerID = @employerID", conn);
                                cmd.Parameters.AddWithValue("@employerID", cbxEmployerID.SelectedValue);
                                EmployerID = (int)cmd.ExecuteScalar();
                                SessionData.LoggedInEmployerID = EmployerID;

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
                    else // Admin
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
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while logging in:\n" + ex.Message, "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
