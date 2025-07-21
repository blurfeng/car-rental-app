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
    public partial class ManageUsers : Form
    {
        private readonly CarRentalEntities _db;

        public ManageUsers()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen<AddUser>())
            {
                var addUserForm = new AddUser(this);
                addUserForm.MdiParent = this.MdiParent;
                addUserForm.Show();
            }
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvUserList.SelectedRows[0].Cells["Id"].Value;
                var user = _db.Users.FirstOrDefault(u => u.id == id);

                var hashedPassword = Utils.DefaultHashedPassword();

                user.password = hashedPassword;
                _db.SaveChanges();

                MessageBox.Show($"Password for user {user.username} has been reset.", "Password Reset", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        private void btnSwitchActive_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvUserList.SelectedRows[0].Cells["Id"].Value;
                var user = _db.Users.FirstOrDefault(u => u.id == id);

                user.isActive = user.isActive == true ? false : true;
                _db.SaveChanges();
                PopulateGrid();

                MessageBox.Show($"User {user.username} is now {(user.isActive == true ? "active" : "inactive")}.", "User Status Changed", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        private void ManageUsers_Load(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        public void PopulateGrid()
        {
            var users = _db.Users.Select(q => new
            {
                Id = q.id,
                Username = q.username,
                Role = q.UserRoles.FirstOrDefault().Roles.name,
                IsActive = q.isActive,
            }).ToList();

            gvUserList.DataSource = users;
            gvUserList.Columns["Id"].Visible = false;
            gvUserList.Columns["Username"].HeaderText = "User Name";
            gvUserList.Columns["Role"].HeaderText = "Role Name";
            gvUserList.Columns["IsActive"].HeaderText = "Active";
        }
    }
}
