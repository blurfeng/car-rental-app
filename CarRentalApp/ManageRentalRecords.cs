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
    public partial class ManageRentalRecords : Form
    {
        private readonly CarRentalEntities _db;

        public ManageRentalRecords()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
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

        public void PopulateGrid()
        {
            var records = _db.CarRentelRecord.Select(q => new
            {
                Id = q.id,
                Customer = q.CustomerName,
                DateOut = q.DateRented,
                DateIn = q.DateReturned,
                Cost = q.Cost,
                Car = q.TypesOfCar.Make + " " + q.TypesOfCar.Model
            }).ToList();

            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["Id"].Visible = false;
        }

        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            var addRentalRecordForm = new AddEditRentalRecord(this)
            {
                MdiParent = this.MdiParent
            };
            addRentalRecordForm.Show();
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // 获取选中行的 Id。
                // get Id of selected row.
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;

                // 查询数据库以获取记录。
                // query database for record.
                var record = _db.CarRentelRecord.FirstOrDefault(q => q.id == id);

                // 打开 AddEditVehicle 窗体，并传入车辆数据。
                // launch AddEditVehicle window with data.
                var addEditRentalRecord = new AddEditRentalRecord(this, record);
                addEditRentalRecord.MdiParent = this.MdiParent;
                addEditRentalRecord.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //throw;
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                var id = (int)gvRecordList.SelectedRows[0].Cells["Id"].Value;
                var record = _db.CarRentelRecord.FirstOrDefault(q => q.id == id);

                // 从数据库中删除车辆记录。
                // delete vehicle from table.
                _db.CarRentelRecord.Remove(record);
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

        private void btnRefresh_Click(object sender, EventArgs e)
        {

        }
    }
}
