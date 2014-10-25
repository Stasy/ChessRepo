using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess.Chessmans
{
    public class King : Chessman
    {
        public King(string color) : base(color, "King")
        {
            CanCastling = true;
        }

        private bool CanCastling { get; set; }

        private static readonly bool[] FlagForCheckDiagonalAttack = new bool[4];

        public static bool CheckKingMove(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            object parentSender)
        {
            //Общие правила хода
            var result = false;
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != 1 || Math.Abs(startY - finishY) != 1)
                        result = true;
                }
                else
                {
                    //Проверка возможности рокировки
                    if (Math.Abs(startX - finishX) == 2 && ((King) sender).CanCastling && ((Chessman) sender).FirstMove &&
                        !((Chessman) sender).FakeCheck)
                    {
                        if ((startX - finishX) < 0)
                        {
                            var res = false;
                            res = CheckAllyKingBeAttacked(startX, startY, startX, startY, chessmanPresenceSign,
                                controls,
                                sender, res);
                            res = CheckAllyKingBeAttacked(startX, startY, startX + 1, startY, chessmanPresenceSign,
                                controls,
                                sender, res);
                            res = CheckAllyKingBeAttacked(startX, startY, startX + 2, startY, chessmanPresenceSign,
                                controls,
                                sender, res);

                            if (!res)
                            {
                                for (var i = 1; startX + i < 8; i ++)
                                {
                                    if (!chessmanPresenceSign[startY, startX + i])
                                    {
                                        foreach (var control in controls)
                                        {
                                            if (control is Castle)
                                            {
                                                if (((Castle) control).ChessColor == ((Chessman) sender).ChessColor &&
                                                    ((Castle) control).FirstMove && ((Castle) control).Location.X == 377)
                                                {
                                                    var startCasleX = (((Castle) control).Location.X - 27)/50;
                                                    var startCastleY = (((Castle) control).Location.Y - 27)/50;
                                                    chessmanPresenceSign[startCastleY, startCasleX] = false;
                                                    chessmanPresenceSign[finishY, finishX - 1] = true;
                                                    ((Castle) control).FirstMove = false;

                                                    ((Castle) control).Location = new Point((finishX - 1)*50 + 27,
                                                        (finishY)*50 + 27);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            var res = false;
                            res = CheckAllyKingBeAttacked(startX, startY, startX, startY, chessmanPresenceSign,
                                controls,
                                sender, res);
                            res = CheckAllyKingBeAttacked(startX, startY, startX - 1, startY, chessmanPresenceSign,
                                controls,
                                sender, res);
                            res = CheckAllyKingBeAttacked(startX, startY, startX - 2, startY, chessmanPresenceSign,
                                controls,
                                sender, res);

                            if (!res)
                            {
                                for (var i = 1; startX - i >= 0; i++)
                                {
                                    if (!chessmanPresenceSign[startY, startX - i])
                                    {
                                        foreach (var control in controls)
                                        {
                                            if (control is Castle)
                                            {
                                                if (((Castle) control).ChessColor == ((Chessman) sender).ChessColor &&
                                                    ((Castle) control).FirstMove && ((Castle) control).Location.X == 27)
                                                {
                                                    var startCasleX = (((Castle) control).Location.X - 27)/50;
                                                    var startCastleY = (((Castle) control).Location.Y - 27)/50;
                                                    chessmanPresenceSign[startCastleY, startCasleX] = false;
                                                    chessmanPresenceSign[finishY, finishX - 1] = true;
                                                    ((Castle) control).FirstMove = false;

                                                    ((Castle) control).Location = new Point((finishX + 1)*50 + 27,
                                                        (finishY)*50 + 27);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Math.Abs(startX - finishX) != 1)
                        {
                            result = true;
                        }
                    }
                }
            }
            else
            {
                if (Math.Abs(startY - finishY) != 1)
                {
                    result = true;
                }
            }

            //Проверяем наличие шахматы в конечной ячейке
            if (result == false)
            {
                result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender);
            }

            //Проверка возможности атак на короля
            result = CheckKingBeAttacked(finishX, finishY, chessmanPresenceSign, controls, sender, result, parentSender);

            //Проверка необходимости удалить шахмату

            if (!result)
            {
                foreach (var control in controls)
                {
                    if (control is Chessman)
                    {
                        if (((Chessman) control).Location == ((Chessman) sender).Location &&
                            ((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            controls.Remove((Chessman) control);
                        }
                    }
                }
            }

            return result;
        }

        public static bool CheckKingBeAttacked(int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            bool result,
            object parentSender)
        {
            var diagonalResult = CheckDiagonalAttack(finishX, finishY, chessmanPresenceSign, controls,
                sender, parentSender, result);
            var horizontalResult = CheckHorizontalAttack(finishX, finishY, chessmanPresenceSign,
                controls, sender, parentSender, result);
            var verticalResult = CheckVerticalAttack(finishX, finishY, chessmanPresenceSign, controls,
                sender, parentSender, result);
            var horsesResult = CheckHorsesAttack(finishX, finishY, controls, sender, parentSender, result);

            result = verticalResult
                ? verticalResult
                : horizontalResult
                    ? horizontalResult
                    : diagonalResult ? diagonalResult : horsesResult;

            return result;
        }

        private static bool CheckDiagonalAttack(
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            object parentSender,
            bool result)
        {
            FlagForCheckDiagonalAttack[0] =
                FlagForCheckDiagonalAttack[1] =
                    FlagForCheckDiagonalAttack[2] =
                        FlagForCheckDiagonalAttack[3] = true;

            for (var i = 1; (finishX + i) < 8 && (finishY + i) < 8 && FlagForCheckDiagonalAttack[0]; i++)
            {
                var checkPoint = new Point((finishX + i)*50 + 27, (finishY + i)*50 + 27);
                if (chessmanPresenceSign[finishY + i, finishX + i] && FlagForCheckDiagonalAttack[0])
                {
                    result = CheckCurrentDiagonal(checkPoint, controls, sender, parentSender, result, 0, i);
                }
            }


            for (var i = 1; (finishX - i) >= 0 && (finishY - i) >= 0 && FlagForCheckDiagonalAttack[1]; i++)
            {
                var finishPoint = new Point((finishX - i)*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX - i] && FlagForCheckDiagonalAttack[1])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, parentSender, result, 1, i);
                }
            }

            for (var i = 1; (finishX - i) >= 0 && (finishY + i) < 8 && FlagForCheckDiagonalAttack[2]; i++)
            {
                var finishPoint = new Point((finishX - i)*50 + 27, (finishY + i)*50 + 27);
                if (chessmanPresenceSign[finishY + i, finishX - i] && FlagForCheckDiagonalAttack[2])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, parentSender, result, 2, i);
                }
            }

            for (var i = 1; (finishX + i) < 8 && (finishY - i) >= 0 && FlagForCheckDiagonalAttack[3]; i++)
            {
                var finishPoint = new Point((finishX + i)*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX + i] && FlagForCheckDiagonalAttack[3])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, parentSender, result, 3, i);
                }
            }

            return result;
        }

        private static bool CheckHorizontalAttack(int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            object parentSender,
            bool result)
        {
            FlagForCheckDiagonalAttack[0] =
                FlagForCheckDiagonalAttack[1] =
                    FlagForCheckDiagonalAttack[2] =
                        FlagForCheckDiagonalAttack[3] = true;

            for (var i = 1; (finishX + i) < 8 && FlagForCheckDiagonalAttack[0]; i++)
            {
                var checkPoint = new Point((finishX + i)*50 + 27, finishY*50 + 27);
                if (chessmanPresenceSign[finishY, finishX + i] && FlagForCheckDiagonalAttack[0])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, parentSender, result, 0, i);
                }
            }

            for (var i = 1; (finishX - i) >= 0 && FlagForCheckDiagonalAttack[1]; i++)
            {
                var checkPoint = new Point((finishX - i)*50 + 27, finishY*50 + 27);
                if (chessmanPresenceSign[finishY, finishX - i] && FlagForCheckDiagonalAttack[1])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, parentSender, result, 1, i);
                }
            }

            return result;
        }

        private static bool CheckVerticalAttack(
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            object parentSender,
            bool result)
        {
            for (var i = 1; (finishY - i) >= 0 && FlagForCheckDiagonalAttack[2]; i++)
            {
                var checkPoint = new Point(finishX*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX] && FlagForCheckDiagonalAttack[2])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, parentSender, result, 2, i);
                }
            }

            for (var i = 1; (finishY + i) < 8 && FlagForCheckDiagonalAttack[3]; i++)
            {
                var checkPoint = new Point(finishX*50 + 27, (finishY + i)*50 + 27);
                if (chessmanPresenceSign[finishY + i, finishX] && FlagForCheckDiagonalAttack[3])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, parentSender, result, 3, i);
                }
            }
            return result;
        }

        private static bool CheckHorsesAttack(int finishX,
            int finishY,
            ControlCollection controls,
            object sender,
            object parentSender,
            bool result)
        {
            Point checkPoint;

            if ((finishX + 1) < 8)
            {
                if ((finishY + 2) < 8)
                {
                    checkPoint = new Point((finishX + 1)*50 + 27, (finishY + 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }

                if ((finishY - 2) >= 0)
                {
                    checkPoint = new Point((finishX + 1)*50 + 27, (finishY - 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }
            }

            if ((finishX - 1) >= 0)
            {
                if ((finishY + 2) < 8)
                {
                    checkPoint = new Point((finishX - 1)*50 + 27, (finishY + 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }

                if ((finishY - 2) >= 0)
                {
                    checkPoint = new Point((finishX - 1)*50 + 27, (finishY - 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }
            }

            if ((finishX + 2) < 8)
            {
                if ((finishY - 1) >= 0)
                {
                    checkPoint = new Point((finishX + 2)*50 + 27, (finishY + 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }

                if ((finishY + 1) < 8)
                {
                    checkPoint = new Point((finishX + 2)*50 + 27, (finishY - 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }
            }

            if ((finishX - 2) >= 0)
            {
                if ((finishY + 1) < 8)
                {
                    checkPoint = new Point((finishX - 2)*50 + 27, (finishY + 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }

                if ((finishY - 1) >= 0)
                {
                    checkPoint = new Point((finishX - 2)*50 + 27, (finishY - 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, parentSender, result);
                }
            }

            return result;
        }

        private static bool CheckCurrentDiagonal(Point checkPoint, ControlCollection controls, object sender,
            object parentSender,
            bool result, int currentDiagonal, int i)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint && !((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Queen || control is Elephant)
                            {
                                result = true;
                            }

                            if (control is Pawn && (Math.Abs(i) == 1) || control is King && (Math.Abs(i) == 1))
                            {
                                if (control is Pawn)
                                {
                                    if (((Pawn) control).ChessColor == "white" &&
                                        ((Pawn) control).Location.Y > ((Chessman) sender).Location.Y ||
                                        ((Pawn) control).ChessColor == "black" &&
                                        ((Pawn) control).Location.Y < ((Chessman) sender).Location.Y)
                                    {
                                        result = true;
                                    }
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }

                        FlagForCheckDiagonalAttack[currentDiagonal] = false;
                        break;
                    }

                    if (((Chessman) control).FakeLocation == checkPoint && ((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Queen || control is Elephant)
                            {
                                result = true;
                            }

                            if (control is Pawn && (Math.Abs(i) == 1) || control is King && (Math.Abs(i) == 1))
                            {
                                result = true;
                            }
                        }

                        FlagForCheckDiagonalAttack[currentDiagonal] = false;
                        break;
                    }
                }
            }
            return result;
        }

        private static bool CheckCurrentHorizontalOrVertical(Point checkPoint, ControlCollection controls, object sender,
            object parentSender,
            bool result, int currentDiagonalOrVertical, int i)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint && !((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Queen || control is Castle ||
                                control is King && (Math.Abs(i) == 1))
                            {
                                result = true;
                            }
                        }

                        FlagForCheckDiagonalAttack[currentDiagonalOrVertical] = false;
                        break;
                    }

                    if (((Chessman) control).FakeLocation == checkPoint && ((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Queen || control is Castle ||
                                control is King && (Math.Abs(i) == 1))
                            {
                                result = true;
                            }
                        }

                        FlagForCheckDiagonalAttack[currentDiagonalOrVertical] = false;
                        break;
                    }
                }
            }
            return result;
        }

        private static bool CheckCurrentHorse(Point checkPoint, ControlCollection controls, object sender,
            object parentSender,
            bool result)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint && !((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Horse)
                            {
                                result = true;
                            }
                        }
                        break;
                    }

                    if (((Chessman) control).FakeLocation == checkPoint && ((Chessman) parentSender).FakeCheck)
                    {
                        if (((Chessman) control).ChessColor != ((Chessman) sender).ChessColor)
                        {
                            if (control is Horse)
                            {
                                result = true;
                            }
                        }
                        break;
                    }
                }
            }
            return result;
        }
    }
}