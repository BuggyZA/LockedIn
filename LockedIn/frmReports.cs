using LockedIn.Data;
using LockedIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockedIn
{
    public partial class frmReports : Form
    {
        string prisonerNum = "";
        DateTime startDate;
        DateTime endDate;

        String constr = DatabaseConfig.GetConStr();
        SqlDataAdapter adapter;
        DataSet ds;
        SqlCommand command;
        public frmReports()
        {
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            pnlJobsCompleted.Visible = false;
            pnlPrisonerDetails.Visible = false;
            pnlDisplayJobs.Visible = false;
            pnlDisplayPrisoners.Visible = false;
            btnRRNext.Visible = false;
            cmbRRPrisonerNum.Visible = false;
            lblPrisonerNum.Visible=false;

           // ComboBoxHelper.PopulatePrisonerNumbers(cmbRRPrisonerNum);

            

        }

        private void btnRRNext_Click(object sender, EventArgs e)
        {
            if (cmbRRPrisonerNum.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cmbRRPrisonerNum.SelectedItem;
                prisonerNum = selectedRow["PrisonerNum"].ToString();
            }
            startDate = calPrisonerStart.SelectionRange.Start;
            endDate = calPrisonerEnd.SelectionRange.Start;

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql1 = "SELECT p.FirstName + ' ' + p.LastName AS FullName " +
                     "FROM PRISONER p " +
                     "WHERE p.PrisonerNum = @prisonerNum"; 

                    using (command = new SqlCommand(sql1, conn))
                    {
                        command.Parameters.AddWithValue("@prisonerNum", prisonerNum);
                    }


                    object result = command.ExecuteScalar();

                    if (result != null)
                    {
                        lblDetails.Text = result.ToString();  
                    }
                    else
                    {
                        lblDetails.Text = "Prisoner not found";
                    }

                    conn.Close();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying prisoner report: " + ex.Message);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql2 = "SELECT j.JobName, j.JobDescription, j.StartDate, j.EndDate, poj.JobFinished " +
                                 "FROM PRISONER p, PRISONER_ON_JOB poj, JOB j " +
                                 "WHERE p.PrisonerID = poj.PrisonerID AND j.JobID = poj.JobID " +
                                 "AND p.PrisonerNum = @prisonerNum AND poj.JobFinished = 1" +
                                 " AND (j.StartDate BETWEEN @startDate AND @endDate" +
                                 " AND j.EndDate BETWEEN @startDate AND @endDate)" +
                                 //" OR (j.StartDate <= @startDate AND j.EndDate >= @endDate))" +
                                 " ORDER BY j.StartDate";
                    

                    using (command = new SqlCommand(sql2, conn))
                    {
                        command.Parameters.AddWithValue("@prisonerNum", prisonerNum);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                    }


                    ds = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds, "Prisoners");
                    dbDisplayPrisoners.DataSource = ds;
                    dbDisplayPrisoners.DataMember = "Prisoners";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying prisoner report: " + ex.Message);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql3 = "SELECT COUNT(*) AS CompletedJobsCount" +
                                   " FROM PRISONER p, PRISONER_ON_JOB poj, JOB j" +
                                   " WHERE p.PrisonerID = poj.PrisonerID" +
                                   " AND j.JobID = poj.JobID" +
                                   " AND p.PrisonerNum = @prisonerNum " +
                                   " AND poj.JobFinished = 1" +
                                   " AND (j.StartDate BETWEEN @startDate AND @endDate" +
                                   " AND j.EndDate BETWEEN @startDate AND @endDate)";
                                   //" AND (j.StartDate <= @startDate AND j.EndDate >= @endDate))";


                    using (command = new SqlCommand(sql3, conn))
                    {
                        command.Parameters.AddWithValue("@prisonerNum", prisonerNum);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                    }


                    ds = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds, "CompletedJobs");
                    lstPrisonersJobsCompleted.DataSource = ds.Tables["CompletedJobs"];
                    lstPrisonersJobsCompleted.DisplayMember = "CompletedJobsCount";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying prisoner report: " + ex.Message);
            }

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql4 = " SELECT CAST(SUM(DATEDIFF(day, j.StartDate, j.EndDate) * (DATEPART(hour, j.DurationPerDay) + DATEPART(minute, j.DurationPerDay) / 60.0)) AS DECIMAL(10,0)) AS TotalHoursWorked" +
                        " FROM PRISONER p, PRISONER_ON_JOB poj, JOB j" +
                        " WHERE p.PrisonerID = poj.PrisonerID" +
                        " AND j.JobID = poj.JobID AND p.PrisonerNum = @prisonerNum  AND poj.JobFinished = 1 " +
                        " AND (j.StartDate BETWEEN @startDate AND @endDate" +
                        " AND j.EndDate BETWEEN @startDate AND @endDate)";
                       // " AND (j.StartDate <= @startDate AND j.EndDate >= @endDate))";

                    using (command = new SqlCommand(sql4, conn))
                    {
                        command.Parameters.AddWithValue("@prisonerNum", prisonerNum);
                        command.Parameters.AddWithValue("@startDate", startDate);
                        command.Parameters.AddWithValue("@endDate", endDate);
                    }


                    ds = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds, "HoursWorked");
                    lstTotalHours.DataSource = ds.Tables["HoursWorked"];
                    lstTotalHours.DisplayMember = "TotalHoursWorked";

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying prisoner report: " + ex.Message);
            }



            pnlPrisonerDetails.Visible = false;
            pnlDisplayPrisoners.Visible = true;
        }

        private void btnPrisonersReportBack_Click(object sender, EventArgs e)
        {
            pnlReports.Visible = true;
            pnlDisplayPrisoners.Visible = false;
        }

        private void btnReportBack_Click(object sender, EventArgs e)
        {
            pnlDisplayJobs.Visible = false;
            pnlReports.Visible = true;
        }

        private void btnGenerateReport_Click(object sender, EventArgs e)
        {
            startDate = calRRStartDate.SelectionRange.Start;
            endDate = calRREndDate.SelectionRange.Start;


            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql = "SELECT JobName, JobDescription, JobLocation, StartDate, EndDate FROM JOB WHERE Completed = 1" +
                        " AND (StartDate BETWEEN @startDate AND @endDate" +
                        " AND EndDate BETWEEN @startDate AND @endDate)";
                       // " AND (StartDate <= @startDate AND EndDate >= @endDate))";

                    command = new SqlCommand(sql, conn);

                    command.Parameters.AddWithValue("@startDate", startDate);
                    command.Parameters.AddWithValue("@endDate", endDate);

                    ds = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds, "Jobs");
                    dbDisplayJobs.DataSource = ds.Tables["Jobs"];

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error displaying prisoner report: " + ex.Message);
            }

            pnlJobsCompleted.Visible = false;
            pnlDisplayJobs.Visible = true;
        }

        private void btnRRBack_Click(object sender, EventArgs e)
        {
            pnlJobsCompleted.Visible = false;
            pnlReports.Visible = true;
        }

        private void btnJobsCompleted_Click(object sender, EventArgs e)
        {
            pnlJobsCompleted.Visible = true;
            pnlReports.Visible = false;
        }

        private void btnPrisonerRecord_Click(object sender, EventArgs e)
        {
            pnlPrisonerDetails.Visible = true;
            pnlReports.Visible = false;
        }

        private void lstPrisoners_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void calPrisonerStart_DateChanged(object sender, DateRangeEventArgs e)
        {
            
              
        }

        private void calPrisonerEnd_DateChanged(object sender, DateRangeEventArgs e)
        {
            
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to select these dates?", "Confirm dates", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes) 
            {
                startDate = calPrisonerStart.SelectionRange.Start;
                endDate = calPrisonerEnd.SelectionRange.Start;

                lblPrisonerNum.Visible = true;
                btnConfirm.Visible = false;
                btnRRNext.Visible = true;
                cmbRRPrisonerNum.Visible = true;

                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();

                        string sql = "SELECT DISTINCT p.PrisonerNum" +
                        " FROM PRISONER p" +
                        " INNER JOIN PRISONER_ON_JOB poj ON p.PrisonerID = poj.PrisonerID" +
                        " INNER JOIN JOB j ON j.JobID = poj.JobID" +
                        " WHERE poj.JobFinished = 1" + 
                        " AND (j.StartDate BETWEEN @startDate AND @endDate" +
                        " AND j.EndDate BETWEEN @startDate AND @endDate)" +
                        " ORDER BY p.PrisonerNum";


                        using (command = new SqlCommand(sql, conn))
                        {
                            //command.Parameters.AddWithValue("@prisonerID", prisonerID);
                            command.Parameters.AddWithValue("@startDate", startDate);
                            command.Parameters.AddWithValue("@endDate", endDate);
                        }


                        ds = new DataSet();
                        adapter = new SqlDataAdapter(command);
                        adapter.Fill(ds, "Prisoner");
                        cmbRRPrisonerNum.DataSource = ds.Tables["Prisoner"];
                        cmbRRPrisonerNum.DisplayMember = "PrisonerNum";

                        conn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error displaying prisoner report: " + ex.Message);
                }
            }
            else
            {
                calPrisonerEnd.SetDate(DateTime.Now);
                calPrisonerStart.SetDate(DateTime.Now);
            }
            
        }

        private void button21_Click(object sender, EventArgs e)
        {
            frmAdmin myForm = new frmAdmin();
            myForm.ShowDialog();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlReports.Visible = true;
            pnlPrisonerDetails.Visible = false;
        }
    }
}
