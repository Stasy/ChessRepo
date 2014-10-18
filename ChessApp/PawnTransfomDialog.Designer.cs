namespace Chess
{
    partial class PawnTransfomDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PawnTransfomDialog));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.RBPawn = new System.Windows.Forms.RadioButton();
            this.RBQueen = new System.Windows.Forms.RadioButton();
            this.RBCastle = new System.Windows.Forms.RadioButton();
            this.RBElephant = new System.Windows.Forms.RadioButton();
            this.RBHorse = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.RBHorse);
            this.groupBox1.Controls.Add(this.RBElephant);
            this.groupBox1.Controls.Add(this.RBCastle);
            this.groupBox1.Controls.Add(this.RBQueen);
            this.groupBox1.Controls.Add(this.RBPawn);
            this.groupBox1.Location = new System.Drawing.Point(12, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(180, 155);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Вырианты превращения";
            // 
            // RBPawn
            // 
            this.RBPawn.AutoSize = true;
            this.RBPawn.Location = new System.Drawing.Point(21, 28);
            this.RBPawn.Name = "RBPawn";
            this.RBPawn.Size = new System.Drawing.Size(59, 17);
            this.RBPawn.TabIndex = 0;
            this.RBPawn.TabStop = true;
            this.RBPawn.Text = "Пешка";
            this.RBPawn.UseVisualStyleBackColor = true;
            this.RBPawn.CheckedChanged += new System.EventHandler(this.PawnTransfomDialog_Load);
            // 
            // RBQueen
            // 
            this.RBQueen.AutoSize = true;
            this.RBQueen.Location = new System.Drawing.Point(21, 51);
            this.RBQueen.Name = "RBQueen";
            this.RBQueen.Size = new System.Drawing.Size(60, 17);
            this.RBQueen.TabIndex = 1;
            this.RBQueen.TabStop = true;
            this.RBQueen.Text = "Ферзь";
            this.RBQueen.UseVisualStyleBackColor = true;
            // 
            // RBCastle
            // 
            this.RBCastle.AutoSize = true;
            this.RBCastle.Location = new System.Drawing.Point(21, 74);
            this.RBCastle.Name = "RBCastle";
            this.RBCastle.Size = new System.Drawing.Size(57, 17);
            this.RBCastle.TabIndex = 2;
            this.RBCastle.TabStop = true;
            this.RBCastle.Text = "Ладья";
            this.RBCastle.UseVisualStyleBackColor = true;
            // 
            // RBElephant
            // 
            this.RBElephant.AutoSize = true;
            this.RBElephant.Location = new System.Drawing.Point(21, 97);
            this.RBElephant.Name = "RBElephant";
            this.RBElephant.Size = new System.Drawing.Size(50, 17);
            this.RBElephant.TabIndex = 3;
            this.RBElephant.TabStop = true;
            this.RBElephant.Text = "Слон";
            this.RBElephant.UseVisualStyleBackColor = true;
            // 
            // RBHorse
            // 
            this.RBHorse.AutoSize = true;
            this.RBHorse.Location = new System.Drawing.Point(21, 120);
            this.RBHorse.Name = "RBHorse";
            this.RBHorse.Size = new System.Drawing.Size(50, 17);
            this.RBHorse.TabIndex = 4;
            this.RBHorse.TabStop = true;
            this.RBHorse.Text = "Конь";
            this.RBHorse.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(30, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Выберите фигуру";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(58, 227);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // PawnTransfomDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(204, 262);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PawnTransfomDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Превращение";
            this.Load += new System.EventHandler(this.PawnTransfomDialog_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.RadioButton RBHorse;
        public System.Windows.Forms.RadioButton RBElephant;
        public System.Windows.Forms.RadioButton RBCastle;
        public System.Windows.Forms.RadioButton RBQueen;
        public System.Windows.Forms.RadioButton RBPawn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}