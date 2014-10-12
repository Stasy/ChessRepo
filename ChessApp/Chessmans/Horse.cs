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

        public static bool CheckHorseMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
