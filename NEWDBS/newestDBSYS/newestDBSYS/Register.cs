using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace newestDBSYS
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private string GenerateCustomerID(SqlConnection conn)
        {
            SqlCommand cmd = new SqlCommand("SELECT MAX(CustomerID) FROM [User1]", conn);
            string maxID = cmd.ExecuteScalar() as string;
            if (maxID == null)
            {
                // If no existing IDs, start with C-001
                return "C-001";
            }
            else
            {
                // Extract the numeric part of the ID
                int numericPart = int.Parse(maxID.Substring(2)); // Assuming format C-XXX
                numericPart++; // Increment the numeric part
                               // Format the new ID
                return "C-" + numericPart.ToString("000");
            }
        }

        private bool IsAnyFieldEmpty()
        {
            return string.IsNullOrEmpty(tbuser.Text) || string.IsNullOrEmpty(tblastname.Text) ||
                   string.IsNullOrEmpty(tbfirstname.Text) || string.IsNullOrEmpty(tbaddress.Text) ||
                   string.IsNullOrEmpty(tbpassword.Text) || string.IsNullOrEmpty(tbconfirmpassword.Text);
        }

        private bool DoPasswordsMatch()
        {
            return tbpassword.Text == tbconfirmpassword.Text;
        }
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < hashedBytes.Length; i++)
                {
                    builder.Append(hashedBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void RegisterButton_Click(object sender, EventArgs e)
        {
            if (IsAnyFieldEmpty())
            {
                MessageBox.Show("Please fill out all fields before registering.");
                return;
            }

            if (!DoPasswordsMatch())
            {
                MessageBox.Show("Passwords do not match. Please enter matching passwords.");
                return;
            }

            SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True");
            conn.Open();

            // Generate customer ID

            string hashedPassword = HashPassword(tbpassword.Text);
            string customerID = GenerateCustomerID(conn);

            // Insert into database
            SqlCommand cmd = new SqlCommand("INSERT INTO [User1] (CustomerID, Username, Lastname, Firstname, Address, Password) VALUES (@CustomerID, @Username, @Lastname, @Firstname, @Address, @Password)", conn);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            cmd.Parameters.AddWithValue("@Username", tbuser.Text);
            cmd.Parameters.AddWithValue("@Lastname", tblastname.Text);
            cmd.Parameters.AddWithValue("@Firstname", tbfirstname.Text);
            cmd.Parameters.AddWithValue("@Address", tbaddress.Text);
            cmd.Parameters.AddWithValue("@Password", hashedPassword); // Store hashed password
            cmd.ExecuteNonQuery();

            conn.Close();
            MessageBox.Show("Created Successfully!");
            this.Close();
        }

        private void Register_Load(object sender, EventArgs e)
        {

        }
    }
}

