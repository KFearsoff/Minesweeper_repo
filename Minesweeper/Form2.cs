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
    public partial class Form2 : Form
    {
        //Берет индекс прежде выбранного опции и автоматически ее выбирает.
        public Form2()
        {
            InitializeComponent();
            switch (Properties.Settings.Default.SelectedRadioButton)
            {
                case 1:
                    radioButton1.Checked = true;
                    break;
                case 2:
                    radioButton2.Checked = true;
                    break;
                case 3:
                    radioButton3.Checked = true;
                    break;
                case 4:
                    radioButton4.Checked = true;
                    break;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MineCount = 10;
            Properties.Settings.Default.Rows = 9;
            Properties.Settings.Default.Columns = 9;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MineCount = 40;
            Properties.Settings.Default.Rows = 16;
            Properties.Settings.Default.Columns = 16;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.MineCount = 99;
            Properties.Settings.Default.Rows = 16;
            Properties.Settings.Default.Columns = 32;
        }

        //Включает три текстовых окна для заполнения и автоматически заполняет их текущими настройками.
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Enabled = true;
            textBox2.Enabled = true;
            textBox3.Enabled = true;
            textBox1.Text = Properties.Settings.Default.Rows.ToString();
            textBox2.Text = Properties.Settings.Default.Columns.ToString();
            textBox3.Text = Properties.Settings.Default.MineCount.ToString();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Если какие-либо данные введены неправильно, то выскочит окно с ошибкой.
            if (radioButton4.Checked == true)
            {
                int temp;
                if (int.TryParse(textBox1.Text, out temp) && temp > 0 && temp < 51)
                    Properties.Settings.Default.Rows = temp;
                else MessageBox.Show("Invalid rows input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (int.TryParse(textBox2.Text, out temp) && temp > 0 && temp < 51)
                    Properties.Settings.Default.Columns = temp;
                else MessageBox.Show("Invalid columns input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (int.TryParse(textBox2.Text, out temp) && temp > 0 && temp < 1001)
                    Properties.Settings.Default.Columns = temp;
                else MessageBox.Show("Invalid mines input!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Properties.Settings.Default.Save();
            Close();
        }
    }
}
