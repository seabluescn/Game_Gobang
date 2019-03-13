using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace Gobang
{
    public enum MapState
    {
        None = 0,
        Computer = 1,
        Peoper = 2
    }

    public enum GameState
    {
        Stop = 0,
        ComputerRun = 1,
        PeopleRun = 2
    }

    public enum LastChessState
    {
        Computer = 1,
        People = 2
    }

    public partial class frmMain : Form
    {
        private const int XOFFSET = 23;
        private const int YOFFSET = 23;
        private const int WIDTH = 15;
        private const int length = 35;
        private const int ChessWidth = 32;

        //
        private MapState[,] map = new MapState[WIDTH, WIDTH];
        //
        private List<Point> Recoder = new List<Point>();
        private LastChessState LastChess = LastChessState.People;

        //
        private bool PeopleHasBlack = true;
        private GameState CurrentGameState = GameState.Stop;               

        //
        public frmMain()
        {
            InitializeComponent();
        }

        private void ClearMap()
        {
            int i, j;
            for (i = 0; i < WIDTH; i++)
                for (j = 0; j < WIDTH; j++)
                {
                    map[i, j] = MapState.None;
                }
            Recoder.Clear();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            this.skinEngine1.SkinFile = "DiamondBlue.ssk";
        }

        private void frmMain_Paint(object sender, PaintEventArgs e)
        {
           // Debug.WriteLine(e.ClipRectangle.ToString());

            Bitmap bmp = new Bitmap(540, 540);
            Graphics g = Graphics.FromImage(bmp);

            g.DrawImage(Gobang.Properties.Resources.Ground, 0, 0,537,537);

            int i,j;
            int x, y;
            for (i = 0; i < WIDTH; i++)
            {
                for (j = 0; j < WIDTH; j++)
                {
                    if (map[i, j] ==  MapState.Computer)
                    {
                        x = XOFFSET + i * length - 16;
                        y = YOFFSET + j * length - 16;
                        if (PeopleHasBlack)                        
                        {                           
                            g.DrawImage(Gobang.Properties.Resources.white, x, y, ChessWidth, ChessWidth);
                        }
                        else
                        {
                            g.DrawImage(Gobang.Properties.Resources.black, x, y, ChessWidth, ChessWidth);
                        }
                    }

                    if (map[i, j] ==  MapState .Peoper)
                    {
                        x = XOFFSET + i * length - 16;
                        y = YOFFSET + j * length - 16;
                        if (PeopleHasBlack)
                        {
                            g.DrawImage(Gobang.Properties.Resources.black, x, y, ChessWidth, ChessWidth);
                        }
                        else
                        {
                            g.DrawImage(Gobang.Properties.Resources.white, x, y, ChessWidth, ChessWidth);
                        }
                    }
                }
            }

            if (Recoder.Count > 0)
            {
                Point p = Recoder[Recoder.Count - 1];
                x = XOFFSET + p.X * length - 17;
                y = YOFFSET + p.Y * length - 17;
                g.DrawEllipse(Pens.Red, x, y, 33, 33);
            }

            this.picMain.Image = bmp;
        }
              
        private void btnStartNewGame_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();

            this.ClearMap();
            Invalidate();

            if (!PeopleHasBlack)
            {
                DateTime now = DateTime.Now;
                int x = 7 + now.Second % 3;
                int y = 7 + now.Millisecond % 3;
                map[x, y] = MapState.Computer;
                Recoder.Add(new Point(x,y));
            }

            CurrentGameState = GameState.PeopleRun;
            sbInfo.Text = "该你走了！";
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            backgroundWorker.CancelAsync();

            if (LastChess == LastChessState.People)
            {
                if (Recoder.Count > 0)
                {
                    Point p = Recoder[Recoder.Count - 1];
                    map[p.X, p.Y] = MapState.None;
                    Recoder.RemoveAt(Recoder.Count - 1);
                   
                    Invalidate();
                    sbInfo.Text = "该你走了！";
                }
            }
            else
            {
                if (Recoder.Count > 1)
                {
                    Point p = Recoder[Recoder.Count - 1];
                    map[p.X, p.Y] = MapState.None;
                    Recoder.RemoveAt(Recoder.Count - 1);

                    if (Recoder.Count > 0)
                    {
                        p = Recoder[Recoder.Count - 1];
                        map[p.X, p.Y] = MapState.None;
                        Recoder.RemoveAt(Recoder.Count - 1);
                    }
                    Invalidate();
                    sbInfo.Text = "该你走了！";
                }
            }

            LastChess = LastChessState.Computer;

            if (CurrentGameState == GameState.Stop)
            {
                CurrentGameState = GameState.PeopleRun;
            }

            if (Recoder.Count == 0)
            {
                this.btnGoBack.Enabled = false;
            }
        }

        //People
        private void picMain_MouseClick(object sender, MouseEventArgs e)
        {
            if (CurrentGameState == GameState.PeopleRun)
            {

                if (e.Button == MouseButtons.Left)
                {
                    int x = (e.X - XOFFSET + 17) / length;
                    int y = (e.Y - YOFFSET + 17) / length;

                    if (x >= 0 && x < 15 && y >= 0 && y < 15)
                    {
                        if (map[x, y] == MapState.None)
                        {
                            map[x, y] = MapState.Peoper;
                            Recoder.Add(new Point(x, y));
                            LastChess = LastChessState.People;
                            PlaySound();
                            Invalidate();
                            if (Recoder.Count > 0)
                            {
                                this.btnGoBack.Enabled = true;
                            }

                            if (CheckSuccess())
                            {
                                this.CurrentGameState = GameState.Stop;
                                this.pgbComputer.Value = 0;
                                this.pic.Enabled = false;
                                sbInfo.Text = "你赢了！";
                                MessageBox.Show("恭喜你，你赢了！", "提示");

                                return;
                            }

                            this.CurrentGameState = GameState.ComputerRun;
                            this.pic.Enabled = true;
                            sbInfo.Text = "电脑正在想解，请稍候...";

                            this.backgroundWorker.RunWorkerAsync();
                        }                       
                    }
                }
            }
        }
         
        //Computer
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Computer_Compute(e);          
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            pgbComputer.Value = e.ProgressPercentage;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                return;
            }

            PlaySound();
            Invalidate();            

            this.pgbComputer.Value = 0;
            this.pic.Enabled = false;
            sbInfo.Text = "该你走了！";
            this.CurrentGameState = GameState.PeopleRun;

            if (Recoder.Count> 0)
            {
                this.btnGoBack.Enabled = true;
            }

            if (CheckSuccess())
            {
                this.CurrentGameState = GameState.Stop;
                sbInfo.Text = "电脑获胜！";
                MessageBox.Show("对不起,你输了！","提示");
            }
        }
        
        #region 界面
        private void menuGameStart_Click(object sender, EventArgs e)
        {
            this.btnStartNewGame_Click(null, null);
        }

        private void menuGameGoBack_Click(object sender, EventArgs e)
        {
            this.btnGoBack_Click(null, null);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void menuBlack_Click(object sender, EventArgs e)
        {
            this.PeopleHasBlack = true;
            this.menuBlack.Checked = true;
            this.menuWhite.Checked = false;
        }

        private void meunWhite_Click(object sender, EventArgs e)
        {
            this.PeopleHasBlack = false;
            this.menuBlack.Checked = false;
            this.menuWhite.Checked = true;
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.backgroundWorker.IsBusy)
            {
                e.Cancel = true;
            }
        }

        //声音
        private void menuSetupSound_Click(object sender, EventArgs e)
        {
            Sound = !Sound;
            this.menuSetupSound.Checked = Sound;
        }

        private void menuSetupMusic_Click(object sender, EventArgs e)
        {
            Music = !Music;
            this.menuSetupMusic.Checked = Music;
            this.timer.Enabled = Music;

            if (Music == false)
            {
                mci.Stop();
            }
        }
        private bool Sound = false;
        private System.Media.SoundPlayer SoundMove = new System.Media.SoundPlayer(Gobang.Properties.Resources.ClickSound);

        private void PlaySound()
        {
            if (Sound)
            {
                SoundMove.Play();
            }
        }
        //背景音乐
        private bool Music = false;
        private List<string> listMusic = new List<string>();
        MciDevice mci = new MciDevice();
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.Music)
            {
                if (listMusic.Count == 0)
                {
                    string Path = System.Environment.CurrentDirectory;
                    DirectoryInfo Dir = new DirectoryInfo(Path + "\\Audio");
                    foreach (FileInfo fi in Dir.GetFiles("*.mp3"))
                    {
                        listMusic.Add(fi.FullName);
                    }
                    Dir = new DirectoryInfo(Path + "\\Audio");
                    foreach (FileInfo fi in Dir.GetFiles("*.wma"))
                    {
                        listMusic.Add(fi.FullName);
                    }

                    foreach (string f in listMusic)
                    {
                        Debug.WriteLine(f);
                    }
                }

                if (listMusic.Count != 0)
                {
                    int Dur = mci.Duration;
                    int Cur = mci.CurrentPosition;

                    if (Cur == Dur)
                    {
                        mci = new MciDevice();

                        int ran = DateTime.Now.Second % listMusic.Count;
                        mci.FileName = listMusic[ran];

                        Debug.WriteLine(ran.ToString());
                        Debug.WriteLine(mci.FileName);

                        mci.Play();
                    }
                }
            }
        }
        #endregion
        
        //胜利
        private bool CheckSuccess()
        {
            int i, j;
            for (i = 0; i < WIDTH; i++)
            {
                for (j = 0; j < WIDTH; j++)
                {
                    if (map[i, j] == MapState.Computer)
                    {
                        if (MatchSuccess(i, j, MapState.Computer))
                        {                            
                            return true; ;
                        }
                    }
                }
            }

            for (i = 0; i < WIDTH; i++)
            {
                for (j = 0; j < WIDTH; j++)
                {
                    if (map[i, j] == MapState.Peoper)
                    {
                        if (MatchSuccess(i, j, MapState.Peoper))
                        {                           
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private bool MatchSuccess(int x, int y, MapState state)
        {
            int time;
            int Count =0;
            int cx,cy;

            for (time = 0; time < 5; time++)
            {
                cx = x + time;
                cy = y;
                if (cx > 14)
                    break;

                if (map[cx, cy] == state)
                {
                    Count++;
                }
            }
            if (Count == 5)
            {
                return true;
            }

            Count = 0; 
            for (time = 0; time < 5; time++)
            {
                cx = x ;
                cy = y +time ;
                if (cy > 14)
                    break;

                if (map[cx, cy] == state)
                {
                    Count++;
                }
            }
            if (Count == 5)
            {
                return true;
            }

            Count = 0;
            for (time = 0; time < 5; time++)
            {
                cx = x + time;
                cy = y + time;
                if (cx > 14 || cy > 14)
                    break;

                if (map[cx, cy] == state)
                {
                    Count++;
                }
            }
            if (Count == 5)
            {
                return true;
            }

            Count = 0;
            for (time = 0; time < 5; time++)
            {
                cx = x + time;
                cy = y - time;
                if (cx > 14 || cy < 0)
                    break;

                if (map[cx, cy] == state)
                {
                    Count++;
                }
            }
            if (Count == 5)
            {
                return true;
            }          
           
            return false;
        }
        

        #region 人工智能
        private void Computer_Compute(DoWorkEventArgs e)
        {
            int[,] ComputerScore = new int[WIDTH, WIDTH];
            int[,] PeopleScore = new int[WIDTH, WIDTH];

            int ComputerMax = 0;
            int PeopleMax = 0;

            int ComputerMaxX = -1;
            int ComputerMaxY = -1;

            int PeopleMaxX = -1;
            int PeopleMaxY = -1;

            int i, j;
            for (i = 0; i < WIDTH; i++)
            {
                for (j = 0; j < WIDTH; j++)
                {
                    if (backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    int progress = ((i * WIDTH) + j) * 100 / 225;
                    if (progress > 100)
                        progress = 100;
                    backgroundWorker.ReportProgress(progress);

                    if (map[i, j] != MapState.None)
                    {
                        ComputerScore[i, j] = 0;
                        PeopleScore[i, j] = 0;
                    }
                    else
                    {
                        ComputerScore[i, j] = ComputeScore(i,j, MapState.Computer);
                        PeopleScore[i, j] = ComputeScore(i, j, MapState.Peoper);
                    }

                    if (ComputerScore[i, j] > ComputerMax)
                    {
                        ComputerMax = ComputerScore[i, j];
                        ComputerMaxX = i;
                        ComputerMaxY = j;
                    }

                    if (PeopleScore[i, j] > PeopleMax)
                    {
                        PeopleMax = PeopleScore[i, j];
                        PeopleMaxX = i;
                        PeopleMaxY = j;
                    }
                }
            }

            int ResultX = 0;
            int ResultY = 0;

            //高级：有多个最大值，再比较对方的值
            if (PeopleMax > ComputerMax)
            {
                int Score = 0;
                int Max = 0;
                for (i = 0; i < WIDTH; i++)
                {
                    for (j = 0; j < WIDTH; j++)
                    {
                        if (PeopleScore[i, j] == PeopleMax)
                        {
                            Score = ComputerScore[i, j];
                            if (Score > Max)
                            {
                                Max = Score;
                                ResultX = i;
                                ResultY = j;
                            }
                        }
                    }
                }
                //ResultX = PeopleMaxX;
                //ResultY = PeopleMaxY;
                Debug.WriteLine("(防守)PeopleMax : " + PeopleMax.ToString());
            }
            else
            {
                int Score = 0;
                int Max = 0;
                for (i = 0; i < WIDTH; i++)
                {
                    for (j = 0; j < WIDTH; j++)
                    {
                        if (ComputerScore[i, j] == ComputerMax)
                        {
                            Score = PeopleScore[i, j];
                            if (Score > Max)
                            {
                                Max = Score;
                                ResultX = i;
                                ResultY = j;
                            }
                        }
                    }
                }
                //ResultX = ComputerMaxX;
                //ResultY = ComputerMaxY;
                Debug.WriteLine("(进攻)ComputerMax : " + ComputerMax.ToString());
            }

            if (ResultX == -1 || ResultY == -1)
            {
                return;
            }

            map[ResultX, ResultY] = MapState.Computer;
            Recoder.Add(new Point(ResultX, ResultY));
            LastChess = LastChessState.Computer;
            return;           
        }

        public int ComputeScore(int x,int y,MapState state)
        {
            int Score = 1;  //是空就有1分
           // int cx,cy;

      //已胜利
            if (MatchActiveFour(x, y, state))
            {
                return 10000000;
            }

      //必胜:必胜无大小之分
            //活三（冲四）
            if (MatchActiveThree(x, y, state))
            {
                return 5000000;
            }
            //双杀(活三，半活四，断五 共两个)
            if (MatchDoubleKill(x, y, state))
            {
                return 4000000;
            }           

            
      //加成           

            //连二(两个连子，可以成活三)
            if (MatchActiveTwo(x, y, state))
            {
                Score = 1000000;
            } 
            
            //活一(独子，可以成连二)
            Score += 100000 * MatchActiveOne(x, y, state);  

            //近二 
            Score += 10000 * MatchClosedTwo(x, y, state);
            
            //近一 
            Score += 1000 * MatchClosedOne(x, y, state);  

            //远二
            Score += 100 * MatchFarTwo(x, y, state);

            //远一
            Score += 10 * MatchFarOne(x, y, state);  
           
            //Poor一
            Score += 1 * MatchPoorOne(x, y, state);  

            return Score;
        }

        public bool MatchEquals(int x, int y, MapState state)
        {
            if (x < 0 || x > 14 || y < 0 || y > 14)
                return false;

            return (map[x, y] == state);
        }

        //活连四
        public bool MatchActiveFour(int x, int y, MapState state)
        {
            if (MatchEquals(x - 4, y, state) && MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state))
            {
                return true;
            }
            if (MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state))
            {
                return true;
            }
            if (MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x +2, y, state))
            {
                return true;
            }
            if (MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state))
            {
                return true;
            }
            if (MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state) && MatchEquals(x + 4, y, state))
            {
                return true;
            }

            if (MatchEquals(x - 4, y-4, state) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state))
            {
                return true;
            }
            if (MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state))
            {
                return true;
            }
            if (MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state))
            {
                return true;
            }
            if (MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state))
            {
                return true;
            }
            if (MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state) && MatchEquals(x + 4, y+4, state))
            {
                return true;
            }

            if (MatchEquals(x , y - 4, state) && MatchEquals(x , y - 3, state) && MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state))
            {
                return true;
            }
            if (MatchEquals(x , y - 3, state) && MatchEquals(x , y - 2, state) && MatchEquals(x , y - 1, state) && MatchEquals(x , y + 1, state))
            {
                return true;
            }
            if (MatchEquals(x, y - 2, state) && MatchEquals(x , y - 1, state) && MatchEquals(x , y + 1, state) && MatchEquals(x , y + 2, state))
            {
                return true;
            }
            if (MatchEquals(x, y - 1, state) && MatchEquals(x , y + 1, state) && MatchEquals(x, y + 2, state) && MatchEquals(x , y + 3, state))
            {
                return true;
            }
            if (MatchEquals(x, y + 1, state) && MatchEquals(x , y + 2, state) && MatchEquals(x , y + 3, state) && MatchEquals(x , y + 4, state))
            {
                return true;
            }

            if (MatchEquals(x+4, y - 4, state) && MatchEquals(x+3, y - 3, state) && MatchEquals(x+2, y - 2, state) && MatchEquals(x+1, y - 1, state))
            {
                return true;
            }
            if (MatchEquals(x+3, y - 3, state) && MatchEquals(x+2, y - 2, state) && MatchEquals(x+1, y - 1, state) && MatchEquals(x-1, y + 1, state))
            {
                return true;
            }
            if (MatchEquals(x+2, y - 2, state) && MatchEquals(x+1, y - 1, state) && MatchEquals(x-1, y + 1, state) && MatchEquals(x-2, y + 2, state))
            {
                return true;
            }
            if (MatchEquals(x+1, y - 1, state) && MatchEquals(x-1, y + 1, state) && MatchEquals(x-2, y + 2, state) && MatchEquals(x-3, y + 3, state))
            {
                return true;
            }
            if (MatchEquals(x-1, y + 1, state) && MatchEquals(x-2, y + 2, state) && MatchEquals(x-3, y + 3, state) && MatchEquals(x-4, y + 4, state))
            {
                return true;
            }           

            return false;
        }

        //活三（冲四）
        public bool MatchActiveThree(int x, int y, MapState state)
        {
            if(MatchEquals(x-3,y,state) && MatchEquals(x-2,y,state) && MatchEquals(x-1,y,state) && (MatchEquals(x-4,y,MapState.None)&&MatchEquals(x+1,y,MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x + 2, y, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x + 3, y, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state) && (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 4, y, MapState.None)))
            {
                return true;
            }

            if (MatchEquals(x - 3, y - 3, state) && MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && (MatchEquals(x - 4, y - 4, MapState.None) && MatchEquals(x + 1, y + 1, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && (MatchEquals(x - 3, y - 3, MapState.None) && MatchEquals(x + 2, y + 2, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && (MatchEquals(x - 2, y - 2, MapState.None) && MatchEquals(x + 3, y + 3, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && MatchEquals(x + 3, y + 3, state) && (MatchEquals(x - 1, y - 1, MapState.None) && MatchEquals(x + 4, y + 4, MapState.None)))
            {
                return true;
            }

            if (MatchEquals(x, y - 3, state) && MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && (MatchEquals(x, y - 4, MapState.None) && MatchEquals(x, y + 1, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && (MatchEquals(x, y - 3, MapState.None) && MatchEquals(x, y + 2, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && (MatchEquals(x, y - 2, MapState.None) && MatchEquals(x, y + 3, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && MatchEquals(x, y + 3, state) && (MatchEquals(x, y - 1, MapState.None) && MatchEquals(x, y + 4, MapState.None)))
            {
                return true;
            }

            if (MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && (MatchEquals(x - 4, y + 4, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x + 2, y - 2, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x + 3, y - 3, MapState.None)))
            {
                return true;
            }
            if (MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state) && (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 4, y - 4, MapState.None)))
            {
                return true;
            }

            return false;
        }

        //双杀(活三，半活四，断五 共两个)
        public bool MatchDoubleKill(int x, int y, MapState state)
        {
            int Count = 0;

        //Next1:  // -
            //活三
            if (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, MapState.None))
            {
                Count++;
                goto Next2;
            }
            if (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, MapState.None))
            {
                Count++;
                goto Next2;
            }
            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, MapState.None))
            {
                Count++;
                goto Next2;
            }
            //半活四
            if (MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && (MatchEquals(x - 4, y, MapState.None) || MatchEquals(x + 1, y, MapState.None)))
            {
                Count++;
                goto Next2;
            }
            if (MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && (MatchEquals(x - 3, y, MapState.None) || MatchEquals(x + 2, y, MapState.None)))
            {
                Count++;
                goto Next2;
            }
            if (MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && (MatchEquals(x - 2, y, MapState.None) || MatchEquals(x + 3, y, MapState.None)))
            {
                Count++;
                goto Next2;
            }
            if (MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state) && (MatchEquals(x - 1, y, MapState.None) || MatchEquals(x + 4, y, MapState.None)))
            {
                Count++;
                goto Next2;
            }
            //断五
            if (MatchEquals(x + 1, y, MapState.None) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state) && MatchEquals(x + 4, y, state))
            {
                Count++;  goto Next2;               
            }
            if (MatchEquals(x + 1, y, MapState.None) && MatchEquals(x -1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x + 1, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x -1, y, state) && MatchEquals(x + 2, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x + 1, y, MapState.None) && MatchEquals(x -3, y, state) && MatchEquals(x -2, y, state) && MatchEquals(x -1, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x + 2, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 3, y, state) && MatchEquals(x + 4, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x + 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 3, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x + 2, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x -1, y, state) && MatchEquals(x +1, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x + 3, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 4, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x + 3, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x + 4, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state))
            {
                Count++; goto Next2;
            }


            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x -2, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x -3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x + 1, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x -4, y, state) && MatchEquals(x -3, y, state) && MatchEquals(x -2, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x -3, y, state) && MatchEquals(x + 1, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x -4, y, state) && MatchEquals(x -3, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state))
            {
                Count++; goto Next2;
            }
            if (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x -4, y, state))
            {
                Count++; goto Next2;
            }

            if (MatchEquals(x - 4, y, MapState.None) && MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state))
            {
                Count++; goto Next2;
            }
            
        Next2:  // |
            //活三
            if (MatchEquals(x, y - 3, MapState.None) && MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, MapState.None))
            {
                Count++;
                goto Next3;
            }
            if (MatchEquals(x, y - 2, MapState.None) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, MapState.None))
            {
                Count++;
                goto Next3;
            }
            if (MatchEquals(x, y - 1, MapState.None) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && MatchEquals(x, y + 3, MapState.None))
            {
                Count++;
                goto Next3;
            }
            //半活四
            if (MatchEquals(x, y - 3, state) && MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && (MatchEquals(x, y - 4, MapState.None) || MatchEquals(x, y + 1, MapState.None)))
            {
                Count++;
                goto Next3;
            }
            if (MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && (MatchEquals(x, y - 3, MapState.None) || MatchEquals(x, y + 2, MapState.None)))
            {
                Count++;
                goto Next3;
            }
            if (MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && (MatchEquals(x, y - 2, MapState.None) || MatchEquals(x, y + 3, MapState.None)))
            {
                Count++;
                goto Next3;
            }
            if (MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && MatchEquals(x, y + 3, state) && (MatchEquals(x, y - 1, MapState.None) || MatchEquals(x, y + 4, MapState.None)))
            {
                Count++;
                goto Next3;
            }
            //断五
            if (MatchEquals(x , y+1, MapState.None) && MatchEquals(x , y+2, state) && MatchEquals(x , y+3, state) && MatchEquals(x , y+4, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+1, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y+2, state) && MatchEquals(x , y+3, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+1, MapState.None) && MatchEquals(x , y-2, state) && MatchEquals(x , y-1, state) && MatchEquals(x , y+2, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+1, MapState.None) && MatchEquals(x , y-3, state) && MatchEquals(x , y-2, state) && MatchEquals(x , y-1, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y+2, MapState.None) && MatchEquals(x , y+1, state) && MatchEquals(x , y+3, state) && MatchEquals(x , y+4, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+2, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y+1, state) && MatchEquals(x , y+3, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+2, MapState.None) && MatchEquals(x , y-2, state) && MatchEquals(x , y-1, state) && MatchEquals(x , y+1, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y+3, MapState.None) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state) && MatchEquals(x , y+4, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y+3, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y+4, MapState.None) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state) && MatchEquals(x , y+3, state))
            {
                Count++;goto Next3;
            }


            if (MatchEquals(x , y-1, MapState.None) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state) && MatchEquals(x , y+3, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-1, MapState.None) && MatchEquals(x , y-2, state) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-1, MapState.None) && MatchEquals(x , y-3, state) && MatchEquals(x , y-2, state) && MatchEquals(x , y+1, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-1, MapState.None) && MatchEquals(x , y-4, state) && MatchEquals(x , y-3, state) && MatchEquals(x , y-2, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y-2, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y+1, state) && MatchEquals(x , y+2, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-2, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y-3, state) && MatchEquals(x , y+1, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-2, MapState.None) && MatchEquals(x , y-1, state) && MatchEquals(x , y-4, state) && MatchEquals(x , y-3, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y-3, MapState.None) && MatchEquals(x , y-2, state) && MatchEquals(x , y-1, state) && MatchEquals(x , y+1, state))
            {
                Count++;goto Next3;
            }
            if (MatchEquals(x , y-3, MapState.None) && MatchEquals(x , y-2, state) && MatchEquals(x , y-1, state) && MatchEquals(x , y-4, state))
            {
                Count++;goto Next3;
            }

            if (MatchEquals(x , y-4, MapState.None) && MatchEquals(x , y-3, state) && MatchEquals(x , y-2, state) && MatchEquals(x, y-1, state))
            {
                Count++;goto Next3;
            }    
            
        Next3:  // /
            //活三
            if (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, MapState.None))
            {
                Count++;
                goto Next4;
            }
            if (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, MapState.None))
            {
                Count++;
                goto Next4;
            }
            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, MapState.None))
            {
                Count++;
                goto Next4;
            }
            //半活四
            if (MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && (MatchEquals(x - 4, y + 4, MapState.None) || MatchEquals(x + 1, y - 1, MapState.None)))
            {
                Count++;
                goto Next4;
            }
            if (MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && (MatchEquals(x - 3, y + 3, MapState.None) || MatchEquals(x + 2, y - 2, MapState.None)))
            {
                Count++;
                goto Next4;
            }
            if (MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && (MatchEquals(x - 2, y + 2, MapState.None) || MatchEquals(x + 3, y - 3, MapState.None)))
            {
                Count++;
                goto Next4;
            }
            if (MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state) && (MatchEquals(x - 1, y + 1, MapState.None) || MatchEquals(x + 4, y - 4, MapState.None)))
            {
                Count++;
                goto Next4;
            }
            //断五
            if (MatchEquals(x + 1, y - 1, MapState.None) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state) && MatchEquals(x + 4, y - 4, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 1, y - 1, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 1, y - 1, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 2, y - 2, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 1, y - 1, MapState.None) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 3, y - 3, state) && MatchEquals(x + 4, y - 4, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 3, y - 3, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x + 3, y - 3, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 4, y - 4, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x + 3, y - 3, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x + 4, y - 4, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state))
            {
                Count++;goto Next4;
            }


            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x + 1, y - 1, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x - 4, y + 4, state) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x + 1, y - 1, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x - 4, y + 4, state) && MatchEquals(x - 3, y + 3, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state))
            {
                Count++;goto Next4;
            }
            if (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x - 4, y + 4, state))
            {
                Count++;goto Next4;
            }

            if (MatchEquals(x - 4, y + 4, MapState.None) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state))
            {
                Count++;goto Next4;
            }    
            
        Next4:  // \
            //活三
            if (MatchEquals(x - 3, y - 3, MapState.None) && MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, MapState.None))
            {
                Count++;
                goto Result;
            }
            if (MatchEquals(x - 2, y - 2, MapState.None) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, MapState.None))
            {
                Count++;
                goto Result;
            }
            if (MatchEquals(x - 1, y - 1, MapState.None) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && MatchEquals(x + 3, y + 3, MapState.None))
            {
                Count++;
                goto Result;
            }
            //半活四
            if (MatchEquals(x - 3, y - 3, state) && MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && (MatchEquals(x - 4, y - 4, MapState.None) || MatchEquals(x + 1, y + 1, MapState.None)))
            {
                Count++;
                goto Result;
            }
            if (MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && (MatchEquals(x - 3, y - 3, MapState.None) || MatchEquals(x + 2, y + 2, MapState.None)))
            {
                Count++;
                goto Result;
            }
            if (MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && (MatchEquals(x - 2, y - 2, MapState.None) || MatchEquals(x + 3, y + 3, MapState.None)))
            {
                Count++;
                goto Result;
            }
            if (MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && MatchEquals(x + 3, y + 3, state) && (MatchEquals(x - 1, y - 1, MapState.None) || MatchEquals(x + 4, y + 4, MapState.None)))
            {
                Count++;
                goto Result;
            }
            //断五
            if (MatchEquals(x + 1, y+1, MapState.None) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state) && MatchEquals(x + 4, y+4, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 1, y+1, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 1, y+1, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 2, y+2, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 1, y+1, MapState.None) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 3, y+3, state) && MatchEquals(x + 4, y+4, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 3, y+3, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x + 3, y+3, MapState.None) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 4, y+4, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x + 3, y+3, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x + 4, y+4, MapState.None) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state))
            {
                Count++;goto Result;
            }


            if (MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 3, y+3, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x + 1, y+1, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x - 4, y-4, state) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state) && MatchEquals(x + 2, y+2, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x - 3, y-3, state) && MatchEquals(x + 1, y+1, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, state) && MatchEquals(x - 4, y-4, state) && MatchEquals(x - 3, y-3, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x - 3, y-3, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x + 1, y+1, state))
            {
                Count++;goto Result;
            }
            if (MatchEquals(x - 3, y-3, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state) && MatchEquals(x - 4, y-4, state))
            {
                Count++;goto Result;
            }

            if (MatchEquals(x - 4, y-4, MapState.None) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, state))
            {
                Count++;goto Result;
            }    
            
       Result:     //结果
            if (Count >= 2)
            {
                return true;
            }
            return false;
        }

   
        //活二(两个连子，可以成活三)
        public bool MatchActiveTwo(int x, int y, MapState state)
        {

            if (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, state) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 1, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 3, y, MapState.None))
            {
                return true;
            }

            if (MatchEquals(x - 3, y - 3, MapState.None) && MatchEquals(x - 2, y - 2, state) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 2, y - 2, MapState.None) && MatchEquals(x - 1, y - 1, state) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 1, y - 1, MapState.None) && MatchEquals(x + 1, y + 1, state) && MatchEquals(x + 2, y + 2, state) && MatchEquals(x + 3, y + 3, MapState.None))
            {
                return true;
            }

            if (MatchEquals(x, y - 3, MapState.None) && MatchEquals(x, y - 2, state) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x, y - 2, MapState.None) && MatchEquals(x, y - 1, state) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x, y - 1, MapState.None) && MatchEquals(x, y + 1, state) && MatchEquals(x, y + 2, state) && MatchEquals(x, y + 3, MapState.None))
            {
                return true;
            }

            if (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, state) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, MapState.None))
            {
                return true;
            }
            if (MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 1, y - 1, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 3, y - 3, MapState.None))
            {
                return true;
            }

            return false;
        }
      
        //近二 
        public int MatchClosedTwo(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, MapState.None) && (MatchEquals(x - 4, y, MapState.None) || MatchEquals(x + 1, y, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y, state) && MatchEquals(x + 2, y, state) && MatchEquals(x + 1, y, MapState.None) && (MatchEquals(x + 4, y, MapState.None) || MatchEquals(x - 1, y, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, MapState.None) && (MatchEquals(x - 4, y-4, MapState.None) || MatchEquals(x + 1, y+1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y+3, state) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 1, y+1, MapState.None) && (MatchEquals(x + 4, y+4, MapState.None) || MatchEquals(x - 1, y-1, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x , y - 3, state) && MatchEquals(x , y - 2, state) && MatchEquals(x , y - 1, MapState.None) && (MatchEquals(x , y - 4, MapState.None) || MatchEquals(x , y + 1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x , y + 3, state) && MatchEquals(x , y + 2, state) && MatchEquals(x , y + 1, MapState.None) && (MatchEquals(x , y + 4, MapState.None) || MatchEquals(x , y - 1, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, MapState.None) && (MatchEquals(x - 4, y + 4, MapState.None) || MatchEquals(x + 1, y - 1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y - 3, state) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 1, y - 1, MapState.None) && (MatchEquals(x + 4, y - 4, MapState.None) || MatchEquals(x - 1, y + 1, MapState.None)))
            {
                Count++;
            }

            return Count;
        }
        //远二
        public int MatchFarTwo(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 4, y, state) && MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, MapState.None) )
            {
                Count++;
            }
            if (MatchEquals(x + 4, y, state) && MatchEquals(x + 3, y, state) && MatchEquals(x + 2, y, MapState.None) && MatchEquals(x + 1, y, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 4, y-4, state) && MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 4, y+4, state) && MatchEquals(x + 3, y+3, state) && MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x + 1, y+1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x , y - 4, state) && MatchEquals(x , y - 3, state) && MatchEquals(x , y - 2, MapState.None) && MatchEquals(x , y - 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x , y + 4, state) && MatchEquals(x , y + 3, state) && MatchEquals(x , y + 2, MapState.None) && MatchEquals(x , y + 1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 4, y + 4, state) && MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 4, y - 4, state) && MatchEquals(x + 3, y - 3, state) && MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None))
            {
                Count++;
            }
           

            return Count;
        }


        //活一(独子，可以成连二,out count)
        public int MatchActiveOne(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 1, y, state) && MatchEquals(x - 2, y, MapState.None) && MatchEquals(x + 1, y, MapState.None) && (MatchEquals(x - 3, y, MapState.None) || MatchEquals(x + 2, y, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 1, y, state) && MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 2, y, MapState.None) && (MatchEquals(x - 2, y, MapState.None) || MatchEquals(x + 3, y, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 1, y-1, state) && MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x + 1, y+1, MapState.None) && (MatchEquals(x - 3, y-3, MapState.None) || MatchEquals(x + 2, y+2, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 1, y+1, state) && MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x + 2, y+2, MapState.None) && (MatchEquals(x - 2, y-2, MapState.None) || MatchEquals(x + 3, y+3, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x , y - 1, state) && MatchEquals(x , y - 2, MapState.None) && MatchEquals(x , y + 1, MapState.None) && (MatchEquals(x , y - 3, MapState.None) || MatchEquals(x , y + 2, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x, y + 1, state) && MatchEquals(x , y - 1, MapState.None) && MatchEquals(x , y + 2, MapState.None) && (MatchEquals(x , y - 2, MapState.None) || MatchEquals(x , y + 3, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 1, y + 1, state) && MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None) && (MatchEquals(x - 3, y + 3, MapState.None) || MatchEquals(x + 2, y - 2, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 1, y - 1, state) && MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 2, y - 2, MapState.None) && (MatchEquals(x - 2, y + 2, MapState.None) || MatchEquals(x + 3, y - 3, MapState.None)))
            {
                Count++;
            }

            return Count;
        }

        //近一 
        public int MatchClosedOne(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, state) && MatchEquals(x - 1, y, MapState.None) && MatchEquals(x + 1, y, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y, MapState.None) && MatchEquals(x + 2, y, state) && MatchEquals(x + 1, y, MapState.None) && MatchEquals(x - 1, y, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y-3, MapState.None) && MatchEquals(x - 2, y-2, state) && MatchEquals(x - 1, y-1, MapState.None) && MatchEquals(x + 1, y+1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y+3, MapState.None) && MatchEquals(x + 2, y+2, state) && MatchEquals(x + 1, y+1, MapState.None) && MatchEquals(x - 1, y-1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x , y - 3, MapState.None) && MatchEquals(x , y - 2, state) && MatchEquals(x , y - 1, MapState.None) && MatchEquals(x , y + 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x , y + 3, MapState.None) && MatchEquals(x , y + 2, state) && MatchEquals(x , y + 1, MapState.None) && MatchEquals(x , y - 1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, state) && MatchEquals(x - 1, y + 1, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y - 3, MapState.None) && MatchEquals(x + 2, y - 2, state) && MatchEquals(x + 1, y - 1, MapState.None) && MatchEquals(x - 1, y + 1, MapState.None))
            {
                Count++;
            }            

            return Count;
        }
        //远一
        public int MatchFarOne(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 3, y, state) && MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, MapState.None) && (MatchEquals(x -4, y, MapState.None)||MatchEquals(x + 1, y, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y, state) && MatchEquals(x + 2, y, MapState.None) && MatchEquals(x + 1, y, MapState.None) && (MatchEquals(x + 4, y, MapState.None) || MatchEquals(x - 1, y, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y-3, state) && MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, MapState.None) && (MatchEquals(x - 4, y-4, MapState.None) || MatchEquals(x + 1, y+1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y+3, state) && MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x + 1, y+1, MapState.None) && (MatchEquals(x + 4, y+4, MapState.None) || MatchEquals(x - 1, y-1, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x , y - 3, state) && MatchEquals(x, y - 2, MapState.None) && MatchEquals(x , y - 1, MapState.None) && (MatchEquals(x , y - 4, MapState.None) || MatchEquals(x , y + 1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x , y + 3, state) && MatchEquals(x , y + 2, MapState.None) && MatchEquals(x , y + 1, MapState.None) && (MatchEquals(x , y + 4, MapState.None) || MatchEquals(x , y - 1, MapState.None)))
            {
                Count++;
            }

            if (MatchEquals(x - 3, y + 3, state) && MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, MapState.None) && (MatchEquals(x - 4, y + 4, MapState.None) || MatchEquals(x + 1, y - 1, MapState.None)))
            {
                Count++;
            }
            if (MatchEquals(x + 3, y - 3, state) && MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None) && (MatchEquals(x + 4, y - 4, MapState.None) || MatchEquals(x - 1, y + 1, MapState.None)))
            {
                Count++;
            }
            

            return Count;
        }
        //Poor一
        public int MatchPoorOne(int x, int y, MapState state)
        {
            int Count = 0;

            if (MatchEquals(x - 4, y, state) && MatchEquals(x - 3, y, MapState.None) && MatchEquals(x - 2, y, MapState.None) && MatchEquals(x - 1, y, MapState.None) )
            {
                Count++;
            }
            if (MatchEquals(x + 4, y, state) && MatchEquals(x + 3, y, MapState.None) && MatchEquals(x + 2, y, MapState.None) && MatchEquals(x + 1, y, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 4, y-4, state) && MatchEquals(x - 3, y-3, MapState.None) && MatchEquals(x - 2, y-2, MapState.None) && MatchEquals(x - 1, y-1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 4, y+4, state) && MatchEquals(x + 3, y+3, MapState.None) && MatchEquals(x + 2, y+2, MapState.None) && MatchEquals(x + 1, y+1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x , y - 4, state) && MatchEquals(x , y - 3, MapState.None) && MatchEquals(x , y - 2, MapState.None) && MatchEquals(x , y - 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x , y + 4, state) && MatchEquals(x , y + 3, MapState.None) && MatchEquals(x , y + 2, MapState.None) && MatchEquals(x , y + 1, MapState.None))
            {
                Count++;
            }

            if (MatchEquals(x - 4, y + 4, state) && MatchEquals(x - 3, y + 3, MapState.None) && MatchEquals(x - 2, y + 2, MapState.None) && MatchEquals(x - 1, y + 1, MapState.None))
            {
                Count++;
            }
            if (MatchEquals(x + 4, y - 4, state) && MatchEquals(x + 3, y - 3, MapState.None) && MatchEquals(x + 2, y - 2, MapState.None) && MatchEquals(x + 1, y - 1, MapState.None))
            {
                Count++;
            } 

            return Count;
        }

        #endregion
    }
}