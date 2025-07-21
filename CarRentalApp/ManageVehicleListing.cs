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
    /// <summary>
    /// 管理车辆列表的窗体。
    /// manage vehicle listing form.
    /// </summary>
    public partial class ManageVehicleListing : Form
    {
        private readonly CarRentalEntities _db;

        public ManageVehicleListing()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        /// <summary>
        /// 打开添加车辆的窗体。
        /// Launch the form to add a new vehicle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCar_Click(object sender, EventArgs e)
        {
            var addEditVehicleForm = new AddEditVehicle(this);
            // 设置窗体的 MDI 父窗体为当前 MDI 父窗体。MDI用于多文档界面（Multiple Document Interface），允许在一个主窗口中打开多个子窗口。
            // Set the MDI parent of the form to the current MDI parent.
            addEditVehicleForm.MdiParent = this.MdiParent;
            addEditVehicleForm.Show();
        }

        /// <summary>
        /// 打开编辑车辆的窗体。
        /// Launch the form to edit the selected vehicle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中行的 Id。
                // get Id of selected row.
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                // 查询数据库以获取记录。
                // query database for record.
                var car = _db.TypesOfCar.FirstOrDefault(q => q.Id == id);

                // 打开 AddEditVehicle 窗体，并传入车辆数据。
                // launch AddEditVehicle window with data.
                var addEditVehicleForm = new AddEditVehicle(this, car);
                addEditVehicleForm.MdiParent = this.MdiParent;
                addEditVehicleForm.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        /// <summary>
        /// 删除选中的车辆。
        /// Delete the selected vehicle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;
                var car = _db.TypesOfCar.FirstOrDefault(q => q.Id == id);

                // 从数据库中删除车辆记录。
                // delete vehicle from table.
                _db.TypesOfCar.Remove(car);
                _db.SaveChanges();

                // 更新车辆列表。
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        /// <summary>
        /// 刷新车辆列表。
        /// Refresh the vehicle listing. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            PopulateGrid();
        }

        /// <summary>
        /// 更新车辆列表的显示。
        /// </summary>
        public void PopulateGrid()
        {
            // 从数据库中选择需要的车辆信息。
            // Select a custom model collection of cars from database
            var cars = _db.TypesOfCar
                .Select(q => new
                {
                    Make = q.Make,
                    Model = q.Model,
                    VIN = q.VIN,
                    Year = q.Year,
                    LicensePlateNumber = q.LicensePlateNumber,
                    q.Id
                })
                .ToList();

            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[4].HeaderText = "License Plate Number";

            // Hide the column for ID. Changed from the hard coded column value to the name, to make it more dynamic. 
            gvVehicleList.Columns["Id"].Visible = false;
        }
    }
}
