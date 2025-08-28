using LockedIn.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using LockedIn.Managers;

namespace LockedIn.Models
{
    public class Job
    {
        static string constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlCommand cmd;
        /*Get and set methods: E.g To use these in application 
         * Set method: prsnr.LastName = txt.Text; or
         * Get method: txt.Text = prsnr.LastName*/
        public int JobID { get; set; }
        public int EmployerID { get; set; }
        public string JobName { get; set; }
        public string JobDescription { get; set; }
        public string JobLocation { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PrisonersNeeded { get; set; }
        public bool Completed { get; set; }
        public int DurationPerDay { get; set; }


        // Default constructor
        public Job(){}

        // Constructor with parameters
        // Constructor with parameters (for Job)
        public Job(int jobId, int employerId, string jobName, string jobDescription,
                   string jobLocation, DateTime startDate, DateTime endDate, int prisonersNeeded,
                   bool completed, int durationPerDay)
        {
            JobID = jobId;
            EmployerID = employerId;
            JobName = jobName;
            JobDescription = jobDescription;
            JobLocation = jobLocation;
            StartDate = startDate;
            EndDate = endDate;
            PrisonersNeeded = prisonersNeeded;
            Completed = completed;
            DurationPerDay = durationPerDay;
        }

        // Method to assign gaurds per 3 prisoners
        public int AssignedGuards()
        {
            return (int)Math.Ceiling((double)JobManager.CountPrisonersOnJob(this) / 3);
        }

        // Main validation method for new prisoner
        public bool ValidateNewJob()
        {
            // Validate Job Name
            if (string.IsNullOrWhiteSpace(JobName))
            {
                MessageBox.Show("Job Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Job Description
            if (string.IsNullOrWhiteSpace(JobDescription))
            {
                MessageBox.Show("Job Description is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Job Location
            if (string.IsNullOrWhiteSpace(JobLocation))
            {
                MessageBox.Show("Job Location is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate EmployerID
            if (EmployerID <= 0)
            {
                MessageBox.Show("Employer ID must be valid.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate PrisonersNeeded
            if (PrisonersNeeded <= 0 || PrisonersNeeded > 10)
            {
                MessageBox.Show("Prisoners needed has to be between 1 and 10", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Validate Start & End Dates
            if (StartDate == default(DateTime))
            {
                MessageBox.Show("Start Date is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (EndDate == default(DateTime))
            {
                MessageBox.Show("End Date is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (EndDate < StartDate)
            {
                MessageBox.Show("End Date cannot be earlier than Start Date.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Validate DurationPerDay
            if (DurationPerDay <= 0 || DurationPerDay > 12)
            {
                MessageBox.Show("Duration per day must be between 1 and 12 hours.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }


    }
}
