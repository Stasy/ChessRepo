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

        public static bool CheckPawnMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
