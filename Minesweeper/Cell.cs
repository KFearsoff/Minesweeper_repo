using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public delegate void CellHandler(int newState);
    public class Cell
    {
        private int state;
        public int State
        {
            get { return state; }
            set
            {
                state = value;
                //Каждый раз, обновляя значение ячейки, вызываем событие StateChanged.
                StateChanged?.Invoke(state);
            }
        }
        public int Value;
        public int Row { get; }
        public int Column { get; }
        public event CellHandler StateChanged;

        public Cell(int row, int column)
        {
            State = 0;
            Row = row;
            Column = column;
            StateChanged += UpdateCellCount;
        }

        //Обновляет количестве ячеек в классе Game.
        private void UpdateCellCount(int newState)
        {
            //Если с ячейки сняли флажок - инкрементируем количество ячеек.
            if (newState == 0) Game.CellsLeft++;
            //Если ячейку раскрыли или поставили на нее флажок - декрементируем количестве ячеек.
            if (newState == 1) Game.CellsLeft--;
            if (newState == 2) Game.CellsLeft--;
        }
    }
}
