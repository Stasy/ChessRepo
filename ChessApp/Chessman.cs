using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Properties;


namespace Chess
{
    public class Chessman : PictureBox
    {
        public Chessman(string color, string chessmanType)
        {
            if (color == "white")
            {
                Image = (chessmanType == "Castle")
                    ? IconToBitmap(Resources.whiteCastle)
                    : (chessmanType == "Elephant")
                        ? IconToBitmap(Resources.whiteElephant)
                        : (chessmanType == "Horse")
                            ? IconToBitmap(Resources.whiteHorse)
                            : (chessmanType == "King")
                                ? IconToBitmap(Resources.whiteKing)
                                : (chessmanType == "Pawn")
                                    ? IconToBitmap(Resources.whitePawn)
                                    : IconToBitmap(Resources.whiteQueen);
            }
            else
            {
                Image = (chessmanType == "Castle")
                    ? IconToBitmap(Resources.blackCastle)
                    : (chessmanType == "Elephant")
                        ? IconToBitmap(Resources.blackElephant)
                        : (chessmanType == "Horse")
                            ? IconToBitmap(Resources.blackHorse)
                            : (chessmanType == "King")
                                ? IconToBitmap(Resources.blackKing)
                                : (chessmanType == "Pawn")
                                    ? IconToBitmap(Resources.blackPawn)
                                    : IconToBitmap(Resources.blackQueen);
            }

            BackColor = Color.Transparent;
            Location = new Point(2, 2);
            Size = new Size(46, 46);
        }

        ~Chessman()
        {
            Image = null;
            BackColor = Color.Gray;
            Size = new Size(0, 0);
        }

        private static Bitmap IconToBitmap(Icon icon)
        {
            var bmp = new Bitmap(46, 46);
            using (Graphics graphic = Graphics.FromImage(bmp))
            {
                graphic.DrawIcon(icon, 0, 0);
            }
            return bmp;
        }
    }
}