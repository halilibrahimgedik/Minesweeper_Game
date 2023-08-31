using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mine_Sweeper
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        Context c = new Context();
        public int _userId;
        string _username;
        string _password;

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            _username = textBoxUserName.Text;
            _password = textBoxPassword.Text;


            var user = Login(_username, _password);

            if (user == null)
            {
                MessageBox.Show("Failed !!");

            }
            else
            {
                Form1 frm = new Form1();
                frm.UserID = user.UserId;
                frm.Show();
                this.Hide();

            }

        }

        private bool LoginAttempt(string username, string password)
        {

            var user = from p in c.Users
                       where p.UserName == username
                        && p.Password == password
                       select p;




            if (user.Any())
            {
                return true;

            }

            else
            {
                return false;
            }

        }

        public User Login(string username, string password)
        {
            Context c = new Context();
            var user = c.Users.SingleOrDefault(i => i.UserName.Equals(username));

            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }

            }
            return null;
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Register frm = new Register();
            frm.Show();
            this.Hide();
        }
    }
}
