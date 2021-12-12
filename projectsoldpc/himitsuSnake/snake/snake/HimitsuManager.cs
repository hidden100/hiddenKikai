using himitsu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snake
{

    public static class HimitsuManager
    {
        static NaturalSelection _ns;
        private static SerVivo _atual;
        static Form1 _form;
        public static double Pontuacao { get; set; }
        public static double MelhorPontuacao { get; set; }

        public static int MelhorGeracao { get; set; }
        public static int Iteracoes { get; set; }

        public static void Start()
        {
            _form = new Form1(false, false);
            double[] dadosIniciais = PegaDadosIniciais();
            Func<string, double> metodoDeAvaliacao = PegaMetodoDeAvaliacao();
            Func<SaidaDeIteracao[], double[]> metodoDeReiteracao = PegaMetodoDeReiteracao();
            SaidaDeIteracao[] possiveisSaidas = PegaPossiveisSaidas();
            Action metodoIniciador = PegaMetodoIniciador();
            double[] faixas = PegaFaixas();


            _ns = new NaturalSelection(dadosIniciais, metodoDeAvaliacao, metodoDeReiteracao, possiveisSaidas, metodoIniciador, null,  faixas);

        }

        private static double[] PegaFaixas()
        {
            List<double> f = new List<double>();
            f.Add(500000000);
            f.Add(10000000000);
            
            return f.OrderBy(x => x).ToArray();
        }

        private static Action PegaMetodoIniciador()
        {
            return () =>
            {
                _form = new Form1(false, true);
            };
        }

        private static Func<SaidaDeIteracao[], double[]> PegaMetodoDeReiteracao()
        {
            return ((x) =>
            {
                SaidaDeIteracao cima = x.Where(y => y.Name == "Cima").First();
                //SaidaDeIteracao baixo = x.Where(y => y.Name == "Baixo").First();
                SaidaDeIteracao direita = x.Where(y => y.Name == "Direita").First();
                SaidaDeIteracao esquerda = x.Where(y => y.Name == "Esquerda").First();

                if (cima.Estado == State.Positive)
                {
                    _form.SetSentido(Sentido.Cima);
                }
                //if (baixo.Estado == State.Positive)
                //{
                //    _form.SetSentido(Sentido.Baixo);
                //}
                if (direita.Estado == State.Positive)
                {
                    _form.SetSentido(Sentido.Direita);
                }
                if (esquerda.Estado == State.Positive)
                {
                    _form.SetSentido(Sentido.Esquerda);
                }
                return _form.GameCycleSincrono();
            });
        }

        private static SaidaDeIteracao[] PegaPossiveisSaidas()
        {
            List<SaidaDeIteracao> saidas = new List<SaidaDeIteracao>();

            SaidaDeIteracao cima = new SaidaDeIteracao();
            cima.Name = "Cima";
            cima.Tipo = TypeOfOutput.BiState;
            saidas.Add(cima);

            //SaidaDeIteracao baixo = new SaidaDeIteracao();
            //baixo.Name = "Baixo";
            //baixo.Tipo = TypeOfOutput.BiState;
            //saidas.Add(baixo);

            SaidaDeIteracao direita = new SaidaDeIteracao();
            direita.Name = "Direita";
            direita.Tipo = TypeOfOutput.BiState;
            saidas.Add(direita);

            SaidaDeIteracao esquerda = new SaidaDeIteracao();
            esquerda.Name = "Esquerda";
            esquerda.Tipo = TypeOfOutput.BiState;
            saidas.Add(esquerda);

            return saidas.ToArray();
        }

        private static Func<string, double> PegaMetodoDeAvaliacao()
        {
            return ((x) =>
            {
                return _form.Pontuacao;
            });

        }

        private static double[] PegaDadosIniciais()
        {
            return _form.GetCurrentInputs();          
        }

        public static void Train(string filePath, int geracao, string prexistente, double? pontuacaoAlvo)
        {
            _ns.FilePath = filePath;
            //_form = new Form1(false, true);
            _ns.EvolveTillGenerationOrPoint(geracao, prexistente, pontuacaoAlvo);
            MelhorGeracao = _ns.MelhorGeracao;
            MelhorPontuacao = _ns.MelhorPontuacao;
        }
        public static void PlayGeneration(int generation)
        {
            List<SerVivo> seres = _ns.PegaGeracaoGravada(generation);

            SerVivo melhor = seres.OrderByDescending(x => x.Pontuacao).First();

            _atual = melhor;
            _form = new Form1(true, true);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(_form);
            StartGenerationPlayCycle();
        }
        public static void PlayGeneration(string path)
        {
            List<SerVivo> seres = _ns.PegaGeracaoGravada(path);

            SerVivo melhor = seres.OrderByDescending(x => x.Pontuacao).First();

            _atual = melhor;
            _form = new Form1(true, true);
            Application.EnableVisualStyles();
            StartGenerationPlayCycle();
            _form.ShowDialog();
        }

        private static void StartGenerationPlayCycle()
        {
            Thread t = new Thread(StartGenerationPlay);
            t.Start();
        }

        private static void StartGenerationPlay()
        {
            while (_form.IsPaused)
            {
                Thread.Sleep(10);
            }
            Pontuacao = _ns.Simulador.Simula(_atual, false);
            Iteracoes = _atual.NumeroIteracoes;
        }
    }
}
