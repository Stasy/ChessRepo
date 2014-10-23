using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Horse : Chessman
    {
        public Horse(string color) : base(color, "Horse")
        {
        }

        public static bool CheckHorseMove(int startX, 
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
            if (startX != finishX + 2 && startX != finishX - 2)
            {
                if (startX != finishX + 1 && startX != finishX - 1)
                {
                    result = true;
                }
                else
                {
                    if (startY != finishY + 2 && startY != finishY - 2)
                    {
                        result = true;
                    }
                }
            }
            else
            {
                if (startY != finishY + 1 && startY != finishY - 1)
                {
                    result = true;
                }
            }

            //Проверяем наличие шахматы в конечной ячейке
            if (result == false)
            {
                result = CheckFreeFinishCell(finishX, finishY, chessmanPresenceSign, controls, sender);
            }

            //Проверка атаки на короля-союзника
            result = CheckAllyKingBeAttacked(startX, startY, finishX, finishY,
                    chessmanPresenceSign, controls, sender, result);

            //Проверка шаха
            if (!result && !((Chessman) sender).FakeCheck)
            {
                CheckEnemyKingBeAttaced(startX, startY, finishX, finishY,
                    chessmanPresenceSign, controls, sender, moveOrder);
            }

            return result;
        }
    }
}
