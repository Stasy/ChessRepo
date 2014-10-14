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

        public static bool CheckPawnMove(int startX, int startY, int finishX, int finishY, Pawn pawn)
        {
            var result = false;

            if (pawn.ChessColor == "white" && startY != finishY + 1
                || pawn.ChessColor == "black" && startY != finishY - 1)
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

            return result;
        }
    }
}