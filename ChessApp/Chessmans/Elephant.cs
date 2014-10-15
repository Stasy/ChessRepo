using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Elephant : Chessman
    {
        public Elephant(string color) : base(color, "Elephant")
        {
        }

        public static bool CheckElephantMove(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender)
        {
            var result = false;
            if (Math.Abs(startX - finishX) != Math.Abs(startY - finishY))
            {
                result = true;
            }
            else
            {
                result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "diagonal");
            }

            //Проверяем наличие шахматы в конечной ячейке
            result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender, result);

            return result;
        }
    }
}
