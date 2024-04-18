﻿using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aromapp
{
    public partial class AddUser : DevExpress.XtraEditors.XtraForm
    {
        bool firstTime;
        public AddUser(bool first)
        {
            this.firstTime = first;
            InitializeComponent();
        }

        void PasswoCorrect(object sender, EventArgs e)
        {
            User user = new User();
            string BarCodePref;

            using (DBHelper helper = new DBHelper())
            {
                BarCodePref = helper.getStoreBarCode();
            }

            pass.ForeColor = Color.Black;
            user.ID = DBHelper.generateID("US", Tables.user);
            user.Name = name.Text;
            user.Password = pass.Text;
            user.IsAdmin = admin.Checked;
            user.isActive = false;
            user.BarCode = BarCodePref + DateTime.Now.Year + DateTime.Now.Month +
            DateTime.Now.Day + user.ID.Replace("US", "").TrimStart('0');



            using (DBHelper helper = new DBHelper())
            {
                if (helper.AddUser(user))
                {
                    MessageBoxer.showGeneralMsg("Bienvenue " + user.Name);


                }
                else
                {
                    MessageBoxer.showErrorMsg("Une erreur s'est produite");
                }
            }

        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            if (name.Text != name.Tag.ToString() &&
                !string.IsNullOrEmpty(name.Text)
                && !string.IsNullOrEmpty(pass.Text))
            {
                if (admin.Checked && !firstTime)
                {
                    Confirm confirm = new Confirm();
                    confirm.Passed += PasswoCorrect;
                    confirm.ShowDialog();

                    return;
                }


                User user = new User();
                string BarCodePref;

                using (DBHelper helper = new DBHelper())
                {
                    BarCodePref = helper.getStoreBarCode();
                }

                user.ID = DBHelper.generateID("US", Tables.user);
                user.Name = name.Text;
                user.Password = pass.Text;
                user.IsAdmin = admin.Checked;
                user.isActive = firstTime;

                user.BarCode = BarCodePref + DateTime.Now.Year + DateTime.Now.Month +
                DateTime.Now.Day + user.ID.Replace("US", "").TrimStart('0');


                if (firstTime && !admin.Checked)
                {
                    MessageBoxer.showGeneralMsg("Le premier utilisateur doit être un administrateur");

                }
                else if (firstTime && admin.Checked)
                {
                    using (DBHelper helper = new DBHelper())
                    {
                        Properties.Settings.Default.LoggedInUserID = user.ID;
                        Properties.Settings.Default.Save();
                        Properties.Settings.Default.LoggedInUserName = user.Name;
                        Properties.Settings.Default.Save();



                        if (helper.AddUser(user))
                        {                           

                            MessageBoxer.showGeneralMsg("Bienvenue " + user.Name);

                            if (firstTime)
                            {
                                Application.Restart();

                            }
                            else
                            {
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBoxer.showErrorMsg("Une erreur s'est produite");
                        }
                    }
                }
            }
            else
            {
                this.Close();
            }
        }

        private void AddUser_Load(object sender, EventArgs e)
        {

        }

        private void name_Click(object sender, EventArgs e)
        {

            HintUtils.HideHint(name);

        }

        private void name_Leave(object sender, EventArgs e)
        {
            
                HintUtils.ShowHint(name);
            
        }

    

        private void pass_IconRightClick(object sender, EventArgs e)
        {
            if (pass.UseSystemPasswordChar)
            {
                pass.PasswordChar = '\0';
                pass.UseSystemPasswordChar = false;

               

            }
            else
            {
                pass.PasswordChar = '●';
                pass.UseSystemPasswordChar = true;

              
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}