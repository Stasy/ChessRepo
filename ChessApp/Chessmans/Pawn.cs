using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Pawn : Chessman
    {
        public Pawn(string color) : base(color, "Pawn")
        {
        }

        public static bool CheckPawnMove(int startX, 
            int startY, 
            int finishX, 
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender)
        {
            //Общие правила хода
            var result = false;

            if (((Chessman)sender).ChessColor == "white" && startY != finishY + 1
                || ((Chessman)sender).ChessColor == "black" && startY != finishY - 1)
            {
                result = true;
            }
            else
            {
                if (startX != finishX)
                {
                    result = true;
                }
            }

            //Проверяем наличие шахматы в конечной ячейке
            result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender, result);

            return result;
        }
    }
}