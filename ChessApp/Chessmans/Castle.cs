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

        public static bool CheckCastleMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
