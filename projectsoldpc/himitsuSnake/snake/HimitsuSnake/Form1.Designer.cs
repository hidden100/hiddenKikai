namespace HimitsuSnake
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
            this.btnTrains = new System.Windows.Forms.Button();
            this.btnSimula = new System.Windows.Forms.Button();
            this.txtGeracao = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPontuacao = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTrains
            // 
            this.btnTrains.Location = new System.Drawing.Point(239, 406);
            this.btnTrains.Margin = new System.Windows.Forms.Padding(4);
            this.btnTrains.Name = "btnTrains";
            this.btnTrains.Size = new System.Drawing.Size(100, 28);
            this.btnTrains.TabIndex = 0;
            this.btnTrains.Text = "Treina";
            this.btnTrains.UseVisualStyleBackColor = true;
            this.btnTrains.Click += new System.EventHandler(this.BtnTrains_Click);
            // 
            // btnSimula
            // 
            this.btnSimula.Location = new System.Drawing.Point(627, 406);
            this.btnSimula.Margin = new System.Windows.Forms.Padding(4);
            this.btnSimula.Name = "btnSimula";
            this.btnSimula.Size = new System.Drawing.Size(100, 28);
            this.btnSimula.TabIndex = 1;
            this.btnSimula.Text = "Simula";
            this.btnSimula.UseVisualStyleBackColor = true;
            this.btnSimula.Click += new System.EventHandler(this.BtnSimula_Click);
            // 
            // txtGeracao
            // 
            this.txtGeracao.Location = new System.Drawing.Point(239, 337);
            this.txtGeracao.Margin = new System.Windows.Forms.Padding(4);
            this.txtGeracao.Name = "txtGeracao";
            this.txtGeracao.Size = new System.Drawing.Size(132, 22);
            this.txtGeracao.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 339);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ultima Geracao";
            // 
            // txtPontuacao
            // 
            this.txtPontuacao.Location = new System.Drawing.Point(239, 367);
            this.txtPontuacao.Margin = new System.Windows.Forms.Padding(4);
            this.txtPontuacao.Name = "txtPontuacao";
            this.txtPontuacao.Size = new System.Drawing.Size(132, 22);
            this.txtPontuacao.TabIndex = 4;
            this.txtPontuacao.Text = "1000000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 370);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Pontuação alvo";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPontuacao);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGeracao);
            this.Controls.Add(this.btnSimula);
            this.Controls.Add(this.btnTrains);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrains;
        private System.Windows.Forms.Button btnSimula;
        private System.Windows.Forms.TextBox txtGeracao;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPontuacao;
        private System.Windows.Forms.Label label2;
    }
}

