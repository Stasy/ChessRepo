using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Chessmans
{
    public class Elephant : Chessman
    {
        public Elephant(string color) : base(color, "Elephant")
        {
        }

        public static bool CheckElephantMove(int startX,
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
            if (Math.Abs(startX - finishX) != Math.Abs(startY - finishY))
            {
                result = true;
            }
            else
            {
                result = CheckJumpOverChessman(startX, startY, finishX, finishY, chessmanPresenceSign, "diagonal");
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
