using System;
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

        private void btnRegister_Click(object sender, EventArgs e)
        {

            if (textBoxPassword.Text==textBoxRepassword.Text)
            {
                var user = new User();
                user.UserName = textBoxUserName.Text;
                user.Password = BCrypt.Net.BCrypt.HashString(textBoxPassword.Text);

                c.Users.Add(user);
                c.SaveChanges();
                
                MessageBox.Show("You have Registered Successfuly");

                Form1 frm = new Form1();
                frm.UserID= user.UserId;
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
            this.Hide();
            frm.Show();
        }
    }
}
