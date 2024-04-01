using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace newestDBSYS
{
    public partial class adminMainPage : Form
    {
        private string connectionString = @"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True";

        public adminMainPage()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM User1";

                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;
            }
        }

        private void adminMainPage_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        private void btnEdit_Click_1(object sender, EventArgs e)
        {


        }


        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this row?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string primaryKeyValue = dataGridView1.SelectedRows[0].Cells["CustomerID"].Value.ToString();

                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = "DELETE FROM User1 WHERE CustomerID = @CustomerID";
                            SqlCommand command = new SqlCommand(query, connection);
                            command.Parameters.AddWithValue("@CustomerID", primaryKeyValue);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Row deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Failed to delete row.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnproduct_Click(object sender, EventArgs e)
        {
            adminProductView product = new adminProductView();
            product.Show();
            this.Hide();
        }
    }
}




