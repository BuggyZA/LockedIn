/*
Hanrè WPJ Delport 50605216
Jonathan Zietsman 50359088
Ashton Manson 51586509
Keenan Buchler 50163566
Leschel Senekal 55558089
Jacobus J van der Merwe 50698044
Keanen J Reeve 50985043
*/
using LockedIn.Data;
using LockedIn.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
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

        private PrintDocument printDocument;
        private int currentRowIndex;
        private DataGridView gridToPrint;
        private int[] columnWidths = new int[] { 150, 325, 200, 150, 150 };

    public frmReports()
        {
            InitializeComponent();
            printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(PrintDocument_PrintPage);
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
            cmbRRPrisonerNum.SelectedIndex = -1;
        }

        private void btnRRNext_Click(object sender, EventArgs e)
        {
            if (cmbRRPrisonerNum.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)cmbRRPrisonerNum.SelectedItem;
                prisonerNum = selectedRow["PrisonerNum"].ToString();

                startDate = calPrisonerStart.SelectionRange.Start;
                endDate = calPrisonerEnd.SelectionRange.Start;

                try
                {
                    using (SqlConnection conn = new SqlConnection(constr))
                    {
                        conn.Open();

                        string sql1 = "SELECT FirstName + ' ' + LastName AS FullName " +
                         "FROM PRISONER " +
                         "WHERE PrisonerNum = @prisonerNum";

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

                        string sql4 = "SELECT CAST(SUM(DATEDIFF(day, j.StartDate, j.EndDate) * j.DurationPerDay) AS DECIMAL(10,0))AS TotalHoursWorked" +
                            " FROM PRISONER p, PRISONER_ON_JOB poj, JOB j" +
                            " WHERE p.PrisonerID = poj.PrisonerID" +
                            " AND j.JobID = poj.JobID AND p.PrisonerNum = @prisonerNum  AND poj.JobFinished = 1 " +
                            " AND (j.StartDate BETWEEN @startDate AND @endDate" +
                            " AND j.EndDate BETWEEN @startDate AND @endDate)";

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
            else
            {
                MessageBox.Show("Please select a prisoner number.");
            }
            
        }

        private void btnPrisonersReportBack_Click(object sender, EventArgs e)
        {
            pnlReports.Visible = true;
            pnlDisplayPrisoners.Visible = false;
            calPrisonerStart.SetDate(DateTime.Now);
            calPrisonerEnd.SetDate(DateTime.Now);
            cmbRRPrisonerNum.SelectedIndex = -1;
            cmbRRPrisonerNum.Visible = false;
            btnConfirm.Visible = true;
            btnRRNext.Visible = false;
            lblPrisonerNum.Visible = false;
        }

        private void btnReportBack_Click(object sender, EventArgs e)
        {
            pnlDisplayJobs.Visible = false;
            pnlReports.Visible = true;
            calRRStartDate.SetDate(DateTime.Now);
            calRREndDate.SetDate(DateTime.Now);
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
            calRRStartDate.SetDate(DateTime.Now);
            calRREndDate.SetDate(DateTime.Now);
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

                if (cmbRRPrisonerNum.SelectedIndex == -1)
                {
                    btnRRNext.Visible = false;
                    MessageBox.Show("There are no prisoners that have completed jobs within this time period");
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
            this.Hide();
            frmAdmin myForm = new frmAdmin();
            myForm.ShowDialog();
            
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            pnlReports.Visible = true;
            pnlPrisonerDetails.Visible = false;
            calPrisonerStart.SetDate(DateTime.Now);
            calPrisonerEnd.SetDate(DateTime.Now);
            cmbRRPrisonerNum.SelectedIndex = -1;
            cmbRRPrisonerNum.Visible = false;
            btnConfirm.Visible = true;
            btnRRNext.Visible = false;
            lblPrisonerNum.Visible = false;
        }

        private void btnPrintJobs_Click(object sender, EventArgs e)
        {
            gridToPrint = dbDisplayJobs;
            currentRowIndex = 0;

            printDocument.DefaultPageSettings.Landscape = true;


            // Show printer selection dialog
            using (PrintDialog printDlg = new PrintDialog())
            {
                printDlg.Document = printDocument;
                printDlg.AllowSomePages = true;
                printDlg.AllowSelection = true;

                if (printDlg.ShowDialog() == DialogResult.OK)
                {
                    printDocument.PrinterSettings = printDlg.PrinterSettings; // use chosen printer
                    printDocument.Print(); // send to printer
                }
            }
        }

        private void PrintDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (gridToPrint == null) return;

            int x = 50, y = 50;
            int cellHeight = 40;
            int colCount = gridToPrint.Columns.Count;

            // Ensure columnWidths array has enough values
            if (columnWidths == null || columnWidths.Length < colCount)
            {
                columnWidths = Enumerable.Repeat(150, colCount).ToArray();
            }

            // Print headers
            for (int i = 0; i < colCount; i++)
            {
                e.Graphics.DrawString(gridToPrint.Columns[i].HeaderText,
                                      new Font("Arial", 10, FontStyle.Bold),
                                      Brushes.Black, x, y);
                x += columnWidths[i];  // use custom width
            }

            y += cellHeight;
            x = 50;

            // Print rows
            while (currentRowIndex < gridToPrint.Rows.Count)
            {
                DataGridViewRow row = gridToPrint.Rows[currentRowIndex];

                for (int i = 0; i < colCount; i++)
                {
                    string text = row.Cells[i].Value?.ToString() ?? "";
                    e.Graphics.DrawString(text,
                                          new Font("Arial", 10),
                                          Brushes.Black, x, y);
                    x += columnWidths[i];  // use custom width
                }

                y += cellHeight;
                x = 50;
                currentRowIndex++;

                if (y > e.MarginBounds.Bottom)
                {
                    e.HasMorePages = true;
                    return;
                }
            }

            currentRowIndex = 0;
            e.HasMorePages = false;
        }

        private void btnPrintPrisoners_Click(object sender, EventArgs e)
        {
            gridToPrint = dbDisplayPrisoners;
            currentRowIndex = 0;

            printDocument.DefaultPageSettings.Landscape = true;

            // Show printer selection dialog
            using (PrintDialog printDlg = new PrintDialog())
            {
                printDlg.Document = printDocument;
                printDlg.AllowSomePages = true;
                printDlg.AllowSelection = true;

                if (printDlg.ShowDialog() == DialogResult.OK)
                {
                    printDocument.PrinterSettings = printDlg.PrinterSettings; // use chosen printer
                    printDocument.Print(); // send to printer
                }
            }
        }

        private void btnMEmpBack_Click(object sender, EventArgs e)
        {
            frmEmployer EMP = new frmEmployer();
            this.Hide();
            EMP.Show();
            btnMEmpBack.Visible = false;
        }

        private void btnbackAdmin_Click(object sender, EventArgs e)
        {
            frmAdmin admin = new frmAdmin();
            this.Hide();
            admin.Show();
            btnbackAdmin.Visible = false;

        }
    }
}
