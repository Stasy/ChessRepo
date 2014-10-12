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

        public static bool CheckKingMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
