using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public delegate void FlagHandler(bool IsIncremented);
    public class Game
    {
        //Если все клетки открыты или помечены флажками, игра заканчивается.
        private static int cellsLeft;
        public static int CellsLeft
        {
            get { return cellsLeft; }
            set
            {
                cellsLeft = value;
                if (cellsLeft == 0) EndScreen(true);
            }
        }
        //Если флажки кончились, то выдается ошибка (далее по коду). Событие обновляет количество флажков на форме.
        public static event FlagHandler FlagCountChanged;
        private static int flagsLeft;
        public static int FlagsLeft
        {
            get { return flagsLeft; }
            set
            {
                if (value < flagsLeft) FlagCountChanged?.Invoke(false);
                else FlagCountChanged?.Invoke(true);
                flagsLeft = value;
            }
        }


        public Game(object sender)
        {
            var t = sender as Form1;

            //Обновляет таймер.
            Form1.timer = new Timer { Interval = 1000 };
            Form1.timer.Stop();
            //По прошествии секунды обновляем прошедшее время на форме.
            Form1.timer.Tick += new EventHandler(t.UpdateTime);

            //Обновляет количество ячеек и флажков. 
            CellsLeft = Properties.Settings.Default.Rows * Properties.Settings.Default.Columns;
            FlagsLeft = Properties.Settings.Default.MineCount;
            Form1.Flags = Properties.Settings.Default.MineCount;
            //При каждом добавлении или удалении флажка отображаем это на форме.
            FlagCountChanged += new FlagHandler(t.UpdateFlags);

            //Удаляем все имеющиеся объекты типа PictureBox.
            int index = 0;
            while (index < t.Controls.Count)
                if (t.Controls[index] is PictureBox) t.Controls.RemoveAt(index);
                else index++;
            //Создаем новое поле и добавляем на форму каждый PictureBox.
            Field f = new Field(Properties.Settings.Default.Rows, Properties.Settings.Default.Columns);
            foreach (var c in f.Pictures) t.Controls.Add(c);
        }

        public static void EndScreen(bool IsWon)
        {
            Form1.timer.Stop();
            Task.Run(() => new Form3(IsWon).ShowDialog());
        }
    }
}
