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
    public partial class AddUser : Form
    {
        private readonly CarRentalEntities _db;
        private readonly ManageUsers _manageUsers;

        public AddUser(ManageUsers manageUsers)
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            _manageUsers = manageUsers;
        }

        private void AddUser_Load(object sender, EventArgs e)
        {
            var roles = _db.Roles.ToList();
            cbRole.DataSource = roles;
            cbRole.ValueMember = "id";
            cbRole.DisplayMember = "name";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                var username = tbUserName.Text;
                var RoleId = (int)cbRole.SelectedValue;
                var password = Utils.DefaultHashedPassword();

                var user = new Users
                {
                    username = username,
                    password = password,
                    isActive = true
                };

                _db.Users.Add(user);
                _db.SaveChanges();

                var userRole = new UserRoles
                {
                    userId = user.id,
                    roleId = RoleId
                };

                _db.UserRoles.Add(userRole);
                _db.SaveChanges();
                _manageUsers.PopulateGrid();

                MessageBox.Show($"User {username} has been added successfully.", "User Added", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
