using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class King : Chessman
    {
        public King(string color) : base(color, "King")
        {
        }

        public static bool CheckKingMove(int startX, int startY, int finishX, int finishY)
        {
            var result = false;
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != 1 && Math.Abs(startY - finishY) != 1)
                        result = true;
                }
                else
                {
                    if ((startX != finishX - 1 && startX != finishX + 1))
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (startY != finishY - 1 && startY != finishY + 1)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
