using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Chess.Chessmans;
using Chess.Properties;

namespace Chess
{
    public partial class ChessDesk : Form
    {
        private const int CellsCountInRow = 8;
        public Point[,] CellsPositions = new Point[CellsCountInRow, CellsCountInRow];
        public bool[,] ChessmanPresenceSign = new bool[8, 8];
        public Label[] Orders = new Label[CellsCountInRow*2];

        public Dictionary<string, Castle> Castles = new Dictionary<string, Castle>();
        public Dictionary<string, Elephant> Elephants = new Dictionary<string, Elephant>();
        public Dictionary<string, Horse> Horses = new Dictionary<string, Horse>();
        public Dictionary<string, King> Kings = new Dictionary<string, King>();
        public Dictionary<string, Pawn> Pawns = new Dictionary<string, Pawn>();
        public Dictionary<string, Queen> Queens = new Dictionary<string, Queen>();

        private bool chessmanCanMove;
        private Point initialMouseLocation;
        private Point initialChessmanLocation;
        private Chessman chess;
        private int chessmansCellIndexColumn, chessmansCellIndexRow;

        private Dictionary<string, int> moveOrder = new Dictionary<string, int>();

        public ChessDesk()
        {
            InitializeComponent();
        }

        private void ChessDesk_Load(object sender, EventArgs e)
        {
            SuspendLayout();

            //Set ChessDesk properties
            BackgroundImage = Resources.background;
            FormBorderStyle = FormBorderStyle.FixedDialog;


            //Create cellsPositions and Orders
            for (var i = 0; i < CellsCountInRow; i++)
            {
                for (var j = 0; j < CellsCountInRow; j++)
                {
                    CellsPositions[i, j].X = j*50 + 27;
                    CellsPositions[i, j].Y = i*50 + 27;

                    ChessmanPresenceSign[i, j] = false;
                }
            }
            AssigneCellsOrder();

            //Create Chessmans
            string[] chessmansColors = {"black", "white"};
            foreach (var color in chessmansColors)
            {
                Kings.Add(color + "King", new King(color));
                Controls.Add(Kings[color + "King"]);

                Queens.Add(color + "Queen", new Queen(color));
                Controls.Add(Queens[color + "Queen"]);

                for (var chessmanIndex = 1; chessmanIndex <= 2; chessmanIndex++)
                {
                    Castles.Add(color + "Castle" + chessmanIndex, new Castle(color));
                    Controls.Add(Castles[color + "Castle" + chessmanIndex]);

                    Elephants.Add(color + "Elephant" + chessmanIndex, new Elephant(color));
                    Controls.Add(Elephants[color + "Elephant" + chessmanIndex]);

                    Horses.Add(color + "Horse" + chessmanIndex, new Horse(color));
                    Controls.Add(Horses[color + "Horse" + chessmanIndex]);
                }

                for (var pawnIndex = 1; pawnIndex <= 8; pawnIndex++)
                {
                    Pawns.Add(color + "Pawn" + pawnIndex, new Pawn(color));
                    Controls.Add(Pawns[color + "Pawn" + pawnIndex]);
                }
            }

            //Подключение обработки событий для объекта "шахмата" 
            foreach (var control in Controls)
            {
                if (control is Chessman)
                {
                    ((Chessman) control).MouseLeave += Chessman_MouseLeave;
                    ((Chessman) control).MouseDown += Chessman_MouseDown;
                    ((Chessman) control).MouseUp += Chessman_MouseUp;
                    ((Chessman) control).MouseMove += Chessman_MouseMove;
                }
            }

            ChassmansInitialDisposition();

            //Set default move order
            moveOrder.Add("white", 1);
            moveOrder.Add("black", 0);

            ResumeLayout(true);
        }

        private void Chessman_MouseLeave(object sender, EventArgs e)
        {
            chessmanCanMove = false;
        }

        private void Chessman_MouseDown(object sender, MouseEventArgs e)
        {
            chessmanCanMove = true;
            initialMouseLocation = e.Location;

            chess = (Chessman) sender;
            initialChessmanLocation = chess.Location;
        }

