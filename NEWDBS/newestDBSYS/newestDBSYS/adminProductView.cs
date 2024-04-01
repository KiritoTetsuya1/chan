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

namespace newestDBSYS
{
    public partial class adminProductView : Form
    {
        private int productIdCounter = 1; // Initial counter value
        private string connectionString = @"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True";
        public adminProductView()
        {
            InitializeComponent();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
                // Generate ProductID
                string productId = GenerateProductId();

                if (productId == null)
                {
                    MessageBox.Show("Failed to generate ProductID.");
                    return;
                }

                // Get values from textboxes
                string productBrand = txtbrand.Text;
                string productModel = txtmodel.Text;
                decimal productPrice;

                // Attempt to parse the price to decimal
                if (!decimal.TryParse(txtprice.Text, out productPrice))
                {
                    MessageBox.Show("Invalid price format.");
                    return;
                }

                // Save to database
                if (SaveToDatabase(productId, productBrand, productModel, productPrice))
                {
                    MessageBox.Show("Product saved successfully.");
                }
                else
                {
                    MessageBox.Show("Failed to save product.");
                }
            }
        private string GenerateProductId()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to get the maximum ProductID from the database
                    string query = "SELECT MAX(ProductID) FROM Product";

                    // Create command
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Execute command and get the result
                        object result = command.ExecuteScalar();

                        // If there are no existing ProductIDs, start with P-001
                        if (result == null || result == DBNull.Value)
                        {
                            return "P-001";
                        }

                        // Extract the numeric part of the highest ProductID
                        string maxProductId = result.ToString();
                        string numericPart = maxProductId.Substring(2); // Exclude the "P-"
                        int numericValue = int.Parse(numericPart);

                        // Increment the numeric part and format it back to P-001 format
                        return "P-" + (numericValue + 1).ToString("D3");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error generating ProductID: " + ex.Message);
                return null;
            }
        }

        private bool SaveToDatabase(string productId, string productBrand, string productModel, decimal productPrice)
        {
            try
            {
                // Establish connection
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL command to insert values into the database
                    string query = "INSERT INTO Product (ProductID, ProductBrand, ProductModel, ProductPrice) VALUES (@ProductID, @ProductBrand, @ProductModel, @ProductPrice)";

                    // Create command
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Add parameters
                        command.Parameters.AddWithValue("@ProductID", productId);
                        command.Parameters.AddWithValue("@ProductBrand", productBrand);
                        command.Parameters.AddWithValue("@ProductModel", productModel);
                        command.Parameters.AddWithValue("@ProductPrice", productPrice);

                        // Execute command
                        command.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                return false;
            }
        }

        private void adminProductView_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'newDBSYSDataSet2.Product' table. You can move, or remove it, as needed.
            this.productTableAdapter.Fill(this.newDBSYSDataSet2.Product);

        }
    }

}
