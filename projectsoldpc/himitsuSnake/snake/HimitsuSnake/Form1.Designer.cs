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
            this.SuspendLayout();
            // 
            // btnTrains
            // 
            this.btnTrains.Location = new System.Drawing.Point(179, 330);
            this.btnTrains.Name = "btnTrains";
            this.btnTrains.Size = new System.Drawing.Size(75, 23);
            this.btnTrains.TabIndex = 0;
            this.btnTrains.Text = "Treina";
            this.btnTrains.UseVisualStyleBackColor = true;
            this.btnTrains.Click += new System.EventHandler(this.BtnTrains_Click);
            // 
            // btnSimula
            // 
            this.btnSimula.Location = new System.Drawing.Point(470, 330);
            this.btnSimula.Name = "btnSimula";
            this.btnSimula.Size = new System.Drawing.Size(75, 23);
            this.btnSimula.TabIndex = 1;
            this.btnSimula.Text = "Simula";
            this.btnSimula.UseVisualStyleBackColor = true;
            this.btnSimula.Click += new System.EventHandler(this.BtnSimula_Click);
            // 
            // txtGeracao
            // 
            this.txtGeracao.Location = new System.Drawing.Point(179, 274);
            this.txtGeracao.Name = "txtGeracao";
            this.txtGeracao.Size = new System.Drawing.Size(100, 20);
            this.txtGeracao.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 277);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Ultima Geracao";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGeracao);
            this.Controls.Add(this.btnSimula);
            this.Controls.Add(this.btnTrains);
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
    }
}

