namespace Chess
{
    partial class ChessDesk
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ChessDesk
            // 
            this.ClientSize = new System.Drawing.Size(449, 452);
            this.Name = "ChessDesk";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChessDesk";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChessDesk_FormClosing);
            this.Load += new System.EventHandler(this.ChessDesk_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.ChessDesk_Paint);
            this.ResumeLayout(false);

        }

        #endregion


    }
}

