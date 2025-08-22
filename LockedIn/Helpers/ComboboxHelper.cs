using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public static class ComboBoxHelper
{
    static String constr = DatabaseConfig.GetConStr();
    static SqlConnection conn;
    static SqlDataAdapter adapter;
    static String query;
    static DataSet ds;


    public static void PopulateEmployerNames(ComboBox comboBox)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                query = "SELECT EmployerID, CONCAT(FirstName, ' ', LastName) AS FullName FROM EMPLOYER";
                adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "EMPLOYER");

                comboBox.DataSource = ds.Tables["EMPLOYER"];
                comboBox.DisplayMember = "FullName";
                comboBox.ValueMember = "EmployerID";
                comboBox.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading employer names: " + ex.Message);
        }
    }

    public static void PopulatePrisonerNumbers(ComboBox comboBox)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                query = "SELECT PrisonerID ,PrisonerNum FROM PRISONER";
                adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "PRISONER");

                comboBox.DataSource = ds.Tables["PRISONER"];
                comboBox.DisplayMember = "PrisonerNum";
                comboBox.ValueMember = "PrisonerID";
                comboBox.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading prisoner numbers: " + ex.Message);
        }
    }
}