        private void Chessman_MouseUp(object sender, MouseEventArgs e)
        {
            chessmanCanMove = false;
            chess = (Chessman) sender;

            //Выравнивание положения шахматы в ячейке
            for (var j = 0; j < CellsCountInRow; j++)
            {
                if (chess.Location.X >= CellsPositions[0, j].X - 25 && chess.Location.X < CellsPositions[0, j].X + 25)
                {
                    chessmansCellIndexColumn = j;
                }
            }
            for (var i = 0; i < CellsCountInRow; i++)
            {
                if (chess.Location.Y >= CellsPositions[i, 0].Y - 25 && chess.Location.Y < CellsPositions[i, 0].Y + 25)
                {
                    chessmansCellIndexRow = i;
                }
            }
            chess.Location = CellsPositions[chessmansCellIndexRow, chessmansCellIndexColumn];

            //Проверка очередности хода
            if (moveOrder[((Chessman) sender).ChessColor] == moveOrder.Values.Max())
            {
                //Проверка логики хода
                CheckChessMove(chess);
            }
            else
            {
                MessageBox.Show(Resources.moveOrderMessageError);
                ((Chessman) sender).Location = initialChessmanLocation;
            }
        }

        private void Chessman_MouseMove(object sender, MouseEventArgs e)
        {
            if (chessmanCanMove)
            {
                chess = (Chessman) sender;
                chess.Top += e.Y - initialMouseLocation.Y;
                chess.Left += e.X - initialMouseLocation.X;
            }
        }

        private void ChessDesk_Paint(object sender, PaintEventArgs e)
        {
            Brush blackBrush = new TextureBrush(Resources.blackCell);
            Brush whiteBrush = new TextureBrush(Resources.grayCell);

            for (var i = 0; i < CellsCountInRow; i++)
            {
                for (var j = 0; j < CellsCountInRow; j++)
                {
                    var brush = ((i + j)%2) == 0 ? whiteBrush : blackBrush;
                    e.Graphics.FillRectangle(brush, CellsPositions[i, j].X - 2, CellsPositions[i, j].Y - 2, 50, 50);
                }
            }
        }

        private void ChessDesk_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Set ChessDesk properties to default
            ClientSize = new Size(284, 262);
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            BackgroundImage = null;
            FormBorderStyle = FormBorderStyle.Sizable;
            BackColor = Color.Gray;

            DeleteCellsOrder();
        }

        private void AssigneCellsOrder()
        {
            const int firstNumberCode = 56;
            const int firstLetterCode = 96;

            for (var i = 0; i < CellsCountInRow*2; i++)
            {
                int orderCode;
                int positionX, positionY;

                if (i < 8)
                {
                    positionX = 7;
                    positionY = i*50 + 42;
                    orderCode = firstNumberCode - i;
                }
                else
                {
                    positionX = (i - 7)*50 - 5;
                    positionY = 430;
                    orderCode = firstLetterCode + (i - 7);
                }

                Orders[i] = new Label
                {
                    BackColor = Color.Transparent,
                    Size = new Size(16, 16),
                    Location = new Point(positionX, positionY),
                    Text = Convert.ToChar(orderCode) + "",
                    ForeColor = Color.DarkGray,
                    Font = new Font(Font, FontStyle.Bold)
                };

                Controls.Add(Orders[i]);
            }
        }

