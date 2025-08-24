using LockedIn.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
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

        private void ARJcmbSelectApplication_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    if (ARJcmbSelectApplication.SelectedIndex == 1)
                    {
                        return;
                    }
                    else
                    {

                        int prisonerId = Convert.ToInt32(ARJcmbSelectApplication.SelectedValue);
                        string sql = "SELECT PrisonerName, Skill1, Skill2 FROM PRISONER WHERE PrisonerID = @PrisonerID";
                        using (SqlCommand comm = new SqlCommand(sql, conn))
                        {
                            comm.Parameters.AddWithValue("@PrisonerID", prisonerId);

                            SqlDataReader reader = comm.ExecuteReader();
                            if (reader.Read())
                            {
                                ARJtxtPrisonerName.Text = reader["PrisonerName"].ToString();
                                ARJtxtPrisonerSkill1.Text = reader["Skill1"].ToString();
                                ARJtxtPrisonerSkill2.Text = reader["Skill2"].ToString();
                            }

                        }
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading application details: " + ex.Message);
            }
            
          
        }

        private void ARJbtnBack_Click(object sender, EventArgs e)
        {
            tabCont.SelectedIndex = 0;
        }
    }

        



        
}
