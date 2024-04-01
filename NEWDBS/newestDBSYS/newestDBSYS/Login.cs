using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace newestDBSYS
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private string HashPassword(string password)
        {
            // Hash the password using SHA-256 algorithm
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

        private void button2_Click(object sender, EventArgs e)
        {
            Register form2 = new Register();
            form2.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = tbloginuser.Text;
            string password = tbloginpass.Text;

            // Your connection string
            string connectionString = @"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True";

            // SQL query to retrieve hashed password for the provided username
            string query = "SELECT Password FROM [User1] WHERE Username = @Username";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    // Retrieve the hashed password from the database
                    string hashedPassword = command.ExecuteScalar() as string;

                    if (hashedPassword != null)
                    {
                        // Hash the password entered by the user
                        string hashedInputPassword = HashPassword(password);

                        // Check if the hashed password matches the hashed password stored in the database
                        if (hashedPassword.Equals(hashedInputPassword, StringComparison.OrdinalIgnoreCase))
                        {
                            // Login successful
                            MainPage form5 = new MainPage();
                            form5.Show();
                            this.Hide();
                        }
                        else
                        {
                            // Login failed
                            MessageBox.Show("Invalid username or password. Please try again.");
                        }
                    }
                    else
                    {
                        // Username not found
                        MessageBox.Show("Invalid username or password. Please try again.");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string adminUsername = tbloginuser.Text;
            string adminPassword = tbloginpass.Text;

            // Your connection string
            string connectionString = @"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True";

            // SQL query to retrieve admin password for the provided admin username
            string query = "SELECT adminPass FROM admin WHERE adminUser = @AdminUser";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@AdminUser", adminUsername);

                    // Retrieve the admin password from the database
                    string storedAdminPassword = command.ExecuteScalar() as string;

                    if (storedAdminPassword != null && storedAdminPassword == adminPassword)
                    {
                        // Admin login successful
                        MessageBox.Show("Admin login successful.");

                        // Proceed to admin main page
                        adminMainPage adminMainPage = new adminMainPage();
                        adminMainPage.Show();

                        // Hide the login form
                        this.Hide();
                    }
                    else
                    {
                        // Admin login failed
                        MessageBox.Show("Invalid admin username or password. Please try again.");
                    }
                }
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }

}


