using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Castle : Chessman
    {
        public Castle(string color) : base(color, "Castle")
        {
        }

        public static bool CheckCastleMove(int startX, 
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
            if (startX != finishX)
            {
                if (startY != finishY)
                {
                    result = true;
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
            ((Chessman)sender).RemoveChessman = false;
           
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
