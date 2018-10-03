﻿using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.Linq;

namespace Scan_Project
{
    class dbConnections
    {
        private string startPath = Application.StartupPath;

        OleDbConnection cn;

        public dbConnections()
        {
            cn = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={startPath}\\scanDB.accdb");
        }

        private string GenerateHashString(HashAlgorithm algo, string text)
        {
            // Compute hash from text parameter
            algo.ComputeHash(Encoding.UTF8.GetBytes(text));

            // Get has value in array of bytes
            var result = algo.Hash;

            // Return as hexadecimal string
            return string.Join(
                string.Empty,
                result.Select(x => x.ToString("x2")));
        }

        private string MD5(string text)
        {
            var result = default(string);

            using (var algo = new MD5CryptoServiceProvider())
            {
                result = GenerateHashString(algo, text);
            }

            return result;
        }

        /// <summary>
        /// گرفتن پروژه ها
        /// </summary>
        /// <returns>جدول پروژه‌ها</returns>
        public DataTable GetProjects()
        {
            DataTable dt = new DataTable();

            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = "select * from Projects";
            cmd.Connection = cn;
            OleDbDataAdapter da = new OleDbDataAdapter(cmd);
            try
            {
                cn.Open();
                da.Fill(dt);
                cn.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return dt;
        }

        /// <summary>
        /// ایجاد پروژه جدید
        /// </summary>
        /// <param name="projectName">اسم پروژه</param>
        public void InsertProjects(string projectName)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = string.Format("insert into Projects (projectName, projectStartDate) values ('{0}', '{1}')", projectName, System.DateTime.Now);
            cmd.Connection = cn;

            try
            {
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// چک کردن یوزر و پسورد وارد شده.
        /// </summary>
        /// <param name="userName">نام کاربری</param>
        /// <param name="password">رمز عبور</param>
        /// <returns></returns>
        public bool CheckUser(string userName, string password)
        {            
            DataTable dt = new DataTable();

            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = $"SELECT userName from Users where userName='{userName}' and userPassword='{MD5(password)}' and userIsActive=true";
            cmd.Connection = cn;
            
            try
            {
                cn.Open();
                OleDbDataAdapter da = new OleDbDataAdapter(cmd);
                da.Fill(dt);
                cn.Close();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            return dt.Rows.Count > 0 ? true : false;
        }

    }
}
