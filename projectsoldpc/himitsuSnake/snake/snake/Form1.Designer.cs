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
            this.pnFundo = new System.Windows.Forms.Panel();
            this.pnF = new System.Windows.Forms.Panel();
            this.lblDistancia = new System.Windows.Forms.Label();
            this.lblPontuacao = new System.Windows.Forms.Label();
            this.pnGame = new snake.ExtendedPanel();
            this.lblPontos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // pnFundo
            // 
            this.pnFundo.Location = new System.Drawing.Point(0, 0);
            this.pnFundo.Name = "pnFundo";
            this.pnFundo.Size = new System.Drawing.Size(200, 100);
            this.pnFundo.TabIndex = 0;
            // 
            // pnF
            // 
            this.pnF.BackColor = System.Drawing.Color.White;
            this.pnF.Location = new System.Drawing.Point(0, 0);
            this.pnF.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnF.Name = "pnF";
            this.pnF.Size = new System.Drawing.Size(1035, 782);
            this.pnF.TabIndex = 0;
            this.pnF.Paint += new System.Windows.Forms.PaintEventHandler(this.PnF_Paint);
            // 
            // lblDistancia
            // 
            this.lblDistancia.AutoSize = true;
            this.lblDistancia.Location = new System.Drawing.Point(13, 808);
            this.lblDistancia.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDistancia.Name = "lblDistancia";
            this.lblDistancia.Size = new System.Drawing.Size(152, 17);
            this.lblDistancia.TabIndex = 4;
            this.lblDistancia.Text = "Distância Percorrida: 0";
            // 
            // lblPontuacao
            // 
            this.lblPontuacao.AutoSize = true;
            this.lblPontuacao.Location = new System.Drawing.Point(407, 808);
            this.lblPontuacao.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPontuacao.Name = "lblPontuacao";
            this.lblPontuacao.Size = new System.Drawing.Size(97, 17);
            this.lblPontuacao.TabIndex = 5;
            this.lblPontuacao.Text = "Food Count: 0";
            // 
            // pnGame
            // 
            this.pnGame.BackColor = System.Drawing.Color.Transparent;
            this.pnGame.ForeColor = System.Drawing.Color.Transparent;
            this.pnGame.Location = new System.Drawing.Point(0, 0);
            this.pnGame.Margin = new System.Windows.Forms.Padding(4);
            this.pnGame.Name = "pnGame";
            this.pnGame.Size = new System.Drawing.Size(1035, 782);
            this.pnGame.TabIndex = 0;
            this.pnGame.Paint += new System.Windows.Forms.PaintEventHandler(this.PnGame_Paint);
            // 
            // lblPontos
            // 
            this.lblPontos.AutoSize = true;
            this.lblPontos.Location = new System.Drawing.Point(596, 808);
            this.lblPontos.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPontos.Name = "lblPontos";
            this.lblPontos.Size = new System.Drawing.Size(92, 17);
            this.lblPontos.TabIndex = 6;
            this.lblPontos.Text = "Pontuação: 0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1036, 855);
            this.Controls.Add(this.lblPontos);
            this.Controls.Add(this.lblPontuacao);
            this.Controls.Add(this.lblDistancia);
            this.Controls.Add(this.pnGame);
            this.Controls.Add(this.pnF);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnFundo;
        private System.Windows.Forms.Panel pnF;
        private ExtendedPanel pnGame;
        private System.Windows.Forms.Label lblDistancia;
        private System.Windows.Forms.Label lblPontuacao;
        private System.Windows.Forms.Label lblPontos;
    }
}

