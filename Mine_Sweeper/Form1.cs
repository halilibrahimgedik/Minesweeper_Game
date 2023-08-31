using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;


namespace Mine_Sweeper
{
    public partial class Form1 : Form
    {
        public int UserID;

        private readonly Context c = new Context();
        private MineSweeper mineSweeper;

        private string gameMode;
        private int flag;
        private int score = 0;
        private int min = 0; // timer için kullanıcaz
        private bool isPlaying = true;
        private int win;
        //sound
        private SoundPlayer backGroundplayer;
        private SoundPlayer explosionPlayer;
        private readonly Image minesAtPanel = Properties.Resources.mine32;
        private List<Mine> mines;
        private List<Mine> clickedMines;


        public Form1()
        {
            InitializeComponent();
            backGroundplayer = new SoundPlayer("Elevator.wav");
            explosionPlayer = new SoundPlayer("explosion2.wav");

        }

        private void Form1_Load(object sender, EventArgs e)
        {       
            pictureBoxSound.Image = Properties.Resources.soundOpen;

            mineSweeper = new MineSweeper(new Size(300, 300), 20);

            backGroundplayer.Play();
            clickedMines = new List<Mine>();
            gameMode = "Easy";
            flag = 20;
            labelFlag.Text = "20";

            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;

            AddMineToPanel();
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
            btn.Location = loc;
            btn.Size = new Size(30, 30);
            btn.Click += new EventHandler(btn_Click);
            btn.MouseUp += btn_MouseUp;
            btn.Image = Properties.Resources.button;
            btn.FlatStyle = FlatStyle.Popup;
            btn.Name = loc.X + "" + loc.Y;
            panel1.Controls.Add(btn);
        }

        private void btn_MouseUp(object sender, MouseEventArgs e)
        {
            Button btn = (sender as Button);
            Mine m = mineSweeper.GetMineAccordingToLocation(btn.Location);

            if (m.Flag == false)
            {
                if (e.Button == MouseButtons.Right)
                {
                    // btn.Text boş ise bayrak koyalım dolu ise koymayalım
                    if (btn.Text == "")
                    {
                        btn.Image = Properties.Resources.flag;
                        m.Flag = true;
                        flag -= 1;
                    }
                }
            }
            else
            {
                if (e.Button == MouseButtons.Right)
                {
                    btn.Image = Properties.Resources.button;
                    m.Flag = false;
                    flag += 1;
                }
            }

            labelFlag.Text = flag.ToString();
        }

