using System.Drawing;

namespace Chess.Chessmans
{
    public class Queen : Chessman
    {
        public Queen(string color) : base(color, "Queen")
        {
        }

        public static bool CheckQueenMove(Point queenStartPosition, Point queenFinishPosition)
        {
            return true;
        }
    }
}