        private void ChassmansInitialDisposition()
        {
            Castles["whiteCastle1"].Location = CellsPositions[7, 0];
            ChessmanPresenceSign[7, 0] = true;
            Castles["whiteCastle2"].Location = CellsPositions[7, 7];
            ChessmanPresenceSign[7, 7] = true;
            Castles["blackCastle1"].Location = CellsPositions[0, 0];
            ChessmanPresenceSign[0, 0] = true;
            Castles["blackCastle2"].Location = CellsPositions[0, 7];
            ChessmanPresenceSign[0, 7] = true;

            Horses["whiteHorse1"].Location = CellsPositions[7, 1];
            ChessmanPresenceSign[7, 1] = true;
            Horses["whiteHorse2"].Location = CellsPositions[7, 6];
            ChessmanPresenceSign[7, 6] = true;
            Horses["blackHorse1"].Location = CellsPositions[0, 1];
            ChessmanPresenceSign[0, 1] = true;
            Horses["blackHorse2"].Location = CellsPositions[0, 6];
            ChessmanPresenceSign[0, 6] = true;

            Elephants["whiteElephant1"].Location = CellsPositions[7, 2];
            ChessmanPresenceSign[7, 2] = true;
            Elephants["whiteElephant2"].Location = CellsPositions[7, 5];
            ChessmanPresenceSign[7, 5] = true;
            Elephants["blackElephant1"].Location = CellsPositions[0, 2];
            ChessmanPresenceSign[0, 2] = true;
            Elephants["blackElephant2"].Location = CellsPositions[0, 5];
            ChessmanPresenceSign[0, 5] = true;

            Queens["whiteQueen"].Location = CellsPositions[7, 3];
            ChessmanPresenceSign[7, 3] = true;
            Queens["blackQueen"].Location = CellsPositions[0, 3];
            ChessmanPresenceSign[0, 4] = true;

            Kings["whiteKing"].Location = CellsPositions[7, 4];
            ChessmanPresenceSign[7, 4] = true;
            Kings["blackKing"].Location = CellsPositions[0, 4];
            ChessmanPresenceSign[0, 3] = true;

            Pawns["whitePawn1"].Location = CellsPositions[6, 0];
            ChessmanPresenceSign[6, 0] = true;
            Pawns["whitePawn2"].Location = CellsPositions[6, 1];
            ChessmanPresenceSign[6, 1] = true;
            Pawns["whitePawn3"].Location = CellsPositions[6, 2];
            ChessmanPresenceSign[6, 2] = true;
            Pawns["whitePawn4"].Location = CellsPositions[6, 3];
            ChessmanPresenceSign[6, 3] = true;
            Pawns["whitePawn5"].Location = CellsPositions[6, 4];
            ChessmanPresenceSign[6, 4] = true;
            Pawns["whitePawn6"].Location = CellsPositions[6, 5];
            ChessmanPresenceSign[6, 5] = true;
            Pawns["whitePawn7"].Location = CellsPositions[6, 6];
            ChessmanPresenceSign[6, 6] = true;
            Pawns["whitePawn8"].Location = CellsPositions[6, 7];
            ChessmanPresenceSign[6, 7] = true;
            Pawns["blackPawn1"].Location = CellsPositions[1, 0];
            ChessmanPresenceSign[1, 0] = true;
            Pawns["blackPawn2"].Location = CellsPositions[1, 1];
            ChessmanPresenceSign[1, 1] = true;
            Pawns["blackPawn3"].Location = CellsPositions[1, 2];
            ChessmanPresenceSign[1, 2] = true;
            Pawns["blackPawn4"].Location = CellsPositions[1, 3];
            ChessmanPresenceSign[1, 3] = true;
            Pawns["blackPawn5"].Location = CellsPositions[1, 4];
            ChessmanPresenceSign[1, 4] = true;
            Pawns["blackPawn6"].Location = CellsPositions[1, 5];
            ChessmanPresenceSign[1, 5] = true;
            Pawns["blackPawn7"].Location = CellsPositions[1, 6];
            ChessmanPresenceSign[1, 6] = true;
            Pawns["blackPawn8"].Location = CellsPositions[1, 7];
            ChessmanPresenceSign[1, 7] = true;
        }

