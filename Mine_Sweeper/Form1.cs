using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using System.Reflection;
using Mine_Sweeper.Migrations;

namespace Mine_Sweeper
{
    public partial class Form1 : Form
    {

        public int _userID;

        public string _gameMode;

        Context c = new Context();


        public Form1()
        {
            InitializeComponent();
            BackGroundplayer = new SoundPlayer("Elevator.wav");
            ExplosionPlayer = new SoundPlayer("explosion2.wav");

        }


        int _flag;
        int _score = 0;
        MineSweeper mineSweeper;

        List<Mine> Mines;

        Image MinesAtPanel = Properties.Resources.mine32;

        int win;

        List<Mine> ClickedMines;

        //sound
        private SoundPlayer BackGroundplayer;
        private SoundPlayer ExplosionPlayer;
        bool Isplaying = true;

        int min = 0; // timer için kullanıcaz

        private void Form1_Load(object sender, EventArgs e)
        {
            ClickedMines = new List<Mine>();

            _gameMode = "Easy";
            BackGroundplayer.Play();
            pictureBoxSound.Image = Properties.Resources.soundOpen;
            mineSweeper = new MineSweeper(new Size(300, 300), 20); // easy 300 / 20 , med 450 / 40 , hard 600 /60
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            _flag = 20;
            labelFlag.Text = "20";

            _score = 0;

            AddMineToPanel();

            ShowAllMines();
        }

        public void AddMineToPanel()
        {
            panel1.Controls.Clear();

            for (int x = 0; x < panel1.Width; x += 30)
            {
                for (int y = 0; y < panel1.Height; y += 30)
                {
                    AddButton(new Point(x, y));
                }
            }

        }

        public void AddButton(Point loc)
        {
            Button btn = new Button();
            //btn.BackColor = Color.Gray;
            btn.Location = loc;
            btn.Size = new Size(30, 30);
            btn.Click += new EventHandler(btn_Click);
            btn.MouseUp += BtnMouseUp;

            btn.Image = Properties.Resources.button;
            btn.FlatStyle = FlatStyle.Popup;
            //btn.FlatAppearance.BorderSize = 0;

            btn.Name = loc.X + "" + loc.Y;
            panel1.Controls.Add(btn);
        }

        private void BtnMouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (sender as Button);
            Mine m = mineSweeper.GetMineAccordingToLocation(btn.Location);


