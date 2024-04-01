using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace newestDBSYS
{
    public partial class EditUser : Form
    {
        private int userId;
        private string connectionString = @"Data Source=DESKTOP-V1NDQ64\SQLEXPRESS;Initial Catalog=newDBSYS;Integrated Security=True";

        public EditUser(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserData();
        }

        private void LoadUserData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM User1 WHERE UserId = @UserId";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserId", userId);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    tbeditfirstname.Text = reader["FirstName"].ToString();
                    tbeditlastname.Text = reader["Lastname"].ToString();
                    tbeditaddress.Text = reader["Address"].ToString();
                    tbedituser.Text = reader["Username"].ToString();
                    tbeditpassword.Text = reader["Password"].ToString();
                    // Add more fields as needed
                }
                reader.Close();
            }
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            SaveUserData();
        }

        private void SaveUserData()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "UPDATE User1 SET FirstName = @FirstName, Lastname = @Lastname, Address = @Address, Username = @Username, Password = @Password WHERE UserId = @UserId";

                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@FirstName", tbeditfirstname.Text);
                command.Parameters.AddWithValue("@Lastname", tbeditlastname.Text);
                command.Parameters.AddWithValue("@Address", tbeditaddress.Text);
                command.Parameters.AddWithValue("@Username", tbedituser.Text);
                command.Parameters.AddWithValue("@Password", tbeditpassword.Text);
                

                connection.Open();
                command.ExecuteNonQuery();
            }

            MessageBox.Show("User information updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            this.Close();
        }
    }
}
