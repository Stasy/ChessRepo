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

            if (((Chessman) sender).FirstMove)
            {
                if (((Chessman)sender).ChessColor == "white" && (startY != finishY + 1 && startY != finishY + 2)
                    || ((Chessman)sender).ChessColor == "black" && (startY != finishY - 1 && startY != finishY - 2))
                {
                    result = true;
                }
                else
                {
                    if (startX != finishX)
                    {
                        if (startY == finishY + 2 || startY == finishY - 2 ||
                            !chessmanPresenceSign[finishY, finishX] || Math.Abs(startX - finishX) > 1)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (chessmanPresenceSign[finishY, finishX])
                        {
                            result = true;
                        }
                        else
                        {
                            ((Chessman)sender).FirstMove = false;
                        }
                    }
                }
            }
            else
            {
                if (((Chessman)sender).ChessColor == "white" && startY != finishY + 1
                    || ((Chessman)sender).ChessColor == "black" && startY != finishY - 1)
                {
                    result = true;
                }
                else
                {
                    if (startX != finishX)
                    {
                        if (!chessmanPresenceSign[finishY, finishX] || Math.Abs(startX - finishX) > 1)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (chessmanPresenceSign[finishY, finishX])
                        {
                            result = true;
                        }
                    }
                }
            }

            //Проверяем наличие шахматы в конечной ячейке
            if (result == false)
            {
                result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender);
            }

            return result;
        }
    }
}