            if (m.Flag == false)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // btn.Text boşsa bayrak koyalım dolu ise koymayalım
                    if (btn.Text == "")
                    {
                        btn.Image = Properties.Resources.flag;
                        m.Flag = true;
                        _flag -= 1;
                    }

                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    btn.Image = Properties.Resources.button;
                    m.Flag = false;
                    _flag += 1;
                }
            }

            labelFlag.Text = _flag.ToString();


        }

        public void btn_Click(object sender, EventArgs e)
        {
            var user = c.Users.Find(_userID);

            Button btn = (sender as Button);

            Mine m = mineSweeper.GetMineAccordingToLocation(btn.Location);

            Mines = new List<Mine>();

            if (m.IsThereMine)
            {
                ExplosionPlayer.Play();
                btn.Image = MinesAtPanel;
                timer1.Stop();
                ShowAllMines();
                MessageBox.Show("Gameover", "warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                panel1.Enabled = false;
                _score = 0;
            }
            else
            {
                int number = HowManyMinesOnAround(m);

                //etrafta mayın yoksa mayın bulana kadar dağılmalı
                if (number == 0)
                {
                    Mines.Add(m);
                    for (int i = 0; i < Mines.Count; i++)
                    {
                        win += win;

                        Mine mine = Mines[i];

                        if (mine != null)
                        {

                            if (mine.MineIsChecked == false && mine.IsThereMine == false)
                            {
                                // etrafında sayı yoksa
                                Button btnx = (Button)panel1.Controls.Find(mine.GetLocation.X + "" + mine.GetLocation.Y, false)[0];

                                if (HowManyMinesOnAround(Mines[i]) == 0)
                                {
                                    btnx.Enabled = false;
                                    btnx.Image = Properties.Resources.DarkGray3;

                                    mine.MineIsChecked = true;

                                    //mine.IsClicked = true;
                                    //ClickedMines.Add(mine);

                                    AddAround(mine);

                                    if (m.IsAddedScore == false)
                                    {
                                        _score++;
                                        m.IsAddedScore = true;
                                    }
                                }
                                // etrafında sayı varsa
                                else
                                {
                                    var number2 = HowManyMinesOnAround(mine);

                                    if (number2 == 1)
                                    {
                                        btnx.ForeColor = Color.Black;
                                        btnx.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                                    }
                                    else if (number2 == 2)
                                    {
                                        btnx.ForeColor = Color.Green;
                                        btnx.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                                    }
                                    else
                                    {
                                        btnx.ForeColor = Color.Red;
                                        btnx.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                                    }

                                    btnx.Text = number2.ToString();
                                    _score++;

                                }
                                
                                labelScoreText.Text=_score.ToString();
                                //---
                                mine.MineIsChecked = true;
                                ClickedMines.Add(mine); 

                            }

                        }

                    }
                }
                else // butonun(mayının) etrafında 0 dan fazla mayın varsa
                {

                    if (number == 1)
                    {
                        btn.ForeColor = Color.Black;
                        btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                        btn.MouseUp -= BtnMouseUp;
                        btn.Image = Properties.Resources.button;
                        m.IsAddedScore = true;
                        
                    }
                    else if (number == 2)
                    {
                        btn.ForeColor = Color.Green;
                        btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                        btn.MouseUp -= BtnMouseUp;
                        btn.Image = Properties.Resources.button;
                        m.IsAddedScore = true;

                    }
                    else
                    {
                        btn.ForeColor = Color.Red;
                        btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                        btn.MouseUp -= BtnMouseUp;
                        btn.Image = Properties.Resources.button;

                        m.IsAddedScore = true;
                    }

                    if (m.IsAddedScore==false)
                    {
                        _score++;
                        m.IsAddedScore = true;
                    }
                   
                    labelScoreText.Text = _score.ToString();
                    btn.Text = number.ToString();

                    //Button btnx = (Button)panel1.Controls.Find(m.GetLocation.X + "" + m.GetLocation.Y, false)[0];
                    //if (m.IsClicked == false)
                    //{
                    //    m.IsClicked = true;
                    //    ClickedMines.Add(m);
                    //}

                    if (m.MineIsChecked == false)
                    {
                        m.MineIsChecked = true;
                        ClickedMines.Add(m);
                    }
                }
                
            }

            RecordScore(); // Kullanıcının skorunu oyunu kaybetsede tutuyoruz ama süreyi tutmayacağız (yani kazanırsa süre tutacağız) 


            // kazanma durumu
            if (ClickedMines.Count== mineSweeper.GetAllMines.Count- mineSweeper.CountOfFillMine)
            {
                timer1.Stop();

                // oyun bittiğinde  bitirme süresi kaydedilecek-
                

                user.GameMode = _gameMode; // oynanılan game mode

                string finishTime1;

                if (user.FinishTime!=null)
                {
                    finishTime1 = user.FinishTime;
                }
                else
                {
                    finishTime1 = "1000";
                }

                string finishTime2 = ($"0{labelMin.Text}:{labelSec.Text}");

                int a = Convert.ToInt32(finishTime2.Replace(":",""));
                int b = Convert.ToInt32(finishTime1.Replace(":",""));

                

                if (a < b)
                {
                    user.FinishTime = finishTime2;
                    c.SaveChanges();
                }

                MessageBox.Show("You Won The Game", "Congratulations ! ",MessageBoxButtons.OK,MessageBoxIcon.Information);
                ClickedMines = new List<Mine>();

                RecordScore();
            }
            

        }

       
        public int HowManyMinesOnAround(Mine m)
        {
            int number = 0;
            if (m.GetLocation.X > 0)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y)).IsThereMine)
                {
                    number++;
                  
                }
            }
            if (m.GetLocation.Y < panel1.Height - 30 && m.GetLocation.X < panel1.Width - 30)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y + 30)).IsThereMine)
                {
                    number++;
                    

                }
            }
            if (m.GetLocation.X < panel1.Width - 30)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y)).IsThereMine)
                {
                    number++;
                    
                }
            }
            if (m.GetLocation.X > 0 && m.GetLocation.Y > 0)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y - 30)).IsThereMine)
                {
                    number++;
                    
                }
            }
            if (m.GetLocation.Y > 0)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y - 30)).IsThereMine)
                {
                    number++;
                    
                }
            }
            if (m.GetLocation.X > 0 && m.GetLocation.Y < panel1.Height - 30)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y + 30)).IsThereMine)
                {
                    number++;
                    
                }
            }
            if (m.GetLocation.Y < panel1.Height - 30)
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y + 30)).IsThereMine)
                {
                    number++;
                    
                }
            }
            if (m.GetLocation.X < panel1.Width - 30 && m.GetLocation.Y > 0) // düzelitlen kısım m.GetLocation.X '>'  '<' e çevrildi.
            {
                if (mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y - 30)).IsThereMine)
                {
                    number++;
                    
                }
            }

            return number;
        }


        public void AddAround(Mine m)
        {
            bool b1 = false;
            bool b2 = false;
            bool b3 = false;
            bool b4 = false;
            if (m.GetLocation.X > 0)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y)));
                b1 = true;
            }
            if (m.GetLocation.Y > 0)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y - 30)));
                b2 = true;
            }
            if (m.GetLocation.X < panel1.Width)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y)));
                b3 = true;
            }
            if (m.GetLocation.Y < panel1.Height)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y + 30)));
                b4 = true;
            }
            if (b1 && b2)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y - 30)));
            }
            if (b1 && b4)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y + 30)));
            }
            if (b2 && b3)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y - 30)));
            }
            if (b2 && b4)
            {
                Mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y + 30)));
            }
        }

        public void ShowAllMines()
        {
            foreach (var item in mineSweeper.GetAllMines)
            {
                if (item.IsThereMine)
                {
                    Button btn = (Button)panel1.Controls.Find(item.GetLocation.X + "" + item.GetLocation.Y, false)[0];
                    btn.Image = MinesAtPanel;
                    btn.BackgroundImageLayout = ImageLayout.Stretch;
                }
            }
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            int sec = Convert.ToInt32(labelSec.Text);
            sec++;

            labelSec.Text = sec.ToString();

            if (sec == 59)
            {
                sec = 0;
                labelSec.Text = sec.ToString();

                min++;
                labelMin.Text = min.ToString();
            }
        }

        private void easyToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClickedMines = new List<Mine>();

            timer1.Start();
            _score = 0;
            _gameMode = "Easy";
            panel1.Enabled = true;
            ClearTimer();
            if (Isplaying)
            {
                BackGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }
            mineSweeper = new MineSweeper(new Size(300, 300), 20); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            _flag = 20;
            labelFlag.Text = _flag.ToString();
            AddMineToPanel();
            ShowAllMines();
        }

        private void mediumToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClickedMines = new List<Mine>();

            timer1.Start();
            _score = 0;
            labelScoreText.Text = _score.ToString();
            _gameMode = "Medium";
            panel1.Enabled = true;
            ClearTimer();
            if (Isplaying)
            {
                BackGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }
            mineSweeper = new MineSweeper(new Size(450, 450), 40); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            _flag = 40;
            labelFlag.Text = _flag.ToString();
            AddMineToPanel();
            //ShowAllMines();
        }

        private void hardToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            ClickedMines = new List<Mine>();

            timer1.Start();
            _score = 0;
            labelScoreText.Text= _score.ToString();
            _gameMode = "Hard";
            panel1.Enabled = true;
            ClearTimer();
            if (Isplaying)
            {
                BackGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }

            mineSweeper = new MineSweeper(new Size(600, 600), 60); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            _flag = 60;
            labelFlag.Text = _flag.ToString();
            AddMineToPanel();
            ShowAllMines();
        }

        private void btn_Restart(object sender, EventArgs e)
        {
            ClickedMines = new List<Mine>();

            _score = 0;
            labelScoreText.Text = _score.ToString();
            panel1.Enabled = true;
            ClearTimer();
            timer1.Start();
            if (panel1.Width == 300)
            {
                mineSweeper = new MineSweeper(new Size(300, 300), 20); // easy 300 / 14 , med 450 / 28 , hard 600 /52
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                _flag = 20;
                labelFlag.Text = _flag.ToString();
                AddMineToPanel();
                //ShowAllMines();
            }
            else if (panel1.Width == 450)
            {
                mineSweeper = new MineSweeper(new Size(450, 450), 40); // easy 300 , med 450 , hard 600
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                panel2.Width = this.Width;
                _flag = 40;
                labelFlag.Text = _flag.ToString();
                AddMineToPanel();
                //ShowAllMines();

            }
            else
            {
                mineSweeper = new MineSweeper(new Size(600, 600), 60); // easy 300 , med 450 , hard 600
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                panel2.Width = this.Width;
                _flag = 60;
                labelFlag.Text = _flag.ToString();
                AddMineToPanel();
                ShowAllMines();
            }

        }

        private void soundImage_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                if (Isplaying == true)
                {
                    BackGroundplayer.Stop();
                    pictureBoxSound.Image = Properties.Resources.mutedsound;
                    Isplaying = false;
                    continue;
                }
                if (Isplaying == false)
                {
                    BackGroundplayer.Play();
                    pictureBoxSound.Image = Properties.Resources.soundOpen;
                    Isplaying = true;
                }


            }


        }

        private void ClearTimer()
        {
            labelSec.Text = "0";
            labelMin.Text = "0";
        }


        private void yourBestScoreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new Scores();

            frm.bestScore = c.Users.Find(_userID).BestScore;

            frm.username = c.Users.Find(_userID).UserName.ToString();

            frm.gameMode = c.Users.Find(_userID).GameMode;

            frm._UserId = _userID;

            var users = c.Users.ToList();

            frm.usersList = users;
            frm.Show();


        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            LoginForm frm = new LoginForm();
            frm.Show();
        }

        public void RecordScore()
        {
            var user = c.Users.Find(_userID);
            // eski skor ile yeni skoru karşılaştırma ;
            var int1 = user.BestScore;
            var int2 = Convert.ToInt32(labelScoreText.Text);

            user.GameMode = _gameMode;


            if (int2 > int1)
            {
                user.BestScore = int2;

            }
            else
            {
                user.BestScore = int1;
            }

            c.SaveChanges();
        }
    }
}