        public void btn_Click(object sender, EventArgs e)
        {
            var user = c.Users.Find(UserID);
            Button btn = (sender as Button);
            Mine m = mineSweeper.GetMineAccordingToLocation(btn.Location);
            mines = new List<Mine>();

            if (m.IsThereMine)
            {
                explosionPlayer.Play();
                btn.Image = minesAtPanel;
                timer1.Stop();
                ShowAllMines();
                MessageBox.Show("Gameover", "warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                panel1.Enabled = false;
                score = 0;
            }
            else
            {
                int number = HowManyMinesOnAround(m);
                //etrafta mayın yoksa mayın bulana kadar dağılmalı
                if (number == 0)
                {
                    mines.Add(m);
                    for (int i = 0; i < mines.Count; i++)
                    {
                        win += win;

                        Mine mine = mines[i];

                        if (mine != null)
                        {

                            if (mine.MineIsChecked == false && mine.IsThereMine == false)
                            {
                                // etrafında sayı yoksa
                                Button btnx = (Button)panel1.Controls.Find(mine.GetLocation.X + "" + mine.GetLocation.Y, false)[0];

                                if (HowManyMinesOnAround(mines[i]) == 0)
                                {
                                    btnx.Enabled = false;
                                    btnx.Image = Properties.Resources.DarkGray3;

                                    mine.MineIsChecked = true;

                                    //mine.IsClicked = true;
                                    //ClickedMines.Add(mine);

                                    AddAround(mine);

                                    if (m.IsAddedScore == false)
                                    {
                                        score++;
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
                                    score++;

                                }
                                
                                labelScoreText.Text=score.ToString();
                                //---
                                mine.MineIsChecked = true;
                                clickedMines.Add(mine); 

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
                        btn.MouseUp -= btn_MouseUp;
                        btn.Image = Properties.Resources.button;
                        m.IsAddedScore = true;
                        
                    }
                    else if (number == 2)
                    {
                        btn.ForeColor = Color.Green;
                        btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                        btn.MouseUp -= btn_MouseUp;
                        btn.Image = Properties.Resources.button;
                        m.IsAddedScore = true;

                    }
                    else
                    {
                        btn.ForeColor = Color.Red;
                        btn.Font = new Font("Tahoma", 10F, FontStyle.Bold);
                        btn.MouseUp -= btn_MouseUp;
                        btn.Image = Properties.Resources.button;

                        m.IsAddedScore = true;
                    }

                    if (m.IsAddedScore==false)
                    {
                        score++;
                        m.IsAddedScore = true;
                    }
                   
                    labelScoreText.Text = score.ToString();
                    btn.Text = number.ToString();

                    if (m.MineIsChecked == false)
                    {
                        m.MineIsChecked = true;
                        clickedMines.Add(m);
                    }
                }
                
            }

            RecordScore(); // Kullanıcının skorunu oyunu kaybetsede tutuyoruz ama süreyi tutmayacağız (yani kazanırsa süre tutacağız) 

            // kazanma durumu
            if (clickedMines.Count== mineSweeper.GetAllMines.Count- mineSweeper.CountOfFillMine)
            {
                timer1.Stop();
                panel1.Enabled = false;

                // oyun bittiğinde  bitirme süresi kaydedilecek-
                user.GameMode = gameMode; // oynanılan game mode

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
                clickedMines = new List<Mine>();

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
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y)));
                b1 = true;
            }
            if (m.GetLocation.Y > 0)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y - 30)));
                b2 = true;
            }
            if (m.GetLocation.X < panel1.Width)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y)));
                b3 = true;
            }
            if (m.GetLocation.Y < panel1.Height)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X, m.GetLocation.Y + 30)));
                b4 = true;
            }
            if (b1 && b2)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y - 30)));
            }
            if (b1 && b4)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X - 30, m.GetLocation.Y + 30)));
            }
            if (b2 && b3)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y - 30)));
            }
            if (b2 && b4)
            {
                mines.Add(mineSweeper.GetMineAccordingToLocation(new Point(m.GetLocation.X + 30, m.GetLocation.Y + 30)));
            }
        }

        public void ShowAllMines()
        {
            foreach (var item in mineSweeper.GetAllMines)
            {
                if (item.IsThereMine)
                {
                    Button btn = (Button)panel1.Controls.Find(item.GetLocation.X + "" + item.GetLocation.Y, false)[0];
                    btn.Image = minesAtPanel;
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
            clickedMines = new List<Mine>();

            timer1.Start();
            score = 0;
            gameMode = "Easy";
            panel1.Enabled = true;

            ClearTimer();

            if (isPlaying)
            {
                backGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }

            mineSweeper = new MineSweeper(new Size(300, 300), 20); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            flag = 20;
            labelFlag.Text = flag.ToString();

            AddMineToPanel();
        }

        private void mediumToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clickedMines = new List<Mine>();

            timer1.Start();
            score = 0;
            labelScoreText.Text = score.ToString();
            gameMode = "Medium";
            panel1.Enabled = true;

            ClearTimer();

            if (isPlaying)
            {
                backGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }

            mineSweeper = new MineSweeper(new Size(450, 450), 40); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            flag = 40;
            labelFlag.Text = flag.ToString();

            AddMineToPanel();
        }

        private void hardToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            clickedMines = new List<Mine>();

            timer1.Start();
            score = 0;
            labelScoreText.Text= score.ToString();
            gameMode = "Hard";
            panel1.Enabled = true;
            ClearTimer();
            if (isPlaying)
            {
                backGroundplayer.Play();
                pictureBoxSound.Image = Properties.Resources.soundOpen;
            }

            mineSweeper = new MineSweeper(new Size(600, 600), 60); // easy 300 , med 450 , hard 600
            panel1.Size = mineSweeper.SizeOfMineSweeper;
            this.Width = panel1.Width + 185;
            this.Height = panel1.Height + 170;
            panel2.Width = this.Width;
            flag = 60;
            labelFlag.Text = flag.ToString();
            AddMineToPanel();
            ShowAllMines();
        }

        private void btn_Restart(object sender, EventArgs e)
        {
            clickedMines = new List<Mine>();

            score = 0;
            labelScoreText.Text = score.ToString();
            panel1.Enabled = true;

            ClearTimer();

            timer1.Start();
            if (panel1.Width == 300)
            {
                mineSweeper = new MineSweeper(new Size(300, 300), 20); // easy 300 / 14 , med 450 / 28 , hard 600 /52
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                flag = 20;
                labelFlag.Text = flag.ToString();

                AddMineToPanel();
            }
            else if (panel1.Width == 450)
            {
                mineSweeper = new MineSweeper(new Size(450, 450), 40); // easy 300 , med 450 , hard 600
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                panel2.Width = this.Width;
                flag = 40;
                labelFlag.Text = flag.ToString();

                AddMineToPanel();
            }
            else
            {
                mineSweeper = new MineSweeper(new Size(600, 600), 60); // easy 300 , med 450 , hard 600
                panel1.Size = mineSweeper.SizeOfMineSweeper;
                this.Width = panel1.Width + 185;
                this.Height = panel1.Height + 170;
                panel2.Width = this.Width;
                flag = 60;
                labelFlag.Text = flag.ToString();
                AddMineToPanel();
            }

        }

        private void soundImage_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 1; i++)
            {
                if (isPlaying == true)
                {
                    backGroundplayer.Stop();
                    pictureBoxSound.Image = Properties.Resources.mutedsound;
                    isPlaying = false;
                    continue;
                }
                if (isPlaying == false)
                {
                    backGroundplayer.Play();
                    pictureBoxSound.Image = Properties.Resources.soundOpen;
                    isPlaying = true;
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
            frm.bestScore = c.Users.Find(UserID).BestScore;
            frm.username = c.Users.Find(UserID).UserName.ToString();
            frm.gameMode = c.Users.Find(UserID).GameMode;
            frm._UserId = UserID;

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
            var user = c.Users.Find(UserID);
            // eski skor ile yeni skoru karşılaştırma ;
            var int1 = user.BestScore;
            var int2 = Convert.ToInt32(labelScoreText.Text);

            user.GameMode = gameMode;


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
