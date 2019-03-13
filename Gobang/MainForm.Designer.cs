namespace Gobang
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuGame = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGameStart = new System.Windows.Forms.ToolStripMenuItem();
            this.menuGameGoBack = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetupSound = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSetupMusic = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.meunColor = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBlack = new System.Windows.Forms.ToolStripMenuItem();
            this.menuWhite = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrong = new System.Windows.Forms.ToolStripMenuItem();
            this.easyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.middleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.menuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.sbInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.pgbComputer = new System.Windows.Forms.ToolStripProgressBar();
            this.pic = new System.Windows.Forms.PictureBox();
            this.btnStartNewGame = new System.Windows.Forms.Button();
            this.btnGoBack = new System.Windows.Forms.Button();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.picMain = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.skinEngine1 = new Sunisoft.IrisSkin.SkinEngine(((System.ComponentModel.Component)(this)));
            this.btnSave = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGame,
            this.menuSetup,
            this.menuHelp});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(795, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuGame
            // 
            this.menuGame.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuGameStart,
            this.menuGameGoBack,
            this.toolStripSeparator1,
            this.menuExit});
            this.menuGame.Name = "menuGame";
            this.menuGame.Size = new System.Drawing.Size(59, 20);
            this.menuGame.Text = "游戏(&G)";
            // 
            // menuGameStart
            // 
            this.menuGameStart.Name = "menuGameStart";
            this.menuGameStart.Size = new System.Drawing.Size(152, 22);
            this.menuGameStart.Text = "开始(&S)";
            this.menuGameStart.Click += new System.EventHandler(this.menuGameStart_Click);
            // 
            // menuGameGoBack
            // 
            this.menuGameGoBack.Name = "menuGameGoBack";
            this.menuGameGoBack.Size = new System.Drawing.Size(152, 22);
            this.menuGameGoBack.Text = "悔子(&B)";
            this.menuGameGoBack.Click += new System.EventHandler(this.menuGameGoBack_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // menuExit
            // 
            this.menuExit.Name = "menuExit";
            this.menuExit.Size = new System.Drawing.Size(152, 22);
            this.menuExit.Text = "退出(&X)";
            this.menuExit.Click += new System.EventHandler(this.menuExit_Click);
            // 
            // menuSetup
            // 
            this.menuSetup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSetupSound,
            this.menuSetupMusic,
            this.toolStripSeparator2,
            this.meunColor,
            this.menuStrong});
            this.menuSetup.Name = "menuSetup";
            this.menuSetup.Size = new System.Drawing.Size(59, 20);
            this.menuSetup.Text = "设置(&T)";
            // 
            // menuSetupSound
            // 
            this.menuSetupSound.Name = "menuSetupSound";
            this.menuSetupSound.Size = new System.Drawing.Size(152, 22);
            this.menuSetupSound.Text = "声音(&S)";
            this.menuSetupSound.Click += new System.EventHandler(this.menuSetupSound_Click);
            // 
            // menuSetupMusic
            // 
            this.menuSetupMusic.Name = "menuSetupMusic";
            this.menuSetupMusic.Size = new System.Drawing.Size(152, 22);
            this.menuSetupMusic.Text = "背景音乐(&M)";
            this.menuSetupMusic.Click += new System.EventHandler(this.menuSetupMusic_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // meunColor
            // 
            this.meunColor.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBlack,
            this.menuWhite});
            this.meunColor.Name = "meunColor";
            this.meunColor.Size = new System.Drawing.Size(152, 22);
            this.meunColor.Text = "颜色(&C)";
            // 
            // menuBlack
            // 
            this.menuBlack.Checked = true;
            this.menuBlack.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuBlack.Name = "menuBlack";
            this.menuBlack.Size = new System.Drawing.Size(160, 22);
            this.menuBlack.Text = "执黑（先手）(&B)";
            this.menuBlack.Click += new System.EventHandler(this.menuBlack_Click);
            // 
            // menuWhite
            // 
            this.menuWhite.Name = "menuWhite";
            this.menuWhite.Size = new System.Drawing.Size(160, 22);
            this.menuWhite.Text = "执白(&W)";
            this.menuWhite.Click += new System.EventHandler(this.meunWhite_Click);
            // 
            // menuStrong
            // 
            this.menuStrong.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.easyToolStripMenuItem,
            this.middleToolStripMenuItem,
            this.hardToolStripMenuItem});
            this.menuStrong.Name = "menuStrong";
            this.menuStrong.Size = new System.Drawing.Size(152, 22);
            this.menuStrong.Text = "Strong";
            // 
            // easyToolStripMenuItem
            // 
            this.easyToolStripMenuItem.Name = "easyToolStripMenuItem";
            this.easyToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.easyToolStripMenuItem.Text = "Easy";
            // 
            // middleToolStripMenuItem
            // 
            this.middleToolStripMenuItem.Name = "middleToolStripMenuItem";
            this.middleToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.middleToolStripMenuItem.Text = "Middle";
            // 
            // hardToolStripMenuItem
            // 
            this.hardToolStripMenuItem.Name = "hardToolStripMenuItem";
            this.hardToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.hardToolStripMenuItem.Text = "Hard";
            // 
            // menuHelp
            // 
            this.menuHelp.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuAbout});
            this.menuHelp.Name = "menuHelp";
            this.menuHelp.Size = new System.Drawing.Size(59, 20);
            this.menuHelp.Text = "帮助(&H)";
            // 
            // menuAbout
            // 
            this.menuAbout.Name = "menuAbout";
            this.menuAbout.Size = new System.Drawing.Size(152, 22);
            this.menuAbout.Text = "关于(&A)";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sbInfo,
            this.pgbComputer});
            this.statusStrip1.Location = new System.Drawing.Point(0, 581);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(795, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // sbInfo
            // 
            this.sbInfo.AutoSize = false;
            this.sbInfo.Name = "sbInfo";
            this.sbInfo.Size = new System.Drawing.Size(300, 17);
            this.sbInfo.Text = "Gobang v0.9 by GaoHai";
            this.sbInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pgbComputer
            // 
            this.pgbComputer.AutoSize = false;
            this.pgbComputer.Name = "pgbComputer";
            this.pgbComputer.Size = new System.Drawing.Size(150, 16);
            // 
            // pic
            // 
            this.pic.Enabled = false;
            this.pic.Image = global::Gobang.Properties.Resources.GIF00041;
            this.pic.Location = new System.Drawing.Point(630, 133);
            this.pic.Name = "pic";
            this.pic.Size = new System.Drawing.Size(75, 75);
            this.pic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic.TabIndex = 2;
            this.pic.TabStop = false;
            this.pic.Visible = false;
            // 
            // btnStartNewGame
            // 
            this.btnStartNewGame.Location = new System.Drawing.Point(620, 457);
            this.btnStartNewGame.Name = "btnStartNewGame";
            this.btnStartNewGame.Size = new System.Drawing.Size(106, 26);
            this.btnStartNewGame.TabIndex = 4;
            this.btnStartNewGame.Text = "开始游戏";
            this.btnStartNewGame.UseVisualStyleBackColor = true;
            this.btnStartNewGame.Click += new System.EventHandler(this.btnStartNewGame_Click);
            // 
            // btnGoBack
            // 
            this.btnGoBack.Enabled = false;
            this.btnGoBack.Location = new System.Drawing.Point(620, 393);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.Size = new System.Drawing.Size(106, 26);
            this.btnGoBack.TabIndex = 5;
            this.btnGoBack.Text = "悔 子";
            this.btnGoBack.UseVisualStyleBackColor = true;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.WorkerReportsProgress = true;
            this.backgroundWorker.WorkerSupportsCancellation = true;
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker_RunWorkerCompleted);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
            // 
            // picMain
            // 
            this.picMain.Location = new System.Drawing.Point(12, 36);
            this.picMain.Name = "picMain";
            this.picMain.Size = new System.Drawing.Size(394, 413);
            this.picMain.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picMain.TabIndex = 6;
            this.picMain.TabStop = false;
            this.picMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.picMain_MouseClick);
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 3000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // skinEngine1
            // 
            this.skinEngine1.SerialNumber = "";
            this.skinEngine1.SkinFile = null;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(620, 517);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(106, 26);
            this.btnSave.TabIndex = 7;
            this.btnSave.Text = "存储棋局";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 603);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.picMain);
            this.Controls.Add(this.btnGoBack);
            this.Controls.Add(this.btnStartNewGame);
            this.Controls.Add(this.pic);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "欢乐五子棋";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.frmMain_Paint);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picMain)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuGame;
        private System.Windows.Forms.ToolStripMenuItem menuGameStart;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuExit;
        private System.Windows.Forms.ToolStripMenuItem menuSetup;
        private System.Windows.Forms.ToolStripMenuItem menuSetupSound;
        private System.Windows.Forms.ToolStripMenuItem menuSetupMusic;
        private System.Windows.Forms.ToolStripMenuItem menuHelp;
        private System.Windows.Forms.ToolStripMenuItem menuAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.PictureBox pic;
        private System.Windows.Forms.Button btnStartNewGame;
        private System.Windows.Forms.Button btnGoBack;
        private System.Windows.Forms.ToolStripMenuItem menuGameGoBack;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripStatusLabel sbInfo;
        private System.Windows.Forms.ToolStripProgressBar pgbComputer;
        private System.Windows.Forms.ToolStripMenuItem meunColor;
        private System.Windows.Forms.ToolStripMenuItem menuBlack;
        private System.Windows.Forms.ToolStripMenuItem menuWhite;
        private System.Windows.Forms.ToolStripMenuItem menuStrong;
        private System.Windows.Forms.ToolStripMenuItem easyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem middleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hardToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private System.Windows.Forms.PictureBox picMain;
        private System.Windows.Forms.Timer timer;
        private Sunisoft.IrisSkin.SkinEngine skinEngine1;
        private System.Windows.Forms.Button btnSave;
    }
}

