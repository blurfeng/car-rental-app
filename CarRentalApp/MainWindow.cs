using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CarRentalApp;

namespace CarRentalApp
{
    public partial class MainWindow : Form
    {
        private Login _loginForm;
        public readonly Users User;
        public readonly string RoleShortName;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login loginForm, Users user)
        {
            InitializeComponent();
            _loginForm = loginForm;
            User = user;
            RoleShortName = user.UserRoles.FirstOrDefault().Roles.shortName;
        }

        private void addRentalRecordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var addRentalForm = new AddEditRentalRecord();
            addRentalForm.MdiParent = this;

            // ShowDialog 确保窗口打开后只能操作这个窗口，直到它关闭。
            // ShowDialog ensures that the window is modal, meaning the user must interact with it before returning to the main window.
            addRentalForm.ShowDialog();
        }

        private void manageVehicleListingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 确保 ManageVehicleListing 窗体只能打开一个实例。
            // Ensure that only one instance of ManageVehicleListing can be opened.
            
            if (!Utils.FormIsOpen<ManageVehicleListing>())
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen<ManageRentalRecords>())
            {
                var manageRentalRecords = new ManageRentalRecords();
                manageRentalRecords.MdiParent = this;
                manageRentalRecords.Show();
            }
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _loginForm?.Close();
        }

        private void manageUsersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utils.FormIsOpen<ManageUsers>())
            {
                var manageUsersForm = new ManageUsers();
                manageUsersForm.MdiParent = this;
                manageUsersForm.Show();
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            if (User.password == Utils.DefaultHashedPassword())
            {
                var resetPasswordForm = new ResetPassword(User, false);
                resetPasswordForm.ShowDialog();
            }

            var userName = User.username;
            tsiLoginText.Text = $"Logged in as: {userName}";
            if (RoleShortName != "admin")
            {
                // 如果不是管理员角色，则隐藏管理用户菜单项。
                // If the user is not an admin, hide the manage users menu item.
                manageUsersToolStripMenuItem.Visible = false;
            }
        }
    }
}
