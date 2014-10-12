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

        public static bool CheckElephantMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
