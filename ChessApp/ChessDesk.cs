using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Chess.Chessmans;
using Chess.Properties;

namespace Chess
{
    public partial class ChessDesk : Form
    {
        const int CellsCountInRow = 8;
        public Panel[,] Cells = new Panel[CellsCountInRow, CellsCountInRow];
        public Label[] Orders = new Label[CellsCountInRow * 2];

        public Dictionary<string, Castle> Castles = new Dictionary<string, Castle>();
        public Dictionary<string, Elephant> Elephants = new Dictionary<string, Elephant>(); 
        public Dictionary<string, Horse> Horses = new Dictionary<string, Horse>(); 
        public Dictionary<string, King> Kings = new Dictionary<string, King>(); 
        public Dictionary<string, Pawn> Pawns = new Dictionary<string, Pawn>();
        public Dictionary<string, Queen> Queens = new Dictionary<string, Queen>(); 

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
            
            //Create Cells and Orders
            CreateCells();
            AssigneCellsOrder();

            //Create Chessmans
            string[] chessmansColors = {"black", "white"};
            foreach (var color in chessmansColors)
            {
                Kings.Add(color + "King", new King(color));
                Queens.Add(color + "Queen", new Queen(color));
                
                for (var chessmanIndex = 1; chessmanIndex <= 2; chessmanIndex++)
                {
                    Castles.Add(color + "Castle" + chessmanIndex, new Castle(color));
                    Elephants.Add(color + "Elephant" + chessmanIndex, new Elephant(color));
                    Horses.Add(color + "Horse" + chessmanIndex, new Horse(color));
                }

                for (var pawnIndex = 1; pawnIndex <= 8; pawnIndex++)
                    Pawns.Add(color + "Pawn" + pawnIndex, new Pawn(color));
            }

            //Chessmans initial disposition
            Castles["whiteCastle1"].Parent = Cells[7, 0];
            Castles["whiteCastle2"].Parent = Cells[7, 7];
            Castles["blackCastle1"].Parent = Cells[0, 0];
            Castles["blackCastle2"].Parent = Cells[0, 7];

            Horses["whiteHorse1"].Parent = Cells[7, 1];
            Horses["whiteHorse2"].Parent = Cells[7, 6];
            Horses["blackHorse1"].Parent = Cells[0, 1];
            Horses["blackHorse2"].Parent = Cells[0, 6];

            Elephants["whiteElephant1"].Parent = Cells[7, 2];
            Elephants["whiteElephant2"].Parent = Cells[7, 5];
            Elephants["blackElephant1"].Parent = Cells[0, 2];
            Elephants["blackElephant2"].Parent = Cells[0, 5];

            Queens["whiteQueen"].Parent = Cells[7, 3];
            Queens["blackQueen"].Parent = Cells[0, 4];

            Kings["whiteKing"].Parent = Cells[7, 4];
            Kings["blackKing"].Parent = Cells[0, 3];

            Pawns["whitePawn1"].Parent = Cells[6, 0];
            Pawns["whitePawn2"].Parent = Cells[6, 1];
            Pawns["whitePawn3"].Parent = Cells[6, 2];
            Pawns["whitePawn4"].Parent = Cells[6, 3];
            Pawns["whitePawn5"].Parent = Cells[6, 4];
            Pawns["whitePawn6"].Parent = Cells[6, 5];
            Pawns["whitePawn7"].Parent = Cells[6, 6];
            Pawns["whitePawn8"].Parent = Cells[6, 7];
            Pawns["blackPawn1"].Parent = Cells[1, 0];
            Pawns["blackPawn2"].Parent = Cells[1, 1];
            Pawns["blackPawn3"].Parent = Cells[1, 2];
            Pawns["blackPawn4"].Parent = Cells[1, 3];
            Pawns["blackPawn5"].Parent = Cells[1, 4];
            Pawns["blackPawn6"].Parent = Cells[1, 5];
            Pawns["blackPawn7"].Parent = Cells[1, 6];
            Pawns["blackPawn8"].Parent = Cells[1, 7];

            ResumeLayout(true);
        }

        private void ChessDesk_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Set ChessDesk properties to default
            ClientSize = new Size(284, 262);
            StartPosition = FormStartPosition.WindowsDefaultLocation;
            BackgroundImage = null;
            FormBorderStyle = FormBorderStyle.Sizable;

            //Delete Cells and Orders
            DeleteCells();
            DeleteCellsOrder();
        }

        private void CreateCells()
        {
            for (var i = 0; i < CellsCountInRow; i++)
            {
                for (var j = 0; j < CellsCountInRow; j++)
                {
                    var startPositionX = j * 50 + 25;
                    var startPositionY = i * 50 + 25;

                    Cells[i, j] = new Panel
                    {
                        Size = new Size(50, 50),
                        Location = new Point(startPositionX, startPositionY),
                        BorderStyle = new BorderStyle()
                    };

                    Controls.Add(Cells[i, j]);

                    Cells[i, j].BackgroundImage = ((i + j) % 2) == 0 ?
                        Resources.grayCell : Resources.blackCell;
                }
            }
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

        private void DeleteCells()
        {
            for (var i = 0; i < CellsCountInRow; i++)
            {
                for (var j = 0; j < CellsCountInRow; j++)
                {
                    Cells[i, j].Dispose();
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
