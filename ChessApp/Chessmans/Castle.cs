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
            object sender)
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

            //Клонируем массив
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

            //Проверка возможности атак на короля
            foreach (var control in controls)
            {
                if (control is King)
                {
                    if (((Chessman)control).ChessColor == ((Chessman)sender).ChessColor)
                    {
                        var kingfinishX = (((King)control).Location.X - 27) / 50;
                        var kingFinishY = (((King)control).Location.Y - 27) / 50;

                        result = King.CheckKingBeAttacked(kingfinishX, kingFinishY, fakeChessmanPresenceSign,
                            controls, control, result);
                    }
                }
            }

            return result;
        }
    }
}
