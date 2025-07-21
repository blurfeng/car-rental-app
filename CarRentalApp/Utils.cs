using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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

        public static string HashPassword(string password)
        {
            SHA256 sha = SHA256.Create();

            // 转换输入字符串为字节数组并计算哈希值。
            // Convert the input string to a byte array and compute the hash.
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

            // 将字节数组转换为十六进制字符串。并添加到 StringBuilder 中。
            // Convert the byte array to a hexadecimal string and append it to StringBuilder.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                sb.Append(bytes[i].ToString("x2"));
            }

            return sb.ToString();
        }

        public static string DefaultHashedPassword()
        {
            // 默认密码的哈希值。
            // The hash value of the default password.
            return HashPassword("Password@123");
        }
    }
}
