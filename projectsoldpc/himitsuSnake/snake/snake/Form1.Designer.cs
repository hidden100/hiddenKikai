namespace snake
{
    partial class Form1
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnGame = new snake.ExtendedPanel();
            this.pnFundo = new System.Windows.Forms.Panel();
            this.pnF = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnGame
            // 
            this.pnGame.BackColor = System.Drawing.Color.Transparent;
            this.pnGame.ForeColor = System.Drawing.Color.Transparent;
            this.pnGame.Location = new System.Drawing.Point(0, 0);
            this.pnGame.Name = "pnGame";
            this.pnGame.Size = new System.Drawing.Size(776, 635);
            this.pnGame.TabIndex = 0;
            this.pnGame.Paint += new System.Windows.Forms.PaintEventHandler(this.PnGame_Paint);
            // 
            // pnF
            // 
            this.pnF.BackColor = System.Drawing.Color.White;
            this.pnF.Location = new System.Drawing.Point(0, 0);
            this.pnF.Name = "pnF";
            this.pnF.Size = new System.Drawing.Size(776, 635);
            this.pnF.TabIndex = 0;
            this.pnF.Paint += new System.Windows.Forms.PaintEventHandler(this.PnF_Paint);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 659);
            this.Controls.Add(this.pnGame);
            this.Controls.Add(this.pnF);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel pnFundo;
        private System.Windows.Forms.Panel pnF;
        private ExtendedPanel pnGame;
    }
}

