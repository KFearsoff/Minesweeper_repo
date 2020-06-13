using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public class CellAnalyzer
    {
        //Выдает матрицу из заданной ячейки и прилежащих к ней ячеек.
        public static Cell[,] GetMatrix(Cell[,] cells, int row, int column)
        {
            int lastRow = cells.GetLength(0) - 1;
            int lastColumn = cells.GetLength(1) - 1;
            //Если ячейка в углу, вызывается метод GetCornerMatrix.
            if (row == 0 && column == 0 || row == 0 && column == lastColumn ||
                row == lastRow && column == 0 || row == lastRow && column == lastColumn)
                return GetCornerMatrix(cells, row, column);
            //Если ячейка с краю, но не в углу, вызывается метод GetEdgeMatrix.
            else if (row == 0 || column == 0 || row == lastRow || column == lastColumn)
                return GetEdgeMatrix(cells, row, column);
            //Иначе возвращает матрицу 3х3 из заданной ячейки (в центре) и прилежащих к ней.
            else return new Cell[3, 3]
                    {{cells[row-1, column-1], cells[row-1, column], cells[row-1, column+1]},
                    {cells[row, column-1], cells[row, column], cells[row, column+1] },
                    {cells[row+1, column-1], cells[row+1, column], cells[row+1, column+1] }};

        }

        //Возвращает матрицу 2х2, если ячейка в углу.
        private static Cell[,] GetCornerMatrix(Cell[,] cells, int row, int column)
        {
            int lastRow = cells.GetLength(0) - 1;
            int lastColumn = cells.GetLength(1) - 1;
            //Левый верхний угол.
            if (row == 0 && column == 0) return new Cell[2, 2]
            { { cells[0, 0], cells[0, 1] },
            { cells[1, 0], cells[1, 1] } };
            //Правый верхний угол.
            else if (row == 0 && column == lastColumn) return new Cell[2, 2]
            { { cells[0, column-1], cells[0, column]},
            {cells[1, column-1], cells[1, column] }};
            //Левый нижний угол.
            else if (row == lastRow && column == 0) return new Cell[2, 2]
                    {{cells[row-1, 0], cells[row-1,1]},
                    {cells[row,0],cells[row,1]}};
            //Правый нижний угол.
            else return new Cell[2, 2]
                    {{cells[row-1, column-1], cells[row-1, column]},
                    {cells[row, column-1], cells[row, column]}};
        }

        //Возвращает матрицу 2х3 или 3х2, если ячейка с краю, но не в углу.
        private static Cell[,] GetEdgeMatrix(Cell[,] cells, int row, int column)
        {
            int lastRow = cells.GetLength(0) - 1;
            int lastColumn = cells.GetLength(1) - 1;
            //Левый край.
            if (column == 0) return new Cell[3, 2]
                    {{cells[row-1, 0], cells[row-1, 1]},{cells[row, 0], cells[row, 1]}, {cells[row+1,0], cells[row+1, 1]}};
            //Правый край.
            else if (column == lastColumn) return new Cell[3, 2]
                    {{cells[row-1, column-1], cells[row-1,column]},
                    {cells[row, column-1], cells[row, column] },
                    { cells[row+1, column-1], cells[row+1, column]} };
            //Верхний край.
            else if (row == 0) return new Cell[2, 3]
                        {{cells[0, column-1], cells[0, column], cells[0, column+1]},
                    {cells[1, column-1], cells[1, column], cells[1, column+1] }};
            //Нижний край.
            else return new Cell[2, 3]
                    {{cells[row-1, column-1], cells[row-1, column], cells[row-1, column+1]},
                    {cells[row, column-1], cells[row, column], cells[row, column+1] }};
        }

        //Присваивает ячейке значение, соответствующее количеству мин вокруг нее.
        public static void AssignValue(Cell[,] cells, int row, int column)
        {
            //Если в ячейке мина - выход из функции.
            if (cells[row, column].Value == -1) return;
            //Получаем матрицу для ячейки.
            var matrix = GetMatrix(cells, row, column);
            //Подсчитываем количество мин в этой матрице.
            int mineCount = 0;
            foreach (var c in matrix)
                if (c.Value == -1) mineCount++;
            //Присваиваем ячейке это значение.
            cells[row, column].Value = mineCount;
        }

        //Присваивает всем ячейкам значения, вызывая метод AssignValue для каждой ячейки.
        public static void AssignAllValues(Cell[,] cells)
        {
            for (int row = 0; row < cells.GetLength(0); row++)
                for (int column = 0; column < cells.GetLength(1); column++)
                    AssignValue(cells, row, column);
        }
    }
}
