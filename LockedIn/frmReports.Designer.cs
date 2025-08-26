namespace LockedIn
{
    partial class frmReports
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel26 = new System.Windows.Forms.Panel();
            this.label26 = new System.Windows.Forms.Label();
            this.panel28 = new System.Windows.Forms.Panel();
            this.panel29 = new System.Windows.Forms.Panel();
            this.pnlDisplayPrisoners = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lstPrisoners = new System.Windows.Forms.ListBox();
            this.btnPrisonersReportBack = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dbDisplayPrisoners = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlPrisonerDetails = new System.Windows.Forms.Panel();
            this.calPrisonerEnd = new System.Windows.Forms.MonthCalendar();
            this.calPrisonerStart = new System.Windows.Forms.MonthCalendar();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRRNext = new System.Windows.Forms.Button();
            this.cmbRRPrisonerNum = new System.Windows.Forms.ComboBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.pnlDisplayJobs = new System.Windows.Forms.Panel();
            this.btnReportBack = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dbDisplayJobs = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlJobsCompleted = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.calRREndDate = new System.Windows.Forms.MonthCalendar();
            this.btnGenerateReport = new System.Windows.Forms.Button();
            this.btnRRBack = new System.Windows.Forms.Button();
            this.label29 = new System.Windows.Forms.Label();
            this.calRRStartDate = new System.Windows.Forms.MonthCalendar();
            this.button21 = new System.Windows.Forms.Button();
            this.pnlReports = new System.Windows.Forms.Panel();
            this.label28 = new System.Windows.Forms.Label();
            this.btnJobsCompleted = new System.Windows.Forms.Button();
            this.btnPrisonerRecord = new System.Windows.Forms.Button();
            this.panel26.SuspendLayout();
            this.panel28.SuspendLayout();
            this.panel29.SuspendLayout();
            this.pnlDisplayPrisoners.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbDisplayPrisoners)).BeginInit();
            this.pnlPrisonerDetails.SuspendLayout();
            this.pnlDisplayJobs.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dbDisplayJobs)).BeginInit();
            this.pnlJobsCompleted.SuspendLayout();
            this.pnlReports.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel26
            // 
            this.panel26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.panel26.Controls.Add(this.label26);
            this.panel26.Location = new System.Drawing.Point(5, 5);
            this.panel26.Name = "panel26";
            this.panel26.Size = new System.Drawing.Size(1898, 57);
            this.panel26.TabIndex = 17;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Bahnschrift Condensed", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(0, 0);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(293, 58);
            this.label26.TabIndex = 0;
            this.label26.Text = "Request Reports:";
            // 
            // panel28
            // 
            this.panel28.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.panel28.Controls.Add(this.panel29);
            this.panel28.Location = new System.Drawing.Point(5, 66);
            this.panel28.Name = "panel28";
            this.panel28.Size = new System.Drawing.Size(1920, 980);
            this.panel28.TabIndex = 18;
            // 
            // panel29
            // 
            this.panel29.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(27)))), ((int)(((byte)(27)))));
            this.panel29.Controls.Add(this.pnlDisplayJobs);
            this.panel29.Controls.Add(this.pnlDisplayPrisoners);
            this.panel29.Controls.Add(this.pnlJobsCompleted);
            this.panel29.Controls.Add(this.pnlPrisonerDetails);
            this.panel29.Controls.Add(this.button21);
            this.panel29.Controls.Add(this.pnlReports);
            this.panel29.Location = new System.Drawing.Point(14, 28);
            this.panel29.Name = "panel29";
            this.panel29.Size = new System.Drawing.Size(1839, 879);
            this.panel29.TabIndex = 0;
            // 
            // pnlDisplayPrisoners
            // 
            this.pnlDisplayPrisoners.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pnlDisplayPrisoners.Controls.Add(this.panel2);
            this.pnlDisplayPrisoners.Controls.Add(this.btnPrisonersReportBack);
            this.pnlDisplayPrisoners.Controls.Add(this.panel3);
            this.pnlDisplayPrisoners.Controls.Add(this.label3);
            this.pnlDisplayPrisoners.Location = new System.Drawing.Point(87, 207);
            this.pnlDisplayPrisoners.Name = "pnlDisplayPrisoners";
            this.pnlDisplayPrisoners.Size = new System.Drawing.Size(1458, 642);
            this.pnlDisplayPrisoners.TabIndex = 15;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.panel2.Controls.Add(this.lstPrisoners);
            this.panel2.Location = new System.Drawing.Point(1082, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 297);
            this.panel2.TabIndex = 17;
            // 
            // lstPrisoners
            // 
            this.lstPrisoners.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F);
            this.lstPrisoners.FormattingEnabled = true;
            this.lstPrisoners.ItemHeight = 39;
            this.lstPrisoners.Location = new System.Drawing.Point(5, 6);
            this.lstPrisoners.Name = "lstPrisoners";
            this.lstPrisoners.Size = new System.Drawing.Size(352, 277);
            this.lstPrisoners.TabIndex = 18;
            // 
            // btnPrisonersReportBack
            // 
            this.btnPrisonersReportBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnPrisonersReportBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnPrisonersReportBack.ForeColor = System.Drawing.Color.White;
            this.btnPrisonersReportBack.Location = new System.Drawing.Point(45, 574);
            this.btnPrisonersReportBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrisonersReportBack.Name = "btnPrisonersReportBack";
            this.btnPrisonersReportBack.Size = new System.Drawing.Size(108, 51);
            this.btnPrisonersReportBack.TabIndex = 17;
            this.btnPrisonersReportBack.Text = "Back";
            this.btnPrisonersReportBack.UseVisualStyleBackColor = false;
            this.btnPrisonersReportBack.Click += new System.EventHandler(this.btnPrisonersReportBack_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.panel3.Controls.Add(this.dbDisplayPrisoners);
            this.panel3.Location = new System.Drawing.Point(20, 74);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1056, 495);
            this.panel3.TabIndex = 16;
            // 
            // dbDisplayPrisoners
            // 
            this.dbDisplayPrisoners.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbDisplayPrisoners.Location = new System.Drawing.Point(12, 9);
            this.dbDisplayPrisoners.Name = "dbDisplayPrisoners";
            this.dbDisplayPrisoners.RowHeadersWidth = 62;
            this.dbDisplayPrisoners.RowTemplate.Height = 28;
            this.dbDisplayPrisoners.Size = new System.Drawing.Size(1035, 471);
            this.dbDisplayPrisoners.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(26, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 39);
            this.label3.TabIndex = 11;
            this.label3.Text = "Report:";
            // 
            // pnlPrisonerDetails
            // 
            this.pnlPrisonerDetails.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pnlPrisonerDetails.Controls.Add(this.calPrisonerEnd);
            this.pnlPrisonerDetails.Controls.Add(this.calPrisonerStart);
            this.pnlPrisonerDetails.Controls.Add(this.label5);
            this.pnlPrisonerDetails.Controls.Add(this.label4);
            this.pnlPrisonerDetails.Controls.Add(this.btnRRNext);
            this.pnlPrisonerDetails.Controls.Add(this.cmbRRPrisonerNum);
            this.pnlPrisonerDetails.Controls.Add(this.label31);
            this.pnlPrisonerDetails.Controls.Add(this.label32);
            this.pnlPrisonerDetails.Location = new System.Drawing.Point(308, 202);
            this.pnlPrisonerDetails.Name = "pnlPrisonerDetails";
            this.pnlPrisonerDetails.Size = new System.Drawing.Size(1297, 443);
            this.pnlPrisonerDetails.TabIndex = 13;
            // 
            // calPrisonerEnd
            // 
            this.calPrisonerEnd.Location = new System.Drawing.Point(740, 52);
            this.calPrisonerEnd.Name = "calPrisonerEnd";
            this.calPrisonerEnd.TabIndex = 16;
            // 
            // calPrisonerStart
            // 
            this.calPrisonerStart.Location = new System.Drawing.Point(189, 52);
            this.calPrisonerStart.Name = "calPrisonerStart";
            this.calPrisonerStart.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(27, 144);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(128, 39);
            this.label5.TabIndex = 14;
            this.label5.Text = "Start Date:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(598, 139);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 39);
            this.label4.TabIndex = 13;
            this.label4.Text = "End Date:";
            // 
            // btnRRNext
            // 
            this.btnRRNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnRRNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnRRNext.ForeColor = System.Drawing.Color.White;
            this.btnRRNext.Location = new System.Drawing.Point(1048, 360);
            this.btnRRNext.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRRNext.Name = "btnRRNext";
            this.btnRRNext.Size = new System.Drawing.Size(238, 58);
            this.btnRRNext.TabIndex = 12;
            this.btnRRNext.Text = "Next";
            this.btnRRNext.UseVisualStyleBackColor = false;
            this.btnRRNext.Click += new System.EventHandler(this.btnRRNext_Click);
            // 
            // cmbRRPrisonerNum
            // 
            this.cmbRRPrisonerNum.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbRRPrisonerNum.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbRRPrisonerNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbRRPrisonerNum.FormattingEnabled = true;
            this.cmbRRPrisonerNum.Location = new System.Drawing.Point(189, 340);
            this.cmbRRPrisonerNum.Name = "cmbRRPrisonerNum";
            this.cmbRRPrisonerNum.Size = new System.Drawing.Size(298, 37);
            this.cmbRRPrisonerNum.TabIndex = 2;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(4, 5);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(190, 39);
            this.label31.TabIndex = 0;
            this.label31.Text = "Prisoner Details:";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Bahnschrift Condensed", 14F);
            this.label32.Location = new System.Drawing.Point(28, 343);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(146, 34);
            this.label32.TabIndex = 0;
            this.label32.Text = "Prisoner Num:";
            // 
            // pnlDisplayJobs
            // 
            this.pnlDisplayJobs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pnlDisplayJobs.Controls.Add(this.btnReportBack);
            this.pnlDisplayJobs.Controls.Add(this.panel1);
            this.pnlDisplayJobs.Controls.Add(this.label1);
            this.pnlDisplayJobs.Location = new System.Drawing.Point(224, 191);
            this.pnlDisplayJobs.Name = "pnlDisplayJobs";
            this.pnlDisplayJobs.Size = new System.Drawing.Size(1096, 645);
            this.pnlDisplayJobs.TabIndex = 14;
            // 
            // btnReportBack
            // 
            this.btnReportBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnReportBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnReportBack.ForeColor = System.Drawing.Color.White;
            this.btnReportBack.Location = new System.Drawing.Point(26, 574);
            this.btnReportBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnReportBack.Name = "btnReportBack";
            this.btnReportBack.Size = new System.Drawing.Size(108, 51);
            this.btnReportBack.TabIndex = 14;
            this.btnReportBack.Text = "Back";
            this.btnReportBack.UseVisualStyleBackColor = false;
            this.btnReportBack.Click += new System.EventHandler(this.btnReportBack_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.panel1.Controls.Add(this.dbDisplayJobs);
            this.panel1.Location = new System.Drawing.Point(16, 71);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1056, 495);
            this.panel1.TabIndex = 15;
            // 
            // dbDisplayJobs
            // 
            this.dbDisplayJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dbDisplayJobs.Location = new System.Drawing.Point(10, 9);
            this.dbDisplayJobs.Name = "dbDisplayJobs";
            this.dbDisplayJobs.RowHeadersWidth = 62;
            this.dbDisplayJobs.RowTemplate.Height = 28;
            this.dbDisplayJobs.Size = new System.Drawing.Size(1035, 471);
            this.dbDisplayJobs.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(20, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 39);
            this.label1.TabIndex = 10;
            this.label1.Text = "Report:";
            // 
            // pnlJobsCompleted
            // 
            this.pnlJobsCompleted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pnlJobsCompleted.Controls.Add(this.label2);
            this.pnlJobsCompleted.Controls.Add(this.calRREndDate);
            this.pnlJobsCompleted.Controls.Add(this.btnGenerateReport);
            this.pnlJobsCompleted.Controls.Add(this.btnRRBack);
            this.pnlJobsCompleted.Controls.Add(this.label29);
            this.pnlJobsCompleted.Controls.Add(this.calRRStartDate);
            this.pnlJobsCompleted.Location = new System.Drawing.Point(241, 229);
            this.pnlJobsCompleted.Name = "pnlJobsCompleted";
            this.pnlJobsCompleted.Size = new System.Drawing.Size(1119, 334);
            this.pnlJobsCompleted.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(567, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 39);
            this.label2.TabIndex = 16;
            this.label2.Text = "End Date:";
            // 
            // calRREndDate
            // 
            this.calRREndDate.Location = new System.Drawing.Point(693, 9);
            this.calRREndDate.Name = "calRREndDate";
            this.calRREndDate.TabIndex = 15;
            // 
            // btnGenerateReport
            // 
            this.btnGenerateReport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnGenerateReport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnGenerateReport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateReport.Location = new System.Drawing.Point(705, 269);
            this.btnGenerateReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnGenerateReport.Name = "btnGenerateReport";
            this.btnGenerateReport.Size = new System.Drawing.Size(274, 51);
            this.btnGenerateReport.TabIndex = 14;
            this.btnGenerateReport.Text = "Generate report";
            this.btnGenerateReport.UseVisualStyleBackColor = false;
            this.btnGenerateReport.Click += new System.EventHandler(this.btnGenerateReport_Click);
            // 
            // btnRRBack
            // 
            this.btnRRBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnRRBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnRRBack.ForeColor = System.Drawing.Color.White;
            this.btnRRBack.Location = new System.Drawing.Point(987, 269);
            this.btnRRBack.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnRRBack.Name = "btnRRBack";
            this.btnRRBack.Size = new System.Drawing.Size(108, 51);
            this.btnRRBack.TabIndex = 13;
            this.btnRRBack.Text = "Back";
            this.btnRRBack.UseVisualStyleBackColor = false;
            this.btnRRBack.Click += new System.EventHandler(this.btnRRBack_Click);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(23, 22);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(128, 39);
            this.label29.TabIndex = 9;
            this.label29.Text = "Start Date:";
            // 
            // calRRStartDate
            // 
            this.calRRStartDate.Location = new System.Drawing.Point(163, 9);
            this.calRRStartDate.Name = "calRRStartDate";
            this.calRRStartDate.TabIndex = 1;
            // 
            // button21
            // 
            this.button21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.button21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.button21.ForeColor = System.Drawing.Color.White;
            this.button21.Location = new System.Drawing.Point(1706, 807);
            this.button21.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.button21.Name = "button21";
            this.button21.Size = new System.Drawing.Size(108, 51);
            this.button21.TabIndex = 11;
            this.button21.Text = "Back";
            this.button21.UseVisualStyleBackColor = false;
            // 
            // pnlReports
            // 
            this.pnlReports.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.pnlReports.Controls.Add(this.label28);
            this.pnlReports.Controls.Add(this.btnJobsCompleted);
            this.pnlReports.Controls.Add(this.btnPrisonerRecord);
            this.pnlReports.Location = new System.Drawing.Point(680, 103);
            this.pnlReports.Name = "pnlReports";
            this.pnlReports.Size = new System.Drawing.Size(516, 545);
            this.pnlReports.TabIndex = 9;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Bahnschrift Condensed", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.Location = new System.Drawing.Point(3, 0);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(103, 39);
            this.label28.TabIndex = 8;
            this.label28.Text = "Reports:";
            // 
            // btnJobsCompleted
            // 
            this.btnJobsCompleted.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnJobsCompleted.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnJobsCompleted.ForeColor = System.Drawing.Color.White;
            this.btnJobsCompleted.Location = new System.Drawing.Point(66, 88);
            this.btnJobsCompleted.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnJobsCompleted.Name = "btnJobsCompleted";
            this.btnJobsCompleted.Size = new System.Drawing.Size(404, 166);
            this.btnJobsCompleted.TabIndex = 7;
            this.btnJobsCompleted.Text = "Jobs Completed Per Time Period";
            this.btnJobsCompleted.UseVisualStyleBackColor = false;
            this.btnJobsCompleted.Click += new System.EventHandler(this.btnJobsCompleted_Click);
            // 
            // btnPrisonerRecord
            // 
            this.btnPrisonerRecord.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.btnPrisonerRecord.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnPrisonerRecord.ForeColor = System.Drawing.Color.White;
            this.btnPrisonerRecord.Location = new System.Drawing.Point(66, 323);
            this.btnPrisonerRecord.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnPrisonerRecord.Name = "btnPrisonerRecord";
            this.btnPrisonerRecord.Size = new System.Drawing.Size(404, 166);
            this.btnPrisonerRecord.TabIndex = 5;
            this.btnPrisonerRecord.Text = "Record of Prisoner per Time Period";
            this.btnPrisonerRecord.UseVisualStyleBackColor = false;
            this.btnPrisonerRecord.Click += new System.EventHandler(this.btnPrisonerRecord_Click);
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1867, 984);
            this.Controls.Add(this.panel28);
            this.Controls.Add(this.panel26);
            this.Name = "frmReports";
            this.Text = "frmReports";
            this.Load += new System.EventHandler(this.frmReports_Load);
            this.panel26.ResumeLayout(false);
            this.panel26.PerformLayout();
            this.panel28.ResumeLayout(false);
            this.panel29.ResumeLayout(false);
            this.pnlDisplayPrisoners.ResumeLayout(false);
            this.pnlDisplayPrisoners.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dbDisplayPrisoners)).EndInit();
            this.pnlPrisonerDetails.ResumeLayout(false);
            this.pnlPrisonerDetails.PerformLayout();
            this.pnlDisplayJobs.ResumeLayout(false);
            this.pnlDisplayJobs.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dbDisplayJobs)).EndInit();
            this.pnlJobsCompleted.ResumeLayout(false);
            this.pnlJobsCompleted.PerformLayout();
            this.pnlReports.ResumeLayout(false);
            this.pnlReports.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel26;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Panel panel28;
        private System.Windows.Forms.Panel panel29;
        private System.Windows.Forms.Panel pnlDisplayPrisoners;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ListBox lstPrisoners;
        private System.Windows.Forms.Button btnPrisonersReportBack;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dbDisplayPrisoners;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel pnlPrisonerDetails;
        private System.Windows.Forms.MonthCalendar calPrisonerEnd;
        private System.Windows.Forms.MonthCalendar calPrisonerStart;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRRNext;
        private System.Windows.Forms.ComboBox cmbRRPrisonerNum;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Panel pnlDisplayJobs;
        private System.Windows.Forms.Button btnReportBack;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dbDisplayJobs;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlJobsCompleted;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.MonthCalendar calRREndDate;
        private System.Windows.Forms.Button btnGenerateReport;
        private System.Windows.Forms.Button btnRRBack;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.MonthCalendar calRRStartDate;
        private System.Windows.Forms.Button button21;
        private System.Windows.Forms.Panel pnlReports;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Button btnJobsCompleted;
        private System.Windows.Forms.Button btnPrisonerRecord;
    }
}