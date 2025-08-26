using LockedIn.Data;
using LockedIn.Models;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LockedIn.Managers
{
    public class PrisonerManager
    {
        static string constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlCommand cmd;
        static SqlDataReader reader;

        //Method to create a Prisoner object directly from DB
        public static Prisoner LoadPrisoner(int prisonerId)
        {
            Prisoner prisoner = null;

            string query = @"SELECT PrisonerID, PrisonerNum, LastName, FirstName, SA_ID, Password, ClearedToWork FROM PRISONER WHERE PrisonerID = @PrisonerID";

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrisonerID", prisonerId);
                        conn.Open();

                        using (reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                prisoner = new Prisoner
                                {
                                    PrisonerID = (int)reader.GetValue(0),
                                    PrisonerNum = reader.GetValue(1).ToString(),
                                    LastName = reader.GetValue(2).ToString(),
                                    FirstName = reader.GetValue(3).ToString(),
                                    SA_ID = reader.GetValue(4).ToString(),
                                    Password = reader.GetValue(5).ToString(),
                                    ClearedToWork = (bool)reader.GetValue(6)
                                };
                            }
                        }
                    }
                }
            }            
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving prisoner with ID {prisonerId}: {ex.Message}");
                return null;
            }

            return prisoner;
        }

        public static bool UpdatePrisoner(Prisoner prisoner)
        {
            string updateQuery = @"UPDATE PRISONER SET PrisonerNum = @PrisonerNum, LastName = @LastName, FirstName = @FirstName, SA_ID = @SA_ID, Password = @Password, ClearedToWork = @ClearedToWork WHERE PrisonerID = @PrisonerID";

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(updateQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrisonerID", prisoner.PrisonerID);
                        cmd.Parameters.AddWithValue("@PrisonerNum", prisoner.PrisonerNum);
                        cmd.Parameters.AddWithValue("@LastName", prisoner.LastName);
                        cmd.Parameters.AddWithValue("@FirstName", prisoner.FirstName);
                        cmd.Parameters.AddWithValue("@SA_ID", prisoner.SA_ID);
                        cmd.Parameters.AddWithValue("@Password", prisoner.Password);
                        cmd.Parameters.AddWithValue("@ClearedToWork", prisoner.ClearedToWork);

                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected > 0)
                        {
                            return true; // Update successful
                        }
                        else
                        {
                            MessageBox.Show("No prisoner found with the specified ID.");
                            return false; // No rows were updated (prisoner not found)
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating prisoner: {ex.Message}");
                return false;
            }
        }

        public static int AddPrisoner(Prisoner prisoner)
        {
            // Option 1: Use OUTPUT clause (Recommended)
            string insertQuery = @"INSERT INTO PRISONER (PrisonerNum, LastName, FirstName, SA_ID, Password, ClearedToWork) OUTPUT INSERTED.PrisonerID VALUES (@PrisonerNum, @LastName, @FirstName, @SA_ID, @Password, @ClearedToWork)";


            try
            {
                using (conn = new SqlConnection(constr))
                {
                    conn.Open();
                    using (cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrisonerNum", prisoner.PrisonerNum);
                        cmd.Parameters.AddWithValue("@LastName", prisoner.LastName);
                        cmd.Parameters.AddWithValue("@FirstName", prisoner.FirstName);
                        cmd.Parameters.AddWithValue("@SA_ID", prisoner.SA_ID);
                        cmd.Parameters.AddWithValue("@Password", prisoner.Password);
                        cmd.Parameters.AddWithValue("@ClearedToWork", prisoner.ClearedToWork);

                        // For Option 1 (OUTPUT clause):
                        object result = cmd.ExecuteScalar();
                        conn.Close();

                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding prisoner: {ex.Message}");
                return -1;
            }

            return -1;
        }

        public static bool DeletePrisoner(int prisonerId)
        {
            string query = "DELETE FROM PRISONER WHERE PrisonerID = @PrisonerID";

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@PrisonerID", prisonerId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting prisoner with ID {prisonerId}: {ex.Message}");
                return false;
            }
        }

        public static bool SAIDExists(string saId)          //Added for sign up page to check if RSA ID already exists
        {
            try
            {
                string query = "SELECT COUNT(*) FROM PRISONER WHERE SA_ID = @SA_ID";
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@SA_ID", saId);
                        conn.Open();
                        int count = (int)cmd.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error checking SA ID: {ex.Message}");
                return false;
            }
        }
    }
}