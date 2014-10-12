using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Chess.Chessmans;
using Chess.Properties;

namespace Chess
{
    public partial class ChessDesk : Form
    {
        const int CellsCountInRow = 8;
        public Point[,] cellsPositions = new Point[CellsCountInRow, CellsCountInRow];
        public Label[] Orders = new Label[CellsCountInRow * 2];

        public Dictionary<string, Castle> Castles = new Dictionary<string, Castle>();
        public Dictionary<string, Elephant> Elephants = new Dictionary<string, Elephant>(); 
        public Dictionary<string, Horse> Horses = new Dictionary<string, Horse>(); 
        public Dictionary<string, King> Kings = new Dictionary<string, King>(); 
        public Dictionary<string, Pawn> Pawns = new Dictionary<string, Pawn>();
        public Dictionary<string, Queen> Queens = new Dictionary<string, Queen>();

        private bool ChessmanCanMove = false;
        private Point InitialChessmanLocation;
        private Chessman chess;

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
                    cellsPositions[i, j].X = j*50 + 27;
                    cellsPositions[i, j].Y = i*50 + 27;
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

            //Подключаей обработку событий для объекта "шахмата" 
            foreach (var control in Controls)
            {
                if (control is Chessman)
                {
                    ((Chessman)control).MouseLeave += Chessman_MouseLeave;
                    ((Chessman) control).MouseDown += Chessman_MouseDown;
                    ((Chessman) control).MouseUp += Chessman_MouseUp;
                    ((Chessman) control).MouseMove += Chessman_MouseMove;
                }
            }

            //Chessmans initial disposition
            Castles["whiteCastle1"].Location= cellsPositions[7, 0];
            Castles["whiteCastle2"].Location = cellsPositions[7, 7];
            Castles["blackCastle1"].Location = cellsPositions[0, 0];
            Castles["blackCastle2"].Location = cellsPositions[0, 7];

            Horses["whiteHorse1"].Location = cellsPositions[7, 1];
            Horses["whiteHorse2"].Location = cellsPositions[7, 6];
            Horses["blackHorse1"].Location = cellsPositions[0, 1];
            Horses["blackHorse2"].Location = cellsPositions[0, 6];

            Elephants["whiteElephant1"].Location = cellsPositions[7, 2];
            Elephants["whiteElephant2"].Location = cellsPositions[7, 5];
            Elephants["blackElephant1"].Location = cellsPositions[0, 2];
            Elephants["blackElephant2"].Location = cellsPositions[0, 5];

            Queens["whiteQueen"].Location = cellsPositions[7, 3];
            Queens["blackQueen"].Location = cellsPositions[0, 4];

            Kings["whiteKing"].Location = cellsPositions[7, 4];
            Kings["blackKing"].Location = cellsPositions[0, 3];

            Pawns["whitePawn1"].Location = cellsPositions[6, 0];
            Pawns["whitePawn2"].Location = cellsPositions[6, 1];
            Pawns["whitePawn3"].Location = cellsPositions[6, 2];
            Pawns["whitePawn4"].Location = cellsPositions[6, 3];
            Pawns["whitePawn5"].Location = cellsPositions[6, 4];
            Pawns["whitePawn6"].Location = cellsPositions[6, 5];
            Pawns["whitePawn7"].Location = cellsPositions[6, 6];
            Pawns["whitePawn8"].Location = cellsPositions[6, 7];
            Pawns["blackPawn1"].Location = cellsPositions[1, 0];
            Pawns["blackPawn2"].Location = cellsPositions[1, 1];
            Pawns["blackPawn3"].Location = cellsPositions[1, 2];
            Pawns["blackPawn4"].Location = cellsPositions[1, 3];
            Pawns["blackPawn5"].Location = cellsPositions[1, 4];
            Pawns["blackPawn6"].Location = cellsPositions[1, 5];
            Pawns["blackPawn7"].Location = cellsPositions[1, 6];
            Pawns["blackPawn8"].Location = cellsPositions[1, 7];

            ResumeLayout(true);
        }

        private void Chessman_MouseLeave(object sender, EventArgs e)
        {
            ChessmanCanMove = false;
        }

        private void Chessman_MouseDown(object sender, MouseEventArgs e)
        {
            ChessmanCanMove = true;
            InitialChessmanLocation = e.Location;
        }

        private void Chessman_MouseUp(object sender, MouseEventArgs e)
        {
            ChessmanCanMove = false;
            chess = (Chessman) sender;

            //Выравнивание положения шахматы в ячейке
            int chessmansCellIndexColumn = 0, chessmansCellIndexRow = 0;
            for (var j = 0; j < CellsCountInRow; j++)
            {
                if (chess.Location.X >= cellsPositions[0, j].X - 25 && chess.Location.X < cellsPositions[0, j].X + 25)
                {
                    chessmansCellIndexColumn = j;
                }
            }
            for (var i = 0; i < CellsCountInRow; i++)
            {
                if (chess.Location.Y >= cellsPositions[i, 0].Y - 25 && chess.Location.Y < cellsPositions[i, 0].Y + 25)
                {
                    chessmansCellIndexRow = i;
                }
            }
            chess.Location = cellsPositions[chessmansCellIndexRow, chessmansCellIndexColumn];

            //Метод проверки хода
        }

        private void Chessman_MouseMove(object sender, MouseEventArgs e)
        {
            if (ChessmanCanMove)
            {
                chess = (Chessman) sender;
                chess.Top += e.Y - InitialChessmanLocation.Y;
                chess.Left += e.X - InitialChessmanLocation.X;
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
                    var brush = ((i + j) % 2) == 0 ? whiteBrush : blackBrush;
                    e.Graphics.FillRectangle(brush, cellsPositions[i, j].X - 2, cellsPositions[i, j].Y - 2, 50, 50);
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

            for (var i = 0; i < CellsCountInRow * 2; i++)
            {
                int orderCode;
                int positionX, positionY;

                if (i < 8)
                {
                    positionX = 7;
                    positionY = i * 50 + 42;
                    orderCode = firstNumberCode - i;
                }
                else
                {
                    positionX = (i - 7) * 50 - 5;
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

        private void DeleteCellsOrder()
        {
            for (var i = 0; i < CellsCountInRow*2; i++)
            {
                Orders[i].Dispose();
            }
        }
    }
}
