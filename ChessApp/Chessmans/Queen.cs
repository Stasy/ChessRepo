﻿using System;
using System.Collections.Generic;

namespace Chess.Chessmans
{
    public class Queen : Chessman
    {
        public Queen(string color) : base(color, "Queen")
        {
        }

        public static bool CheckQueenMove(int startX,
            int startY,
            int finishX,
            int finishY,
            bool[,] chessmanPresenceSign,
            ControlCollection controls,
            object sender, 
            Dictionary<string, int> moveOrder)
        {
            var result = false;

            //Общие правила хода
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    if (Math.Abs(startX - finishX) != Math.Abs(startY - finishY))
                    {
                        result = true;
                    }
                    else
                    {
                        result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "diagonal");
                    }
                }
                else
                {
                    result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "horizontal");
                }
            }
            else
            {
                result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "vertical");
            }

            //Проверяем наличие шахматы в конечной ячейке
            if (result == false)
            {
                result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender);
            }

            //Проверка хода а месте
            if (startX == finishX && startY == finishY)
                result = true;

            //Проверка атаки на короля-союзника
            result = CheckAllyKingBeAttacked(startX, startY, finishX, finishY,
                chessmanPresenceSign, controls, sender, result);

            //Проверка правильного удаления шахматы
            if (result && ((Chessman)sender).RemoveChessman)
            {
                controls.Add(TemporaryChassman);
            }
            ((Chessman) sender).RemoveChessman = false;

            //Проверка шаха и мата
            if (!result && !((Chessman)sender).FakeCheck)
            {
                CheckEnemyKingBeAttaced(startX, startY, finishX, finishY,
                    chessmanPresenceSign, controls, sender, moveOrder);    
            }

            return result;
        }
    }
}