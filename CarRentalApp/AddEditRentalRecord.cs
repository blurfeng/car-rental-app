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
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private readonly CarRentalEntities _db;
        private ManageRentalRecords _manageRentalRecords;

        public AddEditRentalRecord(ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            _manageRentalRecords = manageRentalRecords;
            lblTitle.Text = "Add New Rental";
            this.Text = "Add New Rental";
            isEditMode = false;
            _db = new CarRentalEntities();
        }

        public AddEditRentalRecord(CarRentelRecord recordToEdit, ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            _manageRentalRecords = manageRentalRecords;
            lblTitle.Text = "Edit Rental";
            this.Text = "Edit Rental";

            if (recordToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();

                // Populate the fields with the data from the carToEdit object.
                PopulateFields(recordToEdit);
            }
        }

        private void AddRentalRecord_Load(object sender, EventArgs e)
        {
            //var carTypes = carRentalEntities.TypesOfCars.ToList();

            // Tips: 这里不支持 $"{car.Make} {car.Model}" 的写法。
            // here is not supported $"{car.Make} {car.Model}" syntax.
            var cars = _db.TypesOfCar.Select(
                car => new
                {
                    Id = car.Id,
                    Name = car.Make + " " + car.Model
                }).ToList();

            cbTypeOfCar.DisplayMember = "Name";
            cbTypeOfCar.ValueMember = "Id";
            cbTypeOfCar.DataSource = cars;
        }

        private void PopulateFields(CarRentelRecord record)
        {
            tbCustomerName.Text = record.CustomerName;
            dtRented.Value = (DateTime)record.DateRented;
            dtReturned.Value = (DateTime)record.DateReturned;
            tbCost.Text = record.Cost.ToString();
            lblRecordId.Text = record.id.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateOut = dtRented.Value;
                var dateIn = dtReturned.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbTypeOfCar.Text;
                bool isValid = true;
                string errorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(customerName))
                {
                    isValid = false;
                    errorMessage += "Please enter a valid customer name.\n\r";
                }
                
                if (dateIn < dateOut)
                {
                    isValid = false;
                    errorMessage += "The return date cannot be earlier than the rental date.\n\r";
                }

                if (string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Please select a valid car type.\n\r";
                }

                if (isValid)
                {
                    // 创建一个新的租赁记录对象。
                    // Create a new rental record object.
                    var rentalRecord = new CarRentelRecord();
                    if (isEditMode)
                    {
                        // 如果处于编辑模式，则使用数据库中的现有记录。
                        // If in edit mode, update the existing record.
                        int id = Convert.ToInt32(lblRecordId.Text);
                        rentalRecord = _db.CarRentelRecord.FirstOrDefault(q => q.id == id);
                    }

                    // 使用输入的值更新租赁记录。
                    // Update the rental record with the input values.
                    rentalRecord.CustomerName = customerName;
                    rentalRecord.DateRented = dateOut;
                    rentalRecord.DateReturned = dateIn;
                    rentalRecord.Cost = (decimal)cost;
                    rentalRecord.TypeOfCarId = (int)cbTypeOfCar.SelectedValue;

                    if (!isEditMode)
                    {
                        // 如果不是编辑模式，则添加新记录。
                        // If not in edit mode, add a new record.
                        _db.CarRentelRecord.Add(rentalRecord);
                    }

                    _db.SaveChanges();

                    MessageBox.Show(
                        $"Customer Name: {customerName}\n\r"
                        + $"Date Rented: {dateOut}\n\r"
                        + $"Date Returned: {dateIn}\n\r"
                        + $"Cost: {cost}\n\r"
                        + $"Car Type: {carType}\n\r"
                        + "Thank you for your business.");

                    _manageRentalRecords?.PopulateGrid();
                    Close();
                }
                else
                {
                    MessageBox.Show(errorMessage, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
