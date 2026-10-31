using LockedIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LockedIn.Models
{
    public class Prisoner
    {
        static string constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlCommand cmd;
        /*Get and set methods: E.g To use these in application 
         * Set method: prsnr.LastName = txt.Text; or
         * Get method: txt.Text = prsnr.LastName*/
        public int PrisonerID { get; set; }
        public string PrisonerNum { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string SA_ID { get; set; }
        public string Password { get; set; }
        public bool ClearedToWork { get; set; }

        // Default constructor
        public Prisoner(){}

        // Constructor with parameters
        public Prisoner(string prisonerNum, string lastName, string firstName, string saId, string password, bool clearedToWork)
        {
            PrisonerNum = prisonerNum;
            LastName = lastName;
            FirstName = firstName;
            SA_ID = saId;
            Password = password;
            ClearedToWork = clearedToWork;
        }

        // Method to validate SA ID format (South African ID)
        public bool IsValidSAID()
        {
            if (string.IsNullOrEmpty(SA_ID) || SA_ID.Length != 13)
                return false;

            return SA_ID.All(char.IsDigit);
        }

        //Method to generate unique prsnr num
        public static string GeneratePrisonerNum()
        {
            Random random = new Random();
            string prisonerNum;

            // Generate 6 random digits
            int randomNumber = random.Next(100000, 999999);
            prisonerNum = "P00" + randomNumber.ToString();

            while (PrisonerNumExists(prisonerNum))
            {
                randomNumber = random.Next(100000, 999999);
                prisonerNum = "P00" + randomNumber.ToString();
            }

            return prisonerNum;
        }

        private static bool PrisonerNumExists(string prisonerNum)
        {
            try
            {
                string query = "SELECT COUNT(*) FROM PRISONER WHERE PrisonerNum = @PrisonerNum";
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrisonerNum", prisonerNum);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error checking prisoner number: {ex.Message}");
                return true;
            }
        }

        // Main validation method for new prisoner
        public bool ValidateNewPrisoner()
        {
            // Validate First Name
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                MessageBox.Show("First Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Last Name
            if (string.IsNullOrWhiteSpace(LastName))
            {
                MessageBox.Show("Last Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            

            // Validate SA ID
            if (string.IsNullOrWhiteSpace(SA_ID))
            {
                MessageBox.Show("SA ID is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!IsValidSAID())
            {
                MessageBox.Show("Invalid SA ID format. Must be 13 digits.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(Password))
            {
                MessageBox.Show("Password is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (Password.Length < 6)
            {
                MessageBox.Show("Password must be at least 6 characters.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            

            return true;
        }

    }
}
