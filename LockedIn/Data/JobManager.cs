using LockedIn.Data;
using LockedIn.Models;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace LockedIn.Managers
{
    public class JobManager
    {
        static string constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlCommand cmd;
        static SqlDataReader reader;

        //Method to create a Job object directly from DB
        public static Job LoadJob(int jobID)
        {
            Job job = null;

            string query = @"SELECT JobID, EmployerID, JobName, JobDescription, JobLocation, 
                            StartDate, EndDate, Completed, DurationPerDay 
                     FROM JOB 
                     WHERE JobID = @JobID";

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobID", jobID);
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                job = new Job
                                {
                                    JobID = reader.GetInt32(0),
                                    EmployerID = reader.GetInt32(1),
                                    JobName = reader.GetString(2),
                                    JobDescription = reader.GetString(3),
                                    JobLocation = reader.GetString(4),
                                    StartDate = reader.GetDateTime(5),
                                    EndDate = reader.GetDateTime(6),
                                    Completed = reader.GetBoolean(7),
                                    DurationPerDay = reader.GetInt32(8)
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error retrieving job with ID {jobID}: {ex.Message}");
                return null;
            }

            return job;
        }


        public static int AddJob(Job job)
        {
            string insertQuery = @"INSERT INTO JOB 
                            (EmployerID, JobName, JobDescription, JobLocation, 
                             StartDate, EndDate, Completed, DurationPerDay) 
                           VALUES 
                            (@EmployerID, @JobName, @JobDescription, @JobLocation, 
                             @StartDate, @EndDate, @Completed, @DurationPerDay)";

            string selectQuery = "SELECT CAST(@@IDENTITY AS INT)"; // Get last inserted JobID

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    conn.Open();

                    // Insert the new job
                    using (cmd = new SqlCommand(insertQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("@EmployerID", job.EmployerID);
                        cmd.Parameters.AddWithValue("@JobName", job.JobName);
                        cmd.Parameters.AddWithValue("@JobDescription", job.JobDescription);
                        cmd.Parameters.AddWithValue("@JobLocation", job.JobLocation);
                        cmd.Parameters.AddWithValue("@StartDate", job.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", job.EndDate);
                        cmd.Parameters.AddWithValue("@Completed", job.Completed);
                        cmd.Parameters.AddWithValue("@DurationPerDay", job.DurationPerDay);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            return -1; // Insert failed
                        }
                    }

                    // Fetch the new JobID
                    using (cmd = new SqlCommand(selectQuery, conn))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            return Convert.ToInt32(result);
                        }
                    }

                    return -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding job: {ex.Message}");
                return -1;
            }
        }


        public static bool DeleteJob(int jobId)
        {
            string query = "DELETE FROM JOB WHERE JobID = @JobID";

            try
            {
                using (conn = new SqlConnection(constr))
                {
                    using (cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@JobID", jobId);

                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        return rowsAffected > 0; // True if a row was deleted
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting job with ID {jobId}: {ex.Message}");
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