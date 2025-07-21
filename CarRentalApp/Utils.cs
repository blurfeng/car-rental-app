using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public static class Utils
    {
        /// <summary>
        /// 检查窗体是否已经在应用程序中打开。
        /// Check if the form is already open in the application.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool FormIsOpen<T>() where T : Form
        {
            var openForms = Application.OpenForms.Cast<Form>();
            return openForms.Any(form => form is T);
        }
    }
}
