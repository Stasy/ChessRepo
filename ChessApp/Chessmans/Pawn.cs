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
            FirstJump = "canJump";
            MoveOrderNumber = 0;
            CanTransform = false;
        }

        private string FirstJump { get; set; }
        private int MoveOrderNumber { get; set; }
        public bool CanTransform { get; set; }

        public static bool CheckPawnMove(int startX, 
            int startY, 
            int finishX, 
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender,
            Dictionary<string, int> moveOrder)
        {
            //Общие правила хода
            var result = false;
            var needCheckFreeFinishCell = true;

            if (((Chessman) sender).FirstMove)
            {
                if (((Chessman)sender).ChessColor == "white" && (startY != finishY + 1 && startY != finishY + 2)
                    || ((Chessman)sender).ChessColor == "black" && (startY != finishY - 1 && startY != finishY - 2))
                {
                    result = true;
                }
                else
                {
                    if (startX != finishX)
                    {
                        if (startY == finishY + 2 || startY == finishY - 2 ||
                            !chessmanPresenceSign[finishY, finishX] || Math.Abs(startX - finishX) > 1)
                        {
                            result = true;
                        }
                    }
                    else
                    {
                        if (chessmanPresenceSign[finishY, finishX])
                        {
                            result = true;
                        }
                        else
                        {
                            if (((Chessman) sender).FirstMove && (startY == finishY + 2 || startY == finishY - 2))
                            {
                                ((Pawn) sender).FirstJump = "firstJump";
                                foreach (var control in controls)
                                {
                                    if (control is Pawn)
                                    {
                                        ((Pawn)control).MoveOrderNumber = moveOrder[((Pawn)sender).ChessColor];
                                    } 
                                }
                            }
                            else
                            {
                                ((Pawn) sender).FirstJump = "cantJump";
                            }
                        }
                    }
                }
            }
            else
            {
                if (((Chessman)sender).ChessColor == "white" && startY != finishY + 1
                    || ((Chessman)sender).ChessColor == "black" && startY != finishY - 1)
                {
                    result = true;
                }
                else
                {
                    if (startX != finishX)
                    {
                        if (Math.Abs(startX - finishX) > 1)
                        {
                            result = true;
                        }
                        else
                        {
                            if (!chessmanPresenceSign[finishY, finishX])
                            {
                                //Проверяем "убийство на проходе"
                                foreach (var control in controls)
                                {
                                    if (control is Pawn)
                                    {
                                        if (((Pawn)control).ChessColor != ((Chessman)sender).ChessColor && ((Pawn)control).FirstJump == "firstJump")
                                        {
                                            var pointForBlackVictim = new Point(finishX * 50 + 27, (finishY + 1) * 50 + 27);
                                            var pointForWhiteVictim = new Point(finishX * 50 + 27, (finishY - 1) * 50 + 27);
                                            if (((Pawn) control).Location == pointForBlackVictim &&
                                                ((Chessman) sender).ChessColor == "white" ||
                                                ((Pawn) control).Location == pointForWhiteVictim &&
                                                ((Chessman) sender).ChessColor == "black")
                                            {
                                                if (moveOrder.Values.Min() == ((Pawn) sender).MoveOrderNumber - 2)
                                                {
                                                    needCheckFreeFinishCell = false;
                                                    controls.Remove((Chessman) control);
                                                }
                                                else
                                                {
                                                    result = true;
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
                        if (chessmanPresenceSign[finishY, finishX])
                        {
                            result = true;
                        }
                    }
                }
            }

            //Проверяем наличие шахматы в конечной ячейке
            if (result == false)
            {
                if (needCheckFreeFinishCell)
                {
                    result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender);
                }

                //Проверка первого хода
                if (!result)
                {
                    if (((Chessman)sender).FirstMove)
                    {
                        ((Chessman)sender).FirstMove = false;
                    }
                    else
                    {
                        if (((Pawn)sender).FirstJump == "firstJump")
                        {
                            ((Pawn)sender).FirstJump = "cantJump";
                        }
                    }
                }
            }

            //Признак возможности превращения пешки
            if (((Pawn)sender).ChessColor == "white" && finishY == 0 ||
                     ((Pawn)sender).ChessColor == "black" && finishY == 7)
                {
                    ((Pawn) sender).CanTransform = true;
                }

            return result;
        }
    }
}