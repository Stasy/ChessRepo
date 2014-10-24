using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Chess.Chessmans;
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
            ShahSigne = false;
            MateSigne = false;
            FakeCheck = false;
            RemoveChessman = false;
            FakeLocation = new Point();
        }

        public string ChessColor { get; set; }
        public bool FirstMove { get; set; }
        public bool ShahSigne { get; set; }
        public bool MateSigne { get; set; }
        public bool FakeCheck { get; set; }
        public bool RemoveChessman { get; set; }
        public Point FakeLocation { get; set; }

        public static Chessman TemporaryChassman;

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

        public static bool CheckFreeFinishCell(int X, int Y, bool[,] chessmanPresenceSign, ControlCollection Controls,
            object sender)
        {
            var result = false;
            foreach (var control in Controls)
            {
                if (control is Chessman)
                {
                    var chessFinishLocation = new Point(X*50 + 27, Y*50 + 27);

                    if (((Chessman) control).Location == chessFinishLocation)
                    {
                        if (((Chessman) control).ChessColor == ((Chessman) sender).ChessColor &&
                            control != sender)
                        {
                            result = true;
                        }
                        else
                        {
                            if (control != sender && !((Chessman) sender).FakeCheck)
                            {
                                TemporaryChassman = ((Chessman) control);
                                Controls.Remove((Chessman) control);
                                ((Chessman) sender).RemoveChessman = true;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public static bool CheckAllyKingBeAttacked(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            bool result)
        {
            //Клонируем массив
            bool[,] fakeChessmanPresenceSign = new bool[8, 8];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    fakeChessmanPresenceSign[i, j] = chessmanPresenceSign[i, j];
                }
            }

            //Меняем координаты фигуры, как будто ход завершен
            fakeChessmanPresenceSign[startY, startX] = false;
            fakeChessmanPresenceSign[finishY, finishX] = true;

            //Проверка возможности атак на короля
            foreach (var control in controls)
            {
                if (control is King)
                {
                    if (((Chessman) control).ChessColor == ((Chessman) sender).ChessColor)
                    {
                        var kingfinishX = (((King) control).Location.X - 27)/50;
                        var kingFinishY = (((King) control).Location.Y - 27)/50;

                        result = King.CheckKingBeAttacked(kingfinishX, kingFinishY, fakeChessmanPresenceSign,
                            controls, control, result, sender);
                    }
                }
            }

            if (result)
            {
                fakeChessmanPresenceSign[startY, startX] = chessmanPresenceSign[startY, startX];
                fakeChessmanPresenceSign[finishY, finishX] = chessmanPresenceSign[finishY, finishX];
            }

            return result;
        }

        public static void CheckEnemyKingBeAttaced(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            Dictionary<string, int> moveOrder)
        {
            //Клонируем массив признака расположения шахмат, для фальсификации
            bool[,] fakeChessmanPresenceSign = new bool[8, 8];
            for (var i = 0; i < 8; i++)
            {
                for (var j = 0; j < 8; j++)
                {
                    fakeChessmanPresenceSign[i, j] = chessmanPresenceSign[i, j];
                }
            }
            fakeChessmanPresenceSign[startY, startX] = false;
            fakeChessmanPresenceSign[finishY, finishX] = true;

            //Клонируем свойство расположения шахмат для возможности фальсификации
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    ((Chessman) control).FakeLocation = ((Chessman) control).Location;
                }
            }

            //Проверка возможности шаха
            foreach (var control in controls)
            {
                if (control is King)
                {
                    if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                    {
                        var kingfinishX = (((King) control).Location.X - 27)/50;
                        var kingFinishY = (((King) control).Location.Y - 27)/50;

                        ((Chessman) control).FakeCheck = true;
                        ((Chessman) sender).ShahSigne = King.CheckKingBeAttacked(kingfinishX, kingFinishY,
                            fakeChessmanPresenceSign,
                            controls, control, false, sender);
                        ((Chessman) control).FakeCheck = false;
                    }
                }
            }


            //Проверка возможности мата
            if (((Chessman) sender).ShahSigne)
            {
                var flag = true;
                int chessStartX, chessStartY;
                bool result;

                //Клонируем свойство расположения шахмат для возможности фальсификации
                foreach (var control in controls)
                {
                    if (control is Chessman)
                    {
                        ((Chessman) control).FakeLocation = ((Chessman) control).Location;
                    }
                }

                foreach (var control in controls)
                {
                    if (control is Chessman && flag)
                    {
                        ((Chessman) sender).MateSigne = true;
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            result = true;

                            // Делаем фэйковые координаты
                            chessStartX = (((Chessman) control).Location.X - 27)/50;
                            chessStartY = (((Chessman) control).Location.Y - 27)/50;


                            if (control is King)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        var chessFinishX = i;
                                        var chessFinishY = j;

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = King.CheckKingMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, sender);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (control is Queen)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        //Задаем фэйковое положение
                                        var chessFinishX = i;
                                        var chessFinishY = j;
                                        ((Chessman) control).FakeLocation = new Point(chessFinishX*50 + 27,
                                            chessFinishY*50 + 27);

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = Queen.CheckQueenMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, moveOrder);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (control is Castle)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        var chessFinishX = i;
                                        var chessFinishY = j;
                                        ((Chessman) control).FakeLocation = new Point(chessFinishX*50 + 27,
                                            chessFinishY*50 + 27);

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = Castle.CheckCastleMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, moveOrder);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (control is Elephant)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        var chessFinishX = i;
                                        var chessFinishY = j;
                                        ((Chessman) control).FakeLocation = new Point(chessFinishX*50 + 27,
                                            chessFinishY*50 + 27);

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = Elephant.CheckElephantMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, moveOrder);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }


                            if (control is Horse)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        var chessFinishX = i;
                                        var chessFinishY = j;
                                        ((Chessman) control).FakeLocation = new Point(chessFinishX*50 + 27,
                                            chessFinishY*50 + 27);

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = Horse.CheckHorseMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, moveOrder);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (control is Pawn)
                            {
                                for (var i = 0; i < 8 && flag; i++)
                                {
                                    for (var j = 0; j < 8 && flag; j++)
                                    {
                                        var chessFinishX = i;
                                        var chessFinishY = j;
                                        ((Chessman) control).FakeLocation = new Point(chessFinishX*50 + 27,
                                            chessFinishY*50 + 27);

                                        if (chessFinishX != chessStartX || chessFinishY != chessStartY)
                                        {
                                            ((Chessman) control).FakeCheck = true;
                                            result = Pawn.CheckPawnMove(chessStartX, chessStartY, chessFinishX,
                                                chessFinishY,
                                                fakeChessmanPresenceSign, controls, control, moveOrder);
                                            ((Chessman) control).FakeCheck = false;

                                            if (!result)
                                            {
                                                flag = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!result)
                                ((Chessman) sender).MateSigne = false;
                        }
                    }
                }
            }
        }
    }
}