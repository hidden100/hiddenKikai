using snake;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HimitsuSnake
{
    public partial class Form1 : Form
    {
    string _standardPath = @"C:\Users\hiddenman\Desktop\IA";
        private string _lastFileName = null;

        public Form1()
        {
            InitializeComponent();
            txtGeracao.Text = "5";
            HimitsuManager.Start();
        }

        private void BtnTrains_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = _standardPath;
                DialogResult result = dialog.ShowDialog();
                if (result == DialogResult.OK)
                {
                    _standardPath = dialog.SelectedPath;

                        string prexistente = null;
                    using (OpenFileDialog dialogPreexistente = new OpenFileDialog())
                    {
                        DialogResult res = dialogPreexistente.ShowDialog();

                        //if (string.IsNullOrEmpty(_lastFileName))
                        //{

                        //    _lastFileName = dialog.FileName;
                        //}
                        if (res == DialogResult.OK)
                        {
                            prexistente = dialogPreexistente.FileName;

                        }
                    }
                    HimitsuManager.Train(_standardPath, int.Parse(txtGeracao.Text), prexistente);
                    MessageBox.Show("Treinamento Finalizado - Melhor Geração: " + HimitsuManager.MelhorGeracao + "\nMelhor Pontuação: " + HimitsuManager.MelhorPontuacao);
                }
            }
        }

        private void BtnSimula_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                DialogResult res = dialog.ShowDialog();

                if (string.IsNullOrEmpty(_lastFileName))
                {

                    _lastFileName = dialog.FileName;
                }
                if(res == DialogResult.OK)
                {
                    HimitsuManager.PlayGeneration(dialog.FileName);
                    MessageBox.Show("iteracao " + HimitsuManager.Iteracoes + " Pontuacao: " + HimitsuManager.Pontuacao);

                }
            }
        }
    }
}
