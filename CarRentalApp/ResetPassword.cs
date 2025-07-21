using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ResetPassword : Form
    {
        private readonly CarRentalEntities _db;
        private readonly Users _user;

        public ResetPassword(Users user, bool showCancel = true)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _user = user;
            btnCancel.Visible = showCancel;
        }

        private void ResetPassword_Load(object sender, EventArgs e)
        {

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                var password = tbP1.Text;
                var confirmPassword = tbP2.Text;

                if (password != confirmPassword)
                {
                    MessageBox.Show("Passwords do not match.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tbP1.Text = string.Empty;
                    tbP2.Text = string.Empty;
                    return;
                }

                var user = _db.Users.FirstOrDefault(u => u.id == _user.id);
                user.password = Utils.HashPassword(password);
                _db.SaveChanges();

                MessageBox.Show($"Password for user {user.username} has been reset successfully.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
