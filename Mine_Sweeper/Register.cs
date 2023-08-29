using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mine_Sweeper
{
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        Context c = new Context();
        //public int _userId;

        private void btnRegister_Click(object sender, EventArgs e)
        {
            

            if (textBoxPassword.Text==textBoxRepassword.Text)
            {
                var user = new User();
                user.UserName = textBoxUserName.Text;
                user.Password = BCrypt.Net.BCrypt.HashString(textBoxPassword.Text);

                c.Users.Add(user);
                c.SaveChanges();

                //_userId = user.UserId;
                

                MessageBox.Show("You have Registered Successfuly");

                Form1 frm = new Form1();
                frm._userID= user.UserId;
                this.Hide();
                frm.Show();
            }
            else
            {
                MessageBox.Show("RePassword and Password do not match ");
            }


            
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LoginForm frm = new LoginForm();
            //frm._userId = _userId;
            this.Hide();
            frm.Show();
        }
    }
}
