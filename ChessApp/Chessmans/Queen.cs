using System;
using System.Drawing;

namespace Chess.Chessmans
{
    public class Queen : Chessman
    {
        public Queen(string color) : base(color, "Queen")
        {
        }

        public static bool CheckQueenMove(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender)
        {
            var result = false;

            //Общие правила хода
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != Math.Abs(startY - finishY))
                    {
                        result = true;
                    }
                    else
                    {
                        result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "diagonal");
                    }
                }
                else
                {
                    result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "horizontal");
                }
            }
            else
            {
                result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "vertical");
            }

            //Проверяем наличие шахматы в конечной ячейке
            result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender, result);

            return result;
        }
    }
}