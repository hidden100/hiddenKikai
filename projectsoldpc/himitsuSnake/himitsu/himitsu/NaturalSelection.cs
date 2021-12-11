using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace himitsu
{
    public class NaturalSelection
    {
        int _startPoupulation = 100;
        int _numeroCamadasDeNeuronios = 1;
        int _numeroNeuroniosNaCamada = 30;
        double[] _dadosIniciais;
        Func<string, double> _metodoDeAvaliacao;
        Func<SaidaDeIteracao[], double[]> _metodoReiterador;
        SaidaDeIteracao[] _possiveisSaidas;
        string _filePath = null;
        Simulador _simulador;
        private Random _rdm;
        double _melhorPontuacao = 0;
        int _melhorGeracao = 0;
        private double _pontuacaoMinima = 90;
        private double _pontuacaoMaxima = 27000;
        private double _fatorMaximo = 0.8;
        private double _baseDiminuicaoDeFator1 = 5;
        private double _baseDiminuicaoDeFator2 = 2;
        private double[] _FaixasDePontuacao;

        public double MelhorPontuacao
        {
            get { return _melhorPontuacao; }
        }
        public int MelhorGeracao
        {
            get { return _melhorGeracao; }
        }
        public Simulador Simulador
        {
            get { return _simulador; }
        }

        public string FilePath
        { get { return _filePath; } set{ _filePath = value; } }

        public NaturalSelection(double[] dadosIniciais, Func<string, double> metodoDeAvaliacao, Func<SaidaDeIteracao[], 
            double[]> metodoReiterador, SaidaDeIteracao[] possiveisSaidas, Action metodoIniciador, string filePath, double[] faixas)
        {
            _dadosIniciais = dadosIniciais;
            _metodoDeAvaliacao = metodoDeAvaliacao;
            _metodoReiterador = metodoReiterador;
            _possiveisSaidas = possiveisSaidas;
            _filePath = filePath;
            _FaixasDePontuacao = faixas;
            _simulador = new Simulador(_dadosIniciais, _metodoDeAvaliacao, _metodoReiterador, _possiveisSaidas, metodoIniciador);
        }
        public List<SerVivo> EvolveTillGeneration(int finalGeneration, string prexistente)
        {
            double pontuacao = 0;
            int geracaoAtual = 0;

            List<SerVivo> seres = PegaGeracaoGravada(prexistente);

            SerVivo melhor = seres == null ? null : seres.OrderByDescending(x => x.Pontuacao).FirstOrDefault();
            List<SerVivo> populacao;
            if (melhor != null)
            {
                _melhorPontuacao = melhor.Pontuacao;
                pontuacao = _melhorPontuacao;
                populacao = Reproduz(new List<SerVivo>() { melhor });
                populacao.Add(melhor);
            }
            else
            {
                populacao = Evolve(null, ref pontuacao, ref geracaoAtual);
            }
            while(geracaoAtual < finalGeneration)
            {
                populacao = Evolve(populacao, ref pontuacao, ref geracaoAtual);
            }
            return populacao;
        }
        public List<SerVivo> Evolve(List<SerVivo> geracaoAtual, ref double pontuacaoUltimaGeracao, ref int numeroGeracaoAtual)
        {
            
            if(geracaoAtual == null || pontuacaoUltimaGeracao < _pontuacaoMinima)
            {
                geracaoAtual = CriaGeracaoInicial(numeroGeracaoAtual);
            }
            for(int i = 0; i < geracaoAtual.Count; i++)
            {
                //if (ser.Pontuacao == 0)
                //{
                    geracaoAtual[i].Pontuacao = _simulador.Simula(geracaoAtual[i], true);
                //}
            }

            List<SerVivo> melhores = Darwin(geracaoAtual);

            GravaMelhoresDaGeracao(melhores);

            SerVivo melhor = melhores.OrderByDescending(x => x.Pontuacao).First();

            double maiorPontuacaoNova = melhor.Pontuacao;
            int melhorGeracao = melhor.Geracao;
            numeroGeracaoAtual = melhorGeracao + 1;
            if(maiorPontuacaoNova <= pontuacaoUltimaGeracao && maiorPontuacaoNova == 0)
            {
                return null;
            }
            if (maiorPontuacaoNova > pontuacaoUltimaGeracao)
            {
                _melhorGeracao = melhorGeracao;
            }
            _melhorPontuacao = maiorPontuacaoNova;
            pontuacaoUltimaGeracao = _melhorPontuacao;
            List<SerVivo> newGeneration = Reproduz(melhores);
            newGeneration.Add(melhor);
            return newGeneration;
        }

        private void GravaMelhoresDaGeracao(List<SerVivo> melhores)
        {
            FileHelper.GravaGeracao(melhores, _filePath);
        }
        public List<SerVivo> PegaGeracaoGravada(int geracao)
        {
            return FileHelper.PegaGeracao(geracao, _filePath);
        }
        public List<SerVivo> PegaGeracaoGravada(string path)
        {
            return FileHelper.PegaGeracao(path);
        }
        private List<SerVivo> Reproduz(List<SerVivo> melhores)
        {
            List<SerVivo> nextGeneration = new List<SerVivo>();

            foreach(SerVivo ser in melhores)
            {
                nextGeneration.AddRange(Reproduz(ser));
                ser.Geracao = ser.Geracao + 1;

                nextGeneration.Add(ser);
            }
            return nextGeneration;
        }

        private IEnumerable<SerVivo> Reproduz(SerVivo ser)
        {
            List<SerVivo> nextGen = new List<SerVivo>();
            foreach(KeyValuePair<int, double[][]> camada in ser.Pesos)
            {
                for(int i = 0; i < camada.Value.Length; i++)
                {
                    for(int j = 0; j < camada.Value[i].Length; j++)
                    {
                        IEnumerable<SerVivo> variacoes = GeraVariacoes(ser, camada.Key, i, j, _FaixasDePontuacao, ser.Pontuacao);
                        nextGen.AddRange(variacoes);
                    }
                }
            }
            return nextGen;
        }

        private IEnumerable<SerVivo> GeraVariacoes(SerVivo ser, int key, int i, int j, double[] faixaDePontuacoesOrdenada, double pontuacaoAtual)
        {
            double atual = ser.Pesos[key][i][j];
            if(faixaDePontuacoesOrdenada == null || faixaDePontuacoesOrdenada.Length == 0)
            {
                throw new Exception("A faixa de pontuações deve ser fornecida");
            }
            int faixaEmqueSeEncontra = ProcuraFaixa(faixaDePontuacoesOrdenada, pontuacaoAtual);


            double fator = DefineFator(faixaEmqueSeEncontra, faixaDePontuacoesOrdenada.Length);

            List<SerVivo> result = new List<SerVivo>();

            List<double> novosValores = new List<double>();

            novosValores.Add(atual + fator);
            novosValores.Add(atual - fator);

            foreach(double novoValor in novosValores)
            {
                SerVivo novoSer = ser.Clone();

                novoSer.Geracao = novoSer.Geracao + 1;
                novoSer.NomeUnico = ser.Geracao + "_" + DateTime.Now.ToString();
                novoSer.Pesos[key][i][j] = novoValor;
                result.Add(novoSer);
            }

            return result;
        }

        private double DefineFator(int faixaEmqueSeEncontra, int length)
        {
            double prefator = (((double)faixaEmqueSeEncontra )/ length)*_baseDiminuicaoDeFator1;
            double fator = _fatorMaximo / (Math.Pow(_baseDiminuicaoDeFator2, prefator));

            return fator;
        }

        private int ProcuraFaixa(double[] faixaDePontuacoesOrdenada, double pontuacaoAtual)
        {
            for (int i = 0; i < faixaDePontuacoesOrdenada.Length; i++)
            {
                if(pontuacaoAtual < faixaDePontuacoesOrdenada[i])
                {
                    return i;
                }
            }
            return faixaDePontuacoesOrdenada.Length;
        }

        private List<SerVivo> Darwin(List<SerVivo> geracaoAtual)
        {
            var result = geracaoAtual.OrderByDescending(x => x.Pontuacao);

            return result.Take(1).ToList();
        }

        private List<SerVivo> CriaGeracaoInicial(int numeroGeracaoAtual)
        {
            int count = 0;

            List<SerVivo> geracaoInicial = new List<SerVivo>();
            while (count < _startPoupulation)
            {
                SerVivo novo = CriaSerVivoGeracaoInicial(numeroGeracaoAtual);
                geracaoInicial.Add(novo);
                count++;
            }
            return geracaoInicial;
        }

        private SerVivo CriaSerVivoGeracaoInicial(int numeroGeracaoAtual)
        {
            SerVivo novo = new SerVivo();

            novo.Geracao = numeroGeracaoAtual;
            novo.NomeUnico = numeroGeracaoAtual + "_" + DateTime.Now.ToString();
            novo.Pesos = GeraPesosAleatorios();

            return novo;
        }

        private Dictionary<int, double[][]> GeraPesosAleatorios()
        {
            Dictionary<int, double[][]> result = new Dictionary<int, double[][]>();
            int numeroCamadas = _numeroCamadasDeNeuronios + 1;

            double[][] pesosIniciais = new double[_dadosIniciais.Length][];

            for( int i = 0; i < pesosIniciais.Length; i++)
            {
                pesosIniciais[i] = new double[_numeroNeuroniosNaCamada];

                for(int j = 0; j < _numeroNeuroniosNaCamada; j++)
                {
                    pesosIniciais[i][j] = GeraPesoAleatorio();
                }
            }
            result.Add(0, pesosIniciais);

            for (int camada = 1; camada < _numeroCamadasDeNeuronios; camada++)
            {
                double[][] pesosIntemediarios = new double[_numeroNeuroniosNaCamada][];

                for (int i = 0; i < _numeroNeuroniosNaCamada; i++)
                {

                    pesosIntemediarios[i] = new double[_numeroNeuroniosNaCamada];

                    for (int j = 0; j < _numeroNeuroniosNaCamada; j++)
                    {
                        pesosIntemediarios[i][j] = GeraPesoAleatorio();
                    }
                }
                result.Add(camada, pesosIntemediarios);
            }

            double[][] pesosFinais = new double[_numeroNeuroniosNaCamada][];

            for (int i = 0; i < pesosFinais.Length; i++)
            {
                pesosFinais[i] = new double[_possiveisSaidas.Length];

                for (int j = 0; j < _possiveisSaidas.Length; j++)
                {
                    pesosFinais[i][j] = GeraPesoAleatorio();
                }
            }
            result.Add(_numeroCamadasDeNeuronios, pesosFinais);

            return result;
        }

        private double GeraPesoAleatorio()
        {
            if (_rdm == null)
            {
                _rdm = new Random();
            }

            double bla = _rdm.NextDouble() - 0.5;
            return bla;
        }
    }
}
