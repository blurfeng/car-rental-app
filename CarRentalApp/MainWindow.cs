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
    public partial class MainWindow : Form
    {
        private Login _loginForm;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Login loginForm)
        {
            InitializeComponent();
            _loginForm = loginForm;
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
            var openForms = Application.OpenForms.Cast<Form>();
            bool isOpen = openForms.Any(form => form is ManageVehicleListing);
            if (!isOpen)
            {
                var vehicleListing = new ManageVehicleListing();
                vehicleListing.MdiParent = this;
                vehicleListing.Show();
            }
        }

        private void viewArchiveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var manageRentalRecords = new ManageRentalRecords();
            manageRentalRecords.MdiParent = this;
            manageRentalRecords.Show();
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            _loginForm?.Close();
        }
    }
}
