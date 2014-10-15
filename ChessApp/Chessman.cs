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
            ChessColor = color;
            FirstMove = true;
        }

        public string ChessColor { get; set; }
        public bool FirstMove { get; set; }

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

        public static bool CheckJumpOverChessman(int startX, int startY, int finishX, int finishY,
            bool[,] chessmanPresenceSign, string trajectorysType)
        {
            var result = false;

            if (trajectorysType == "horizontal")
            {
                result = CheckJumpOverChessmanForHorizontalTrajectory(startX, startY, finishX, finishY,
                    chessmanPresenceSign);
            }

            if (trajectorysType == "vertical")
            {
                result = CheckJumpOverChessmanForVerticalTrajectory(startX, startY, finishX, finishY,
                    chessmanPresenceSign);
            }

            if (trajectorysType == "diagonal")
            {
                result = CheckJumpOverChessmanForDiagonalTrajectory(startX, startY, finishX, finishY,
                    chessmanPresenceSign);
            }

            return result;
        }

        private static bool CheckJumpOverChessmanForHorizontalTrajectory(int startX, int startY, int finishX,
            int finishY, bool[,] chessmanPresenceSign)
        {
            var result = false;
            for (var i = 1; i < Math.Abs(startX - finishX); i++)
            {
                if ((startX - finishX) > 0 && chessmanPresenceSign[startY, startX - i] ||
                    (startX - finishX) < 0 && chessmanPresenceSign[startY, startX + i])
                {
                    result = true;
                }
            }

            return result;
        }

        private static bool CheckJumpOverChessmanForVerticalTrajectory(int startX, int startY, int finishX,
            int finishY, bool[,] chessmanPresenceSign)
        {
            var result = false;
            for (var i = 1; i < Math.Abs(startY - finishY); i++)
            {
                if ((startY - finishY) > 0 && chessmanPresenceSign[startY - i, startX] ||
                    (startY - finishY) < 0 && chessmanPresenceSign[startY + i, startX])
                {
                    result = true;
                }
            }

            return result;
        }

        private static bool CheckJumpOverChessmanForDiagonalTrajectory(int startX, int startY, int finishX,
            int finishY, bool[,] chessmanPresenceSign)
        {
            var result = false;
            if ((startX - finishX) == (startY - finishY))
            {
                //главная диагональ
                for (var i = 1; i < Math.Abs(startX - finishX); i++)
                {
                    if ((startX - finishX) > 0 && chessmanPresenceSign[startY - i, startX - i] ||
                        (startX - finishX) < 0 && chessmanPresenceSign[startY + i, startX + i])
                    {
                        result = true;
                    }
                }
            }
            else
            {
                //побочная диагональ
                for (var i = 1; i < Math.Abs(startX - finishX); i++)
                {
                    if ((startX - finishX) > 0 && chessmanPresenceSign[startY + i, startX - i] ||
                        (startX - finishX) < 0 && chessmanPresenceSign[startY - i, startX + i])
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        public static bool CheckFreeFinishCell(int X, int Y, bool[,] chessmanPresenceSign, ControlCollection Controls, object sender)
        {
            var result = false;
            foreach (var control in Controls)
            {
                if (control is Chessman)
                {
                    var chessFinishLocation = new Point(X * 50 + 27, Y * 50 + 27);

                    if (((Chessman)control).Location == chessFinishLocation)
                    {
                        if (((Chessman) control).ChessColor == ((Chessman) sender).ChessColor &&
                            control != sender)
                        {
                            result = true;
                        }
                        else
                        {
                            if(control != sender)
                            {
                                Controls.Remove((Chessman) control);
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}