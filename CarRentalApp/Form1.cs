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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                    MessageBox.Show(
                    $"Customer Name: {customerName}\n\r"
                    + $"Date Rented: {dateOut}\n\r"
                    + $"Date Returned: {dateIn}\n\r"
                    + $"Cost: {cost}\n\r"
                    + $"Car Type: {carType}\n\r"
                    + "Thank you for your business.");
                }
                else
                {
                    MessageBox.Show(errorMessage, "Validation Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //throw;
            }

        }
    }
}
