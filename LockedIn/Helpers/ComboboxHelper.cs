using LockedIn.Data;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

public static class ComboBoxHelper
{
    static String constr = DatabaseConfig.GetConStr();
    static SqlDataAdapter adapter;
    static String query;

    //Methods to fill Comboboxes
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

    public static void PopulateSkillNames(ComboBox comboBox)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                query = "SELECT SkillID ,SkillName FROM SKILL";
                adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "SKILL");

                comboBox.DataSource = ds.Tables["SKILL"];
                comboBox.DisplayMember = "SkillName";
                comboBox.ValueMember = "SkillID";
                comboBox.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading skill names: " + ex.Message);
        }
    }

    public static void PopulateJobNames(ComboBox comboBox)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                query = "SELECT JobID ,JobName FROM JOB";
                adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "JOB");

                comboBox.DataSource = ds.Tables["JOB"];
                comboBox.DisplayMember = "JobName";
                comboBox.ValueMember = "JobID";
                
                
                comboBox.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading job names: " + ex.Message);
        }
    }

    public static void PopulateUncompletedJobNames(ComboBox comboBox)
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(constr))
            {
                conn.Open();

                query = "SELECT JobID ,JobName FROM JOB WHERE Completed = 0";
                adapter = new SqlDataAdapter(query, conn);
                DataSet ds = new DataSet();

                adapter.Fill(ds, "JOB");

                comboBox.DataSource = ds.Tables["JOB"];
                comboBox.DisplayMember = "JobName";
                comboBox.ValueMember = "JobID";


                comboBox.SelectedIndex = -1;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error loading job names: " + ex.Message);
        }
    }

    //Methods to retrieve the ID of selected item

    public static int GetSelectedID(ComboBox comboBox)
    {        
        if (comboBox.SelectedItem is DataRowView selectedRow)
        {
            object value = selectedRow[comboBox.ValueMember];
            if (value == null || value == DBNull.Value)
                return -1; 

            return Convert.ToInt32(value);
        }

        // Fallback
        return comboBox.SelectedValue != null ? Convert.ToInt32(comboBox.SelectedValue) : -1;
    }



}