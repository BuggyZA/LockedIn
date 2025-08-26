using LockedIn.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LockedIn
{
    public partial class frmReports : Form
    {
        string prisonerID = "";
        DateTime startDate;
        DateTime endDate;

        String constr = DatabaseConfig.GetConStr();
        SqlConnection conn;
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

            ComboBoxHelper.PopulatePrisonerNumbers(cmbRRPrisonerNum);
        }

        private void btnRRNext_Click(object sender, EventArgs e)
        {
            prisonerID = cmbRRPrisonerNum.SelectedValue.ToString();

            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();

                    string sql = "SELECT * FROM PRISONER WHERE PrisonerID = @prisonerID";
                    using (command = new SqlCommand(sql, conn))
                    {
                        command.Parameters.AddWithValue("@prisonerID", prisonerID);
                    }


                    ds = new DataSet();
                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(ds, "Prisoners");
                    dbDisplayJobs.DataSource = ds;
                    dbDisplayJobs.DataMember = "Prisoners";

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

                    string sql = "SELECT JobName, JobDescription, JobLocation, StartDate, EndDate FROM JOB WHERE Completed = 1 AND StartDate BETWEEN @startDate AND @endDate";
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
    }
}
