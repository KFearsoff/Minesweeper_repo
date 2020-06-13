using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Minesweeper
{
    public class Field
    {
        public Cell[,] Cells { get; private set; }
        public PictureBox[,] Pictures { get; private set; }
        private bool IsFirstClick = true;

        public Field(int rows, int columns)
        {
            //Генерируется матрица объектов типа Cell, которая отвечает за все внутреннее взаимодействие.
            Cells = new Cell[rows, columns];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                    Cells[i, j] = new Cell(i, j);

            //Генерируется матрица объектов типа PictureBox, которая отвечает за пользовательский интерфейс.
            Pictures = new PictureBox[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    //Генерируем сами объекты.
                    Pictures[i, j] = new PictureBox
                    {
                        //В имени кодируется строка и столбец.
                        Name = $"PictureBox{i}x{j}",
                        Size = new Size(32, 32),
                        //Позиция левого верхнего угла - (20, 30). 
                        Location = new Point(20 + j * 32, 30 + i * 32),
                        Visible = true,
                        BackColor = Color.DodgerBlue,
                        BorderStyle = BorderStyle.FixedSingle
                    };
                    //Два события, чтобы обрабатывать каждый клик мышкой вне зависимости от скорости.
                    Pictures[i, j].MouseClick += OnCellClick;
                    Pictures[i, j].MouseDoubleClick += OnCellClick;
                }
            }
        }

        //Метод, который вызывается при клике мышкой.
        public void OnCellClick(object sender, MouseEventArgs args)
        {
            var pictureBox = sender as PictureBox;
            //Достаем из имени PictureBox строку и столбец.
            var indexes = new Regex(@"\d+x\d+").Match(pictureBox.Name).Value.Split('x');
            int row = int.Parse(indexes[0]);
            int column = int.Parse(indexes[1]);
            //Если это первое нажатие, генерируем мины и определяем значения всех ячеек.
            if (IsFirstClick)
            {
                GenerateMines(Properties.Settings.Default.MineCount, row, column);
                CellAnalyzer.AssignAllValues(Cells);
                IsFirstClick = false;
            }
            //Если левая кнопка мыши - открываем ячейку. Правая - ставим флажок.
            if (args.Button == MouseButtons.Left && Cells[row, column].State == 0) Reveal(Cells[row, column]);
            else if (args.Button == MouseButtons.Right) Flag(Cells[row, column]);
        }

        //Генерирует мины. Примечание: метод вызывается не сразу, а после первого клика.
        //То есть взорваться с первого клика нельзя.
        public void GenerateMines(int mineCount, int row, int column)
        {
            //Включает таймер.
            Form1.timer.Start();
            Random rand = new Random();
            int cellCount = Cells.GetLength(0) * Cells.GetLength(1);
            for (int i = 0; i < Cells.GetLength(0); i++)
                for (int j = 0; j < Cells.GetLength(1); j++)
                    //Защищает кликнутую ячейку от мин.
                    if (i == row || j == column) cellCount--;
                    //Расставляет мины.
                    else if (rand.Next(1, cellCount + 1) <= mineCount)
                    {
                        Cells[i, j].Value = -1;
                        cellCount--;
                        mineCount--;
                    }
                    else cellCount--;
        }

        //Открывает переданную в аргумент ячейку.
        public void Reveal(Cell cell)
        {
            //Если ячейка открыта или помечена флажком, выход из метода.
            if (cell.State == 1 || cell.State == 2) return;
            //Ячейка помечается как открытая, фон становится белым.
            cell.State = 1;
            Pictures[cell.Row, cell.Column].BackColor = Color.White;
            //Если ячейка с миной, то вызывается метод Game.EndScreen(false).
            if (cell.Value == -1)
            {
                Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources.mine);
                Game.EndScreen(false);
            }
            //Если ячейка пустая, то рекурсивным алгоритмом открываются все ближайшие закрытые ячейки.
            if (cell.Value == 0) foreach (var c in CellAnalyzer.GetMatrix(Cells, cell.Row, cell.Column))
                    if (c.State == 0) Reveal(c);
            //Иначе ячейка просто открывается.
            if (cell.Value == 1) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._1);
            if (cell.Value == 2) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._2);
            if (cell.Value == 3) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._3);
            if (cell.Value == 4) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._4);
            if (cell.Value == 5) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._5);
            if (cell.Value == 6) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._6);
            if (cell.Value == 7) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._7);
            if (cell.Value == 8) Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources._8);
            //Наконец, отвязываются события. При нажатии на открытую ячейку ничего не происходит.
            Pictures[cell.Row, cell.Column].MouseClick -= OnCellClick;
            Pictures[cell.Row, cell.Column].MouseDoubleClick -= OnCellClick;
        }

        //Если ячейка скрыта, ставит флаг. Если на ячейке есть флаг, делает ее скрытой. Если флажков не осталось, выдается ошибка.
        public void Flag(Cell cell)
        {
            if (cell.State == 0)
            {
                if (Game.FlagsLeft == 0)
                {
                    MessageBox.Show("You don't have any flags left!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                cell.State = 2;
                Pictures[cell.Row, cell.Column].Image = new Bitmap(Properties.Resources.flag);
                Game.FlagsLeft--;
            }
            else
            {
                cell.State = 0;
                Pictures[cell.Row, cell.Column].Image = null;
                Game.FlagsLeft++;
            }
        }
    }
}
