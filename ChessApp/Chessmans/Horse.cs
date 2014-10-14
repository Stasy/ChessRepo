using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Horse : Chessman
    {
        public Horse(string color) : base(color, "Horse")
        {
        }

        public static bool CheckHorseMove(int startX, int startY, int finishX, int finishY)
        {
            var result = false;
            if (startX != finishX + 2 && startX != finishX - 2)
            {
                if (startX != finishX + 1 && startX != finishX - 1)
                {
                    result = true;
                }
                else
                {
                    if (startY != finishY + 2 && startY != finishY - 2)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (startY != finishY + 1 && startY != finishY - 1)
                {
                    result = true;
                }
            }

            return result;
        }
    }
}
