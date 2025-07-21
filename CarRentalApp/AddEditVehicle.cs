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
    /// 添加或编辑车辆的窗体。
    /// </summary>
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db; 
        private ManageVehicleListing _manageVehicleListing;

        /// <summary>
        /// 初始化添加新车辆的窗体。
        /// </summary>
        /// <param name="manageVehicleListing"></param>
        public AddEditVehicle(ManageVehicleListing manageVehicleListing)
        {
            InitializeComponent();
            _manageVehicleListing = manageVehicleListing;
            lblTitle.Text = "Add New Vehicle";
            this.Text = "Add New Vehicle";
            isEditMode = false;
            _db = new CarRentalEntities();

            lblId.Text = "";
        }

        /// <summary>
        /// 初始化编辑车辆的窗体。
        /// </summary>
        /// <param name="manageVehicleListing"></param>
        /// <param name="carToEdit"></param>
        public AddEditVehicle(ManageVehicleListing manageVehicleListing, TypesOfCar carToEdit)
        {
            InitializeComponent();
            _manageVehicleListing = manageVehicleListing;
            lblTitle.Text = "Edit Vehicle";
            this.Text = "Edit Vehicle";

            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();

                // Populate the fields with the data from the carToEdit object.
                PopulateFields(carToEdit);
            }
        }

        /// <summary>
        /// 设置车辆信息到窗体的字段中。
        /// Populates the form fields with the data from the TypesOfCar object.
        /// </summary>
        /// <param name="car"></param>
        private void PopulateFields(TypesOfCar car)
        {
            lblId.Text = car.Id.ToString();
            tbMake.Text = car.Make;
            tbModel.Text = car.Model;
            tbVIN.Text = car.VIN;
            tbYear.Text = car.Year.ToString();
            tbLicensePlateNumber.Text = car.LicensePlateNumber;
        }

        /// <summary>
        /// 保存车辆信息。
        /// Saves the vehicle information to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (isEditMode)
                {
                    var id = int.Parse(lblId.Text);
                    var car = _db.TypesOfCar.FirstOrDefault(q => q.Id == id);
                    car.Make = tbMake.Text;
                    car.Model = tbModel.Text;
                    car.VIN = tbVIN.Text;
                    car.Year = int.Parse(tbYear.Text);
                    car.LicensePlateNumber = tbLicensePlateNumber.Text;

                    _db.SaveChanges();

                    MessageBox.Show("Vehicle details updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    var newCar = new TypesOfCar
                    {
                        Make = tbMake.Text,
                        Model = tbModel.Text,
                        VIN = tbVIN.Text,
                        Year = int.Parse(tbYear.Text),
                        LicensePlateNumber = tbLicensePlateNumber.Text
                    };

                    _db.TypesOfCar.Add(newCar);
                    _db.SaveChanges();

                    MessageBox.Show("New vehicle added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // 更新管理车辆列表的窗体的车辆列表。
                // Refresh the vehicle listing in the ManageVehicleListing form.
                _manageVehicleListing.PopulateGrid();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
