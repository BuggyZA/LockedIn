namespace LockedIn
{
    partial class frmLogIn
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLogIn));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnlAPESelect = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnEmployer = new System.Windows.Forms.Button();
            this.btnAdministrator = new System.Windows.Forms.Button();
            this.btnPrisoner = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnlLogIn = new System.Windows.Forms.Panel();
            this.btnLIBack = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.imgPass = new System.Windows.Forms.PictureBox();
            this.lblEmployerID = new System.Windows.Forms.Label();
            this.cbxEmployerID = new System.Windows.Forms.ComboBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.btnLISignUp = new System.Windows.Forms.Button();
            this.btnLogIn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.pnlSignUp = new System.Windows.Forms.Panel();
            this.btnSUBack = new System.Windows.Forms.Button();
            this.panel6 = new System.Windows.Forms.Panel();
            this.imgPass2 = new System.Windows.Forms.PictureBox();
            this.lblSUConPassError = new System.Windows.Forms.Label();
            this.lblSUPassError = new System.Windows.Forms.Label();
            this.lblSUIDError = new System.Windows.Forms.Label();
            this.lblSULNError = new System.Windows.Forms.Label();
            this.lblSUFNError = new System.Windows.Forms.Label();
            this.txtSUID = new System.Windows.Forms.TextBox();
            this.lblSUFirstName = new System.Windows.Forms.Label();
            this.txtSUConfirmPass = new System.Windows.Forms.TextBox();
            this.txtSUPassword = new System.Windows.Forms.TextBox();
            this.lblSUConfirmPass = new System.Windows.Forms.Label();
            this.lblSUPassword = new System.Windows.Forms.Label();
            this.txtSULastName = new System.Windows.Forms.TextBox();
            this.txtSUFirstName = new System.Windows.Forms.TextBox();
            this.lblSULastName = new System.Windows.Forms.Label();
            this.lblSUID = new System.Windows.Forms.Label();
            this.btnSULogIn = new System.Windows.Forms.Button();
            this.btnSignUp = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnlAPESelect.SuspendLayout();
            this.panel3.SuspendLayout();
            this.pnlLogIn.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPass)).BeginInit();
            this.pnlSignUp.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPass2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-69, 1);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(1385, 180);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pnlAPESelect
            // 
            this.pnlAPESelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.pnlAPESelect.Controls.Add(this.panel3);
            this.pnlAPESelect.Controls.Add(this.label2);
            this.pnlAPESelect.Location = new System.Drawing.Point(187, 245);
            this.pnlAPESelect.Margin = new System.Windows.Forms.Padding(4);
            this.pnlAPESelect.Name = "pnlAPESelect";
            this.pnlAPESelect.Size = new System.Drawing.Size(467, 462);
            this.pnlAPESelect.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.panel3.Controls.Add(this.btnEmployer);
            this.panel3.Controls.Add(this.btnAdministrator);
            this.panel3.Controls.Add(this.btnPrisoner);
            this.panel3.Location = new System.Drawing.Point(11, 63);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(447, 388);
            this.panel3.TabIndex = 5;
            // 
            // btnEmployer
            // 
            this.btnEmployer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnEmployer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnEmployer.ForeColor = System.Drawing.Color.White;
            this.btnEmployer.Location = new System.Drawing.Point(59, 278);
            this.btnEmployer.Margin = new System.Windows.Forms.Padding(4);
            this.btnEmployer.Name = "btnEmployer";
            this.btnEmployer.Size = new System.Drawing.Size(328, 66);
            this.btnEmployer.TabIndex = 3;
            this.btnEmployer.Text = "Employer";
            this.btnEmployer.UseVisualStyleBackColor = false;
            this.btnEmployer.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnAdministrator
            // 
            this.btnAdministrator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnAdministrator.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdministrator.ForeColor = System.Drawing.Color.White;
            this.btnAdministrator.Location = new System.Drawing.Point(59, 38);
            this.btnAdministrator.Margin = new System.Windows.Forms.Padding(4);
            this.btnAdministrator.Name = "btnAdministrator";
            this.btnAdministrator.Size = new System.Drawing.Size(328, 66);
            this.btnAdministrator.TabIndex = 1;
            this.btnAdministrator.Text = "Administrator";
            this.btnAdministrator.UseVisualStyleBackColor = false;
            this.btnAdministrator.Click += new System.EventHandler(this.button5_Click);
            // 
            // btnPrisoner
            // 
            this.btnPrisoner.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnPrisoner.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnPrisoner.ForeColor = System.Drawing.Color.White;
            this.btnPrisoner.Location = new System.Drawing.Point(59, 159);
            this.btnPrisoner.Margin = new System.Windows.Forms.Padding(4);
            this.btnPrisoner.Name = "btnPrisoner";
            this.btnPrisoner.Size = new System.Drawing.Size(328, 66);
            this.btnPrisoner.TabIndex = 2;
            this.btnPrisoner.Text = "Prisoner";
            this.btnPrisoner.UseVisualStyleBackColor = false;
            this.btnPrisoner.Click += new System.EventHandler(this.button6_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.label2.Location = new System.Drawing.Point(100, 7);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(269, 54);
            this.label2.TabIndex = 4;
            this.label2.Text = "Home Page";
            // 
            // pnlLogIn
            // 
            this.pnlLogIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.pnlLogIn.Controls.Add(this.btnLIBack);
            this.pnlLogIn.Controls.Add(this.panel5);
            this.pnlLogIn.Controls.Add(this.label3);
            this.pnlLogIn.Location = new System.Drawing.Point(699, 245);
            this.pnlLogIn.Margin = new System.Windows.Forms.Padding(4);
            this.pnlLogIn.Name = "pnlLogIn";
            this.pnlLogIn.Size = new System.Drawing.Size(467, 465);
            this.pnlLogIn.TabIndex = 8;
            // 
            // btnLIBack
            // 
            this.btnLIBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnLIBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnLIBack.BackgroundImage")));
            this.btnLIBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLIBack.Location = new System.Drawing.Point(11, 7);
            this.btnLIBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnLIBack.Name = "btnLIBack";
            this.btnLIBack.Size = new System.Drawing.Size(53, 49);
            this.btnLIBack.TabIndex = 6;
            this.btnLIBack.UseVisualStyleBackColor = false;
            this.btnLIBack.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.panel5.Controls.Add(this.imgPass);
            this.panel5.Controls.Add(this.lblEmployerID);
            this.panel5.Controls.Add(this.cbxEmployerID);
            this.panel5.Controls.Add(this.txtPassword);
            this.panel5.Controls.Add(this.txtUsername);
            this.panel5.Controls.Add(this.lblPassword);
            this.panel5.Controls.Add(this.lblUsername);
            this.panel5.Controls.Add(this.btnLISignUp);
            this.panel5.Controls.Add(this.btnLogIn);
            this.panel5.Location = new System.Drawing.Point(11, 63);
            this.panel5.Margin = new System.Windows.Forms.Padding(4);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(447, 388);
            this.panel5.TabIndex = 5;
            // 
            // imgPass
            // 
            this.imgPass.Image = global::LockedIn.Properties.Resources.view;
            this.imgPass.Location = new System.Drawing.Point(374, 101);
            this.imgPass.Name = "imgPass";
            this.imgPass.Size = new System.Drawing.Size(43, 32);
            this.imgPass.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPass.TabIndex = 36;
            this.imgPass.TabStop = false;
            this.imgPass.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgPass_MouseDown);
            this.imgPass.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgPass_MouseUp);
            // 
            // lblEmployerID
            // 
            this.lblEmployerID.AutoSize = true;
            this.lblEmployerID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblEmployerID.Location = new System.Drawing.Point(112, 7);
            this.lblEmployerID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblEmployerID.Name = "lblEmployerID";
            this.lblEmployerID.Size = new System.Drawing.Size(205, 29);
            this.lblEmployerID.TabIndex = 9;
            this.lblEmployerID.Text = "Employer Names:";
            // 
            // cbxEmployerID
            // 
            this.cbxEmployerID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.cbxEmployerID.FormattingEnabled = true;
            this.cbxEmployerID.Location = new System.Drawing.Point(31, 15);
            this.cbxEmployerID.Margin = new System.Windows.Forms.Padding(4);
            this.cbxEmployerID.Name = "cbxEmployerID";
            this.cbxEmployerID.Size = new System.Drawing.Size(377, 37);
            this.cbxEmployerID.TabIndex = 8;
            this.cbxEmployerID.TextChanged += new System.EventHandler(this.cbxEmployerID_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtPassword.Location = new System.Drawing.Point(190, 98);
            this.txtPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(177, 34);
            this.txtPassword.TabIndex = 9;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtUsername.Location = new System.Drawing.Point(190, 40);
            this.txtUsername.Margin = new System.Windows.Forms.Padding(4);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(177, 34);
            this.txtUsername.TabIndex = 7;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblPassword.Location = new System.Drawing.Point(4, 101);
            this.lblPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(126, 29);
            this.lblPassword.TabIndex = 5;
            this.lblPassword.Text = "Password:";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblUsername.Location = new System.Drawing.Point(4, 41);
            this.lblUsername.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(136, 29);
            this.lblUsername.TabIndex = 4;
            this.lblUsername.Text = "Username: ";
            // 
            // btnLISignUp
            // 
            this.btnLISignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnLISignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLISignUp.ForeColor = System.Drawing.Color.Blue;
            this.btnLISignUp.Location = new System.Drawing.Point(59, 318);
            this.btnLISignUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnLISignUp.Name = "btnLISignUp";
            this.btnLISignUp.Size = new System.Drawing.Size(328, 31);
            this.btnLISignUp.TabIndex = 3;
            this.btnLISignUp.Text = "Click here to Sign Up";
            this.btnLISignUp.UseVisualStyleBackColor = false;
            this.btnLISignUp.Click += new System.EventHandler(this.btnSignUp_Click);
            // 
            // btnLogIn
            // 
            this.btnLogIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnLogIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnLogIn.ForeColor = System.Drawing.Color.White;
            this.btnLogIn.Location = new System.Drawing.Point(59, 241);
            this.btnLogIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogIn.Name = "btnLogIn";
            this.btnLogIn.Size = new System.Drawing.Size(328, 66);
            this.btnLogIn.TabIndex = 2;
            this.btnLogIn.Text = "Log in";
            this.btnLogIn.UseVisualStyleBackColor = false;
            this.btnLogIn.Click += new System.EventHandler(this.btnLogIn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.label3.Location = new System.Drawing.Point(100, 7);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 54);
            this.label3.TabIndex = 4;
            this.label3.Text = "Login Page";
            // 
            // pnlSignUp
            // 
            this.pnlSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.pnlSignUp.Controls.Add(this.btnSUBack);
            this.pnlSignUp.Controls.Add(this.panel6);
            this.pnlSignUp.Controls.Add(this.label7);
            this.pnlSignUp.Location = new System.Drawing.Point(1204, 245);
            this.pnlSignUp.Margin = new System.Windows.Forms.Padding(4);
            this.pnlSignUp.Name = "pnlSignUp";
            this.pnlSignUp.Size = new System.Drawing.Size(502, 542);
            this.pnlSignUp.TabIndex = 9;
            // 
            // btnSUBack
            // 
            this.btnSUBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSUBack.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnSUBack.BackgroundImage")));
            this.btnSUBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSUBack.Location = new System.Drawing.Point(11, 7);
            this.btnSUBack.Margin = new System.Windows.Forms.Padding(4);
            this.btnSUBack.Name = "btnSUBack";
            this.btnSUBack.Size = new System.Drawing.Size(53, 49);
            this.btnSUBack.TabIndex = 7;
            this.btnSUBack.UseVisualStyleBackColor = false;
            this.btnSUBack.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.panel6.Controls.Add(this.imgPass2);
            this.panel6.Controls.Add(this.lblSUConPassError);
            this.panel6.Controls.Add(this.lblSUPassError);
            this.panel6.Controls.Add(this.lblSUIDError);
            this.panel6.Controls.Add(this.lblSULNError);
            this.panel6.Controls.Add(this.lblSUFNError);
            this.panel6.Controls.Add(this.txtSUID);
            this.panel6.Controls.Add(this.lblSUFirstName);
            this.panel6.Controls.Add(this.txtSUConfirmPass);
            this.panel6.Controls.Add(this.txtSUPassword);
            this.panel6.Controls.Add(this.lblSUConfirmPass);
            this.panel6.Controls.Add(this.lblSUPassword);
            this.panel6.Controls.Add(this.txtSULastName);
            this.panel6.Controls.Add(this.txtSUFirstName);
            this.panel6.Controls.Add(this.lblSULastName);
            this.panel6.Controls.Add(this.lblSUID);
            this.panel6.Controls.Add(this.btnSULogIn);
            this.panel6.Controls.Add(this.btnSignUp);
            this.panel6.Location = new System.Drawing.Point(11, 60);
            this.panel6.Margin = new System.Windows.Forms.Padding(4);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(484, 464);
            this.panel6.TabIndex = 5;
            // 
            // imgPass2
            // 
            this.imgPass2.Image = global::LockedIn.Properties.Resources.view;
            this.imgPass2.Location = new System.Drawing.Point(435, 216);
            this.imgPass2.Name = "imgPass2";
            this.imgPass2.Size = new System.Drawing.Size(43, 32);
            this.imgPass2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgPass2.TabIndex = 36;
            this.imgPass2.TabStop = false;
            this.imgPass2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.imgPass2_MouseDown);
            this.imgPass2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.imgPass2_MouseUp);
            // 
            // lblSUConPassError
            // 
            this.lblSUConPassError.AutoSize = true;
            this.lblSUConPassError.ForeColor = System.Drawing.Color.Red;
            this.lblSUConPassError.Location = new System.Drawing.Point(224, 316);
            this.lblSUConPassError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUConPassError.Name = "lblSUConPassError";
            this.lblSUConPassError.Size = new System.Drawing.Size(0, 16);
            this.lblSUConPassError.TabIndex = 19;
            // 
            // lblSUPassError
            // 
            this.lblSUPassError.AutoSize = true;
            this.lblSUPassError.ForeColor = System.Drawing.Color.Red;
            this.lblSUPassError.Location = new System.Drawing.Point(224, 257);
            this.lblSUPassError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUPassError.Name = "lblSUPassError";
            this.lblSUPassError.Size = new System.Drawing.Size(0, 16);
            this.lblSUPassError.TabIndex = 18;
            // 
            // lblSUIDError
            // 
            this.lblSUIDError.AutoSize = true;
            this.lblSUIDError.ForeColor = System.Drawing.Color.Red;
            this.lblSUIDError.Location = new System.Drawing.Point(224, 191);
            this.lblSUIDError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUIDError.Name = "lblSUIDError";
            this.lblSUIDError.Size = new System.Drawing.Size(0, 16);
            this.lblSUIDError.TabIndex = 17;
            // 
            // lblSULNError
            // 
            this.lblSULNError.AutoSize = true;
            this.lblSULNError.ForeColor = System.Drawing.Color.Red;
            this.lblSULNError.Location = new System.Drawing.Point(224, 129);
            this.lblSULNError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSULNError.Name = "lblSULNError";
            this.lblSULNError.Size = new System.Drawing.Size(0, 16);
            this.lblSULNError.TabIndex = 16;
            // 
            // lblSUFNError
            // 
            this.lblSUFNError.AutoSize = true;
            this.lblSUFNError.ForeColor = System.Drawing.Color.Red;
            this.lblSUFNError.Location = new System.Drawing.Point(224, 69);
            this.lblSUFNError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUFNError.Name = "lblSUFNError";
            this.lblSUFNError.Size = new System.Drawing.Size(0, 16);
            this.lblSUFNError.TabIndex = 15;
            // 
            // txtSUID
            // 
            this.txtSUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtSUID.Location = new System.Drawing.Point(228, 151);
            this.txtSUID.Margin = new System.Windows.Forms.Padding(4);
            this.txtSUID.Name = "txtSUID";
            this.txtSUID.Size = new System.Drawing.Size(200, 34);
            this.txtSUID.TabIndex = 8;
            this.txtSUID.TextChanged += new System.EventHandler(this.txtSUID_TextChanged);
            // 
            // lblSUFirstName
            // 
            this.lblSUFirstName.AutoSize = true;
            this.lblSUFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblSUFirstName.Location = new System.Drawing.Point(88, 26);
            this.lblSUFirstName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUFirstName.Name = "lblSUFirstName";
            this.lblSUFirstName.Size = new System.Drawing.Size(137, 29);
            this.lblSUFirstName.TabIndex = 14;
            this.lblSUFirstName.Text = "First Name:";
            // 
            // txtSUConfirmPass
            // 
            this.txtSUConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtSUConfirmPass.Location = new System.Drawing.Point(228, 277);
            this.txtSUConfirmPass.Margin = new System.Windows.Forms.Padding(4);
            this.txtSUConfirmPass.Name = "txtSUConfirmPass";
            this.txtSUConfirmPass.Size = new System.Drawing.Size(200, 34);
            this.txtSUConfirmPass.TabIndex = 10;
            this.txtSUConfirmPass.UseSystemPasswordChar = true;
            this.txtSUConfirmPass.TextChanged += new System.EventHandler(this.txtSUConfirmPass_TextChanged);
            // 
            // txtSUPassword
            // 
            this.txtSUPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtSUPassword.Location = new System.Drawing.Point(228, 214);
            this.txtSUPassword.Margin = new System.Windows.Forms.Padding(4);
            this.txtSUPassword.Name = "txtSUPassword";
            this.txtSUPassword.Size = new System.Drawing.Size(200, 34);
            this.txtSUPassword.TabIndex = 9;
            this.txtSUPassword.UseSystemPasswordChar = true;
            this.txtSUPassword.TextChanged += new System.EventHandler(this.txtSUPassword_TextChanged);
            // 
            // lblSUConfirmPass
            // 
            this.lblSUConfirmPass.AutoSize = true;
            this.lblSUConfirmPass.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblSUConfirmPass.Location = new System.Drawing.Point(7, 281);
            this.lblSUConfirmPass.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUConfirmPass.Name = "lblSUConfirmPass";
            this.lblSUConfirmPass.Size = new System.Drawing.Size(216, 29);
            this.lblSUConfirmPass.TabIndex = 11;
            this.lblSUConfirmPass.Text = "Confirm Password:";
            // 
            // lblSUPassword
            // 
            this.lblSUPassword.AutoSize = true;
            this.lblSUPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblSUPassword.Location = new System.Drawing.Point(100, 218);
            this.lblSUPassword.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUPassword.Name = "lblSUPassword";
            this.lblSUPassword.Size = new System.Drawing.Size(126, 29);
            this.lblSUPassword.TabIndex = 10;
            this.lblSUPassword.Text = "Password:";
            // 
            // txtSULastName
            // 
            this.txtSULastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtSULastName.Location = new System.Drawing.Point(228, 90);
            this.txtSULastName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSULastName.Name = "txtSULastName";
            this.txtSULastName.Size = new System.Drawing.Size(200, 34);
            this.txtSULastName.TabIndex = 7;
            this.txtSULastName.TextChanged += new System.EventHandler(this.txtSULastName_TextChanged);
            // 
            // txtSUFirstName
            // 
            this.txtSUFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.txtSUFirstName.Location = new System.Drawing.Point(228, 26);
            this.txtSUFirstName.Margin = new System.Windows.Forms.Padding(4);
            this.txtSUFirstName.Name = "txtSUFirstName";
            this.txtSUFirstName.Size = new System.Drawing.Size(200, 34);
            this.txtSUFirstName.TabIndex = 6;
            this.txtSUFirstName.TextChanged += new System.EventHandler(this.txtSUFirstName_TextChanged);
            // 
            // lblSULastName
            // 
            this.lblSULastName.AutoSize = true;
            this.lblSULastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblSULastName.Location = new System.Drawing.Point(91, 90);
            this.lblSULastName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSULastName.Name = "lblSULastName";
            this.lblSULastName.Size = new System.Drawing.Size(134, 29);
            this.lblSULastName.TabIndex = 5;
            this.lblSULastName.Text = "Last Name:";
            // 
            // lblSUID
            // 
            this.lblSUID.AutoSize = true;
            this.lblSUID.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.lblSUID.Location = new System.Drawing.Point(129, 159);
            this.lblSUID.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSUID.Name = "lblSUID";
            this.lblSUID.Size = new System.Drawing.Size(96, 29);
            this.lblSUID.TabIndex = 4;
            this.lblSUID.Text = "RSA ID:";
            // 
            // btnSULogIn
            // 
            this.btnSULogIn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.btnSULogIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSULogIn.ForeColor = System.Drawing.Color.Blue;
            this.btnSULogIn.Location = new System.Drawing.Point(59, 414);
            this.btnSULogIn.Margin = new System.Windows.Forms.Padding(4);
            this.btnSULogIn.Name = "btnSULogIn";
            this.btnSULogIn.Size = new System.Drawing.Size(328, 31);
            this.btnSULogIn.TabIndex = 3;
            this.btnSULogIn.Text = "Click here to Log In";
            this.btnSULogIn.UseVisualStyleBackColor = false;
            this.btnSULogIn.Click += new System.EventHandler(this.button7_Click);
            // 
            // btnSignUp
            // 
            this.btnSignUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.btnSignUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F);
            this.btnSignUp.ForeColor = System.Drawing.Color.White;
            this.btnSignUp.Location = new System.Drawing.Point(59, 336);
            this.btnSignUp.Margin = new System.Windows.Forms.Padding(4);
            this.btnSignUp.Name = "btnSignUp";
            this.btnSignUp.Size = new System.Drawing.Size(328, 66);
            this.btnSignUp.TabIndex = 2;
            this.btnSignUp.Text = "Sign Up";
            this.btnSignUp.UseVisualStyleBackColor = false;
            this.btnSignUp.Click += new System.EventHandler(this.btnSignUp_Click_1);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(71)))), ((int)(((byte)(3)))));
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.label7.Location = new System.Drawing.Point(100, 7);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(309, 54);
            this.label7.TabIndex = 4;
            this.label7.Text = "Sign Up Page";
            // 
            // frmLogIn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.ClientSize = new System.Drawing.Size(1747, 897);
            this.Controls.Add(this.pnlSignUp);
            this.Controls.Add(this.pnlLogIn);
            this.Controls.Add(this.pnlAPESelect);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "frmLogIn";
            this.ShowInTaskbar = false;
            this.Text = "LockedIn.exe";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnlAPESelect.ResumeLayout(false);
            this.pnlAPESelect.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.pnlLogIn.ResumeLayout(false);
            this.pnlLogIn.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPass)).EndInit();
            this.pnlSignUp.ResumeLayout(false);
            this.pnlSignUp.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imgPass2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnlAPESelect;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnEmployer;
        private System.Windows.Forms.Button btnAdministrator;
        private System.Windows.Forms.Button btnPrisoner;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnlLogIn;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnLISignUp;
        private System.Windows.Forms.Button btnLogIn;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Panel pnlSignUp;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox txtSULastName;
        private System.Windows.Forms.TextBox txtSUFirstName;
        private System.Windows.Forms.Label lblSULastName;
        private System.Windows.Forms.Label lblSUID;
        private System.Windows.Forms.Button btnSULogIn;
        private System.Windows.Forms.Button btnSignUp;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSUID;
        private System.Windows.Forms.Label lblSUFirstName;
        private System.Windows.Forms.TextBox txtSUConfirmPass;
        private System.Windows.Forms.TextBox txtSUPassword;
        private System.Windows.Forms.Label lblSUConfirmPass;
        private System.Windows.Forms.Label lblSUPassword;
        private System.Windows.Forms.Button btnLIBack;
        private System.Windows.Forms.Button btnSUBack;
        private System.Windows.Forms.Label lblEmployerID;
        private System.Windows.Forms.ComboBox cbxEmployerID;
        private System.Windows.Forms.Label lblSUConPassError;
        private System.Windows.Forms.Label lblSUPassError;
        private System.Windows.Forms.Label lblSUIDError;
        private System.Windows.Forms.Label lblSULNError;
        private System.Windows.Forms.Label lblSUFNError;
        private System.Windows.Forms.PictureBox imgPass;
        private System.Windows.Forms.PictureBox imgPass2;
    }
}

