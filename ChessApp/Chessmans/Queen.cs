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
            bool[,] chessmanPresenceSign)
        {
            var result = false;
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != Math.Abs(startY - finishY))
                    {
                        result = true;
                    }
                }
            }
            else
            {
                result = CheckJumpOverChessman(startY, finishY, chessmanPresenceSign);
            }

            return result;
        }
    }
}