using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities _db;

        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                var userName = tbUserName.Text.Trim();
                var password = tbPassword.Text;

                SHA256 sha = SHA256.Create();

                // 转换输入字符串为字节数组并计算哈希值。
                // Convert the input string to a byte array and compute the hash.
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

                // 将字节数组转换为十六进制字符串。并添加到 StringBuilder 中。
                // Convert the byte array to a hexadecimal string and append it to StringBuilder.
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    sb.Append(bytes[i].ToString("x2"));
                }
                var hashedPassword = sb.ToString();

                var user = _db.Users.FirstOrDefault(u => u.username == userName && u.password == hashedPassword);

                if (user != null)
                {
                    var mainWindow = new MainWindow(this);
                    mainWindow.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }
    }
}
