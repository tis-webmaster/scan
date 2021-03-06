﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;
using System.Data.OleDb;

namespace Scan_Project
{
    public partial class LoginForm : Telerik.WinControls.UI.RadForm
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            bool isLogin = false;
            byte userRole = 3;
            dbConnections db = new dbConnections();
            try
            {
                isLogin = db.CheckUser(txtUserName.Text, txtPassword.Text, out userRole);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }

            if (!isLogin)
            {
                RadMessageBox.ThemeName = "TelerikMetro";
                RadMessageBox.Show(null, "نام کاربری یا رمز عبور اشتباه است!", "خطا", MessageBoxButtons.OK,
                    RadMessageIcon.Error, MessageBoxDefaultButton.Button1, RightToLeft.Yes);
            }
            else
            {
                Properties.Settings.Default.userRole = userRole;
                Properties.Settings.Default.userName = txtUserName.Text;
                DialogResult = DialogResult.OK;
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
