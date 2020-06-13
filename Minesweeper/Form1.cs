using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form1 : Form
    {
        Game game;
        public static Timer timer;
        public static int Time { get; private set; }
        public static int Flags { get; set; }

        //Автоматически начинает новую игру.
        public Form1()
        {
            InitializeComponent();
            game = new Game(this);
            toolStripLabel2.Text = "Your flags: " + Properties.Settings.Default.MineCount;
        }

        //Начинает новую игру.
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            game = new Game(this);
            toolStripLabel1.Text = "Your time: 0";
            toolStripLabel2.Text = "Your flags: " + Properties.Settings.Default.MineCount;
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        //Парсит текст и обновляет текущее время.
        public void UpdateTime(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"\d+");
            Time = int.Parse(regex.Match(toolStripLabel1.Text).ToString()) + 1;
            toolStripLabel1.Text = "Your time: " + Time;
        }

        //Обновляет количество флажков.
        public void UpdateFlags(bool IsIncremented)
        {
            if (IsIncremented) Flags++;
            else Flags--;
            toolStripLabel2.Text = "Your flags: " + Flags;
        }
    }
}
