using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
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

        private static readonly bool[] FlagForCheckDiagonalAttack = new bool[4];

        public static bool CheckKingMove(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender)
        {
            //Общие правила хода
            var result = false;
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != 1 && Math.Abs(startY - finishY) != 1)
                        result = true;
                }
                else
                {
                    if ((startX != finishX - 1 && startX != finishX + 1))
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (startY != finishY - 1 && startY != finishY + 1)
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
            result = CheckKingBeAttacked(finishX, finishY, chessmanPresenceSign, controls, sender,
                result);

            return result;
        }

        public static bool CheckKingBeAttacked(int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            bool result)
        {
            var diagonalResult = CheckDiagonalAttack(finishX, finishY, chessmanPresenceSign, controls,
                sender, result);
            var horizontalResult = CheckHorizontalAttack(finishX, finishY, chessmanPresenceSign,
                controls, sender, result);
            var verticalResult = CheckVerticalAttack(finishX, finishY, chessmanPresenceSign, controls,
                sender, result);
            var horsesResult = CheckHorsesAttack(finishX, finishY, controls, sender, result);

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
                    result = CheckCurrentDiagonal(checkPoint, controls, sender, result, 0, i);
                }
            }


            for (var i = 1; (finishX - i) >= 0 && (finishY - i) >= 0 && FlagForCheckDiagonalAttack[1]; i++)
            {
                var finishPoint = new Point((finishX - i)*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX - i] && FlagForCheckDiagonalAttack[1])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, result, 1, i);
                }
            }

            for (var i = 1; (finishX - i) >= 0 && (finishY + i) < 8 && FlagForCheckDiagonalAttack[2]; i++)
            {
                var finishPoint = new Point((finishX - i)*50 + 27, (finishY + i)*50 + 27);
                if (chessmanPresenceSign[finishY + i, finishX - i] && FlagForCheckDiagonalAttack[2])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, result, 2, i);
                }
            }

            for (var i = 1; (finishX + i) < 8 && (finishY - i) >= 0 && FlagForCheckDiagonalAttack[3]; i++)
            {
                var finishPoint = new Point((finishX + i)*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX + i] && FlagForCheckDiagonalAttack[3])
                {
                    result = CheckCurrentDiagonal(finishPoint, controls, sender, result, 3, i);
                }
            }

            return result;
        }

        private static bool CheckHorizontalAttack(int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
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
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, result, 0, i);
                }
            }

            for (var i = 1; (finishX - i) >= 0 && FlagForCheckDiagonalAttack[1]; i++)
            {
                var checkPoint = new Point((finishX - i)*50 + 27, finishY*50 + 27);
                if (chessmanPresenceSign[finishY, finishX - i] && FlagForCheckDiagonalAttack[1])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, result, 1, i);
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
            bool result)
        {
            for (var i = 1; (finishY - i) >= 0 && FlagForCheckDiagonalAttack[2]; i++)
            {
                var checkPoint = new Point(finishX*50 + 27, (finishY - i)*50 + 27);
                if (chessmanPresenceSign[finishY - i, finishX] && FlagForCheckDiagonalAttack[2])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, result, 2, i);
                }
            }

            for (var i = 1; (finishY + i) < 8 && FlagForCheckDiagonalAttack[3]; i++)
            {
                var checkPoint = new Point(finishX*50 + 27, (finishY + i)*50 + 27);
                if (chessmanPresenceSign[finishY + i, finishX] && FlagForCheckDiagonalAttack[3])
                {
                    result = CheckCurrentHorizontalOrVertical(checkPoint, controls, sender, result, 3, i);
                }
            }
            return result;
        }

        private static bool CheckHorsesAttack(int finishX,
            int finishY,
            ControlCollection controls,
            object sender,
            bool result)
        {
            Point checkPoint;

            if ((finishX + 1) < 8)
            {
                if ((finishY + 2) < 8)
                {
                    checkPoint = new Point((finishX + 1)*50 + 27, (finishY + 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }

                if ((finishY - 2) >= 0)
                {
                    checkPoint = new Point((finishX + 1)*50 + 27, (finishY - 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }
            }

            if ((finishX - 1) >= 0)
            {
                if ((finishY + 2) < 8)
                {
                    checkPoint = new Point((finishX - 1)*50 + 27, (finishY + 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }

                if ((finishY - 2) >= 0)
                {
                    checkPoint = new Point((finishX - 1)*50 + 27, (finishY - 2)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }
            }

            if ((finishX + 2) < 8)
            {
                if ((finishY - 1) >= 0)
                {
                    checkPoint = new Point((finishX + 2)*50 + 27, (finishY + 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }

                if ((finishY + 1) < 8)
                {
                    checkPoint = new Point((finishX + 2)*50 + 27, (finishY - 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }
            }

            if ((finishX - 2) >= 0)
            {
                if ((finishY + 1) < 8)
                {
                    checkPoint = new Point((finishX - 2)*50 + 27, (finishY + 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }

                if ((finishY - 1) >= 0)
                {
                    checkPoint = new Point((finishX - 2)*50 + 27, (finishY - 1)*50 + 27);
                    result = CheckCurrentHorse(checkPoint, controls, sender, result);
                }
            }

            return result;
        }

        private static bool CheckCurrentDiagonal(Point checkPoint, ControlCollection controls, object sender,
            bool result, int currentDiagonal, int i)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint)
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
            bool result, int currentDiagonalOrVertical, int i)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint)
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
            bool result)
        {
            foreach (var control in controls)
            {
                if (control is Chessman)
                {
                    if (((Chessman) control).Location == checkPoint)
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