using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Castle : Chessman
    {
        public Castle(string color) : base(color, "Castle")
        {
        }

        public static bool CheckCastleMove(int startX, int startY, int finishX, int finishY, bool[,] ChessmanPresenceSign)
        {
            var result = false;
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    result = true;
                }
                else
                {
                    result = CheckJumpOverChessman(startX, startY, finishX, finishY, ChessmanPresenceSign, "horizontal");
                }
            }
            else
            {
                result = CheckJumpOverChessman(startX, startY, finishX, finishY, ChessmanPresenceSign, "vertical");
            }

            return result;
        }
    }
}