        private void CheckChessMove(object sender)
        {
            var startX = (initialChessmanLocation.X - 27) / 50;
            var startY = (initialChessmanLocation.Y - 27) / 50;
            var finishX = (CellsPositions[chessmansCellIndexRow, chessmansCellIndexColumn].X - 27) / 50;
            var finishY = (CellsPositions[chessmansCellIndexRow, chessmansCellIndexColumn].Y - 27) / 50;

            var impossibleMove = sender is Queen
                ? Queen.CheckQueenMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, moveOrder)
                : sender is Castle
                    ? Castle.CheckCastleMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, moveOrder)
                    : sender is Elephant
                        ? Elephant.CheckElephantMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, moveOrder)
                        : sender is Horse
                            ? Horse.CheckHorseMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, moveOrder)
                            : sender is King
                                ? King.CheckKingMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, sender)
                                : Pawn.CheckPawnMove(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls, sender, moveOrder);

            if (impossibleMove)
            {
                MessageBox.Show(Resources.ImpossibleMoveMessage);

                chess = (Chessman) sender;
                chess.Location = initialChessmanLocation;
            }
            else
            {
                //Проверка возможности превращения
                if (sender is Pawn)
                {
                    if (((Pawn)sender).CanTransform)
                    {
                        var dialogWindow = new PawnTransfomDialog();
                        dialogWindow.Owner = this;

                        DialogResult dialogResult = dialogWindow.ShowDialog();
                        if (dialogResult == DialogResult.OK && !dialogWindow.RBPawn.Checked)
                        {
                            var color = ((Pawn)sender).ChessColor;
                            var finishPoint = new Point(finishX * 50 + 27, finishY * 50 + 27);

                            ((Pawn)sender).Dispose();

                            if (dialogWindow.RBCastle.Checked)
                            {
                                var index = Castles.Count + 1;
                                Castles.Add(color + "Castel" + index, new Castle(color));
                                Controls.Add(Castles[color + "Castel" + index]);
                                Castles[color + "Castel" + index].Location = finishPoint;

                                Castles[color + "Castel" + index].MouseLeave += Chessman_MouseLeave;
                                Castles[color + "Castel" + index].MouseDown += Chessman_MouseDown;
                                Castles[color + "Castel" + index].MouseUp += Chessman_MouseUp;
                                Castles[color + "Castel" + index].MouseMove += Chessman_MouseMove;

                                sender = Castles[color + "Castel" + index];
                                Chessman.CheckEnemyKingBeAttaced(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls,
                                    sender, moveOrder);
                            }

                            if (dialogWindow.RBElephant.Checked)
                            {
                                var index = Elephants.Count + 1;
                                Elephants.Add(color + "Elephant" + index, new Elephant(color));
                                Controls.Add(Elephants[color + "Elephant" + index]);
                                Elephants[color + "Elephant" + index].Location = finishPoint;

                                Elephants[color + "Elephant" + index].MouseLeave += Chessman_MouseLeave;
                                Elephants[color + "Elephant" + index].MouseDown += Chessman_MouseDown;
                                Elephants[color + "Elephant" + index].MouseUp += Chessman_MouseUp;
                                Elephants[color + "Elephant" + index].MouseMove += Chessman_MouseMove;

                                sender = Elephants[color + "Elephant" + index]; 
                                Chessman.CheckEnemyKingBeAttaced(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls,
                                    sender, moveOrder);
                            }

                            if (dialogWindow.RBHorse.Checked)
                            {
                                var index = Horses.Count + 1;
                                Horses.Add(color + "Horse" + index, new Horse(color));
                                Controls.Add(Horses[color + "Horse" + index]);
                                Horses[color + "Horse" + index].Location = finishPoint;

                                Horses[color + "Horse" + index].MouseLeave += Chessman_MouseLeave;
                                Horses[color + "Horse" + index].MouseDown += Chessman_MouseDown;
                                Horses[color + "Horse" + index].MouseUp += Chessman_MouseUp;
                                Horses[color + "Horse" + index].MouseMove += Chessman_MouseMove;

                                sender = Horses[color + "Horse" + index]; 
                                Chessman.CheckEnemyKingBeAttaced(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls,
                                    sender, moveOrder);
                            }

                            if (dialogWindow.RBQueen.Checked)
                            {
                                var index = Queens.Count + 1;
                                Queens.Add(color + "Queen" + index, new Queen(color));
                                Controls.Add(Queens[color + "Queen" + index]);
                                Queens[color + "Queen" + index].Location = finishPoint;

                                Queens[color + "Queen" + index].MouseLeave += Chessman_MouseLeave;
                                Queens[color + "Queen" + index].MouseDown += Chessman_MouseDown;
                                Queens[color + "Queen" + index].MouseUp += Chessman_MouseUp;
                                Queens[color + "Queen" + index].MouseMove += Chessman_MouseMove;

                                sender = Queens[color + "Queen" + index]; 
                                Chessman.CheckEnemyKingBeAttaced(startX, startY, finishX, finishY, ChessmanPresenceSign, Controls,
                                    sender, moveOrder);
                            }
                        }
                    }
                }

                //Проверка мата или шаха
                ChessmanPresenceSign[startY, startX] = false;
                ChessmanPresenceSign[finishY, finishX] = true;

                moveOrder[((Chessman) sender).ChessColor] += -2;

                if (((Chessman)sender).ShahSigne && !((Chessman)sender).MateSigne)
                {
                    MessageBox.Show(Resources.ShahMessage);
                    ((Chessman) sender).ShahSigne = false;
                }

                if (((Chessman) sender).MateSigne)
                {
                    DialogResult result = MessageBox.Show(Resources.MateMessage + Environment.NewLine + Resources.restartQuestion, 
                        Resources.GameEndMessage, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        Application.Restart();
                    }
                    else
                    {
                        Application.Exit();   
                    }
                }
            }
        }

        private void DeleteCellsOrder()
        {
            for (var i = 0; i < CellsCountInRow*2; i++)
            {
                Orders[i].Dispose();
            }
        }
    }
}