using LockedIn.Data;
using LockedIn.Helpers;
using LockedIn.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LockedIn
{
    
    public partial class frmEmployer : Form
    {
        static String constr = DatabaseConfig.GetConStr();
        static SqlConnection conn;
        static SqlDataAdapter adap;
        static String sql;
        static DataSet ds;
        public frmEmployer()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabCont_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {

        }

        private void ARJcmbJobName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage4_Click(object sender, EventArgs e)
        {

        }

        private void frmEmployer_Load(object sender, EventArgs e)
        {
            ComboBoxHelper.PopulateJobNames(ARJcmbJobName);
            ComboBoxHelper.PopulatePrisonerNumbers(ARJcmbSelectApplication);

            
        }

        private void PopulateSkillsHelper(ListBox ARJlsbCurrentSkills, int prisonerID)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    string query = @"SELECT SkillId, SkillName FROM SKILL WHERE SkillId IN (SELECT SkillId FROM SKILL_OF_PRISONER WHERE PrisonerID = @PrisonerID) ORDER BY SkillName";
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@PrisonerID", prisonerID);

                        SqlDataAdapter adapter = new SqlDataAdapter(comm);
                        DataSet ds = new DataSet();
                        adapter.Fill(ds, "SKILL_OF_PRISONER");      //SKILLS_OF_PRISONER >> SKILL_OF_PRISONER

                        ARJlsbCurrentSkills.DataSource = ds.Tables["SKILL_OF_PRISONER"];        //SKILLS_OF_PRISONER >> SKILL_OF_PRISONER
                        ARJlsbCurrentSkills.ValueMember = "SkillId";
                        ARJlsbCurrentSkills.DisplayMember = "SkillName";
                        

                        conn.Close();

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }   

        private void ARJcmbSelectApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (ARJcmbSelectApplication.SelectedValue == null)
            {
                return;
            }

            try
            {
                using(SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    int prisonerId = Convert.ToInt32(ARJcmbSelectApplication.SelectedValue);
                    string sql = "SELECT FirstName FROM PRISONER WHERE PrisonerID = @PrisonerID";
                    using (SqlCommand comm = new SqlCommand(sql, conn))
                    {
                        comm.Parameters.AddWithValue("@PrisonerID", prisonerId);
                        SqlDataReader reader = comm.ExecuteReader();
                        if (reader.Read())
                        {
                            ARJtxtPrisonerName.Text = reader["FirstName"].ToString();
                            ListboxHelper.GetSelectedSkillID(ARJlsbCurrentSkills);
                        }
                    }

                    PopulateSkillsHelper(ARJlsbCurrentSkills, prisonerId);
                    conn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }
          
        

        private void ARJbtnBack_Click(object sender, EventArgs e)
        {
            tabCont.SelectedIndex = 0;
        }

        private void ARJlsbCurrentSkills_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ARJpbl1_Paint(object sender, PaintEventArgs e)
        {

        }
    }

        



        
}
