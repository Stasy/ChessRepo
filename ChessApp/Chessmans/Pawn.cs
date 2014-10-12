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

        public static bool CheckPawnMove(int startX, int startY, int finishX, int finishY)
        {
            var result = false;
            if (startY != finishY + 1)
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
