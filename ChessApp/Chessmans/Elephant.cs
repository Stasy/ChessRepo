using System;
using System.Collections.Generic;
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

        public static bool CheckElephantMove(int startX, int startY, int finishX, int finishY)
        {
            bool result = Math.Abs(startX - finishX) != Math.Abs(startY - finishY);

            return result;
        }
    }
}
