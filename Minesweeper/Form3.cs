using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Form3 : Form
    {
        public Form3(bool IsWon)
        {
            InitializeComponent();
            if (IsWon) textBox1.Text = "Congratulations! You have won!";
            else textBox1.Text = "Unfortunately, you have lost.";
            textBox1.Text += Environment.NewLine + $"Your time is: {Form1.Time}";
            textBox1.Text += Environment.NewLine + $"Do you want to proceed or quit?";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
