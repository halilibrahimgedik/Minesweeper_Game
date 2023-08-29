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
    public partial class Scores : Form
    {
        public Scores()
        {
            InitializeComponent();
        }
        public List<User> usersList = new List<User>();

        public int bestScore;
        public string username;
        public int _UserId;
        public string gameMode;

        private void Scores_Load(object sender, EventArgs e)
        {
            Context c = new Context();

            usersList = c.Users.OrderBy(x => x.BestScore).Take(c.Users.Count() - 3).ToList();

            var top3 = c.Users.OrderByDescending(x => x.BestScore).Take(3).ToList();

            int i = 0;
            foreach (var item in top3)
            {
                dataGridView2.Rows.Add();

                dataGridView2.Rows[i].Cells[0].Value = item.UserId;
                dataGridView2.Rows[i].Cells[1].Value = item.UserName;
                dataGridView2.Rows[i].Cells[2].Value = item.BestScore;
                dataGridView2.Rows[i].Cells[4].Value = item.GameMode;
                dataGridView2.Rows[i].Cells[3].Value = item.FinishTime;

                if (item.UserId!=_UserId)
                {
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    dataGridView2.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    dataGridView2.Rows[i].DefaultCellStyle.BackColor = Color.Black;
                    dataGridView2.Rows[i].DefaultCellStyle.ForeColor = Color.White;

                    dataGridView2.Rows[i].DefaultCellStyle.SelectionBackColor = Color.Yellow;
                    dataGridView2.Rows[i].DefaultCellStyle.SelectionForeColor = Color.Black;

                }


                i++;
            }

            int a = 3;
            foreach (var user in usersList)
            {

                dataGridView2.Rows.Add();

                //dataGridView2.Sort(dataGridView2.Columns[1], System.ComponentModel.ListSortDirection.Descending);

                dataGridView2.Rows[a].Cells[0].Value = user.UserId;
                dataGridView2.Rows[a].Cells[1].Value = user.UserName;
                dataGridView2.Rows[a].Cells[2].Value = user.BestScore;
                dataGridView2.Rows[a].Cells[4].Value = user.GameMode;
                dataGridView2.Rows[a].Cells[3].Value = user.FinishTime;

                if (user.UserId == _UserId)
                {
                    dataGridView2.Rows[a].DefaultCellStyle.BackColor = Color.Black;
                    dataGridView2.Rows[a].DefaultCellStyle.ForeColor = Color.White;

                    dataGridView2.Rows[a].DefaultCellStyle.SelectionBackColor = Color.Black;
                    dataGridView2.Rows[a].DefaultCellStyle.SelectionForeColor = Color.White;

                }

                a++;
            }




        }
    }
}
