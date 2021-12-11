using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public class NaturalSelection
    {
        int _startPoupulation = 100;
        int _numeroCamadasDeNeuronios = 5;
        int _numeroNeuroniosNaCamada = 4;
        double[] _dadosIniciais;
        Func<string, double> _metodoDeAvaliacao;
        Func<SaidaDeIteracao[], double[]> _metodoReiterador;
        SaidaDeIteracao[] _possiveisSaidas;
        string _filePath = null;

        public NaturalSelection(double[] dadosIniciais, Func<string, double> metodoDeAvaliacao, Func<SaidaDeIteracao[], 
            double[]> metodoReiterador, SaidaDeIteracao[] possiveisSaidas, string filePath)
        {
            _dadosIniciais = dadosIniciais;
            _metodoDeAvaliacao = metodoDeAvaliacao;
            _metodoReiterador = metodoReiterador;
            _possiveisSaidas = possiveisSaidas;
            _filePath = filePath;
        }
        public List<SerVivo> EvolveTillGeneration(int finalGeneration)
        {
            List<SerVivo> populacao = Evolve(null);

            while(populacao[0].Geracao < finalGeneration)
            {
                populacao = Evolve(populacao);
            }
            return populacao;
        }
        public List<SerVivo> Evolve(List<SerVivo> geracaoAtual)
        {
            Simulador simulator = new Simulador(_dadosIniciais, _metodoDeAvaliacao, _metodoReiterador, _possiveisSaidas);

            if(geracaoAtual == null)
            {
                geracaoAtual = CriaGeracaoInicial();
            }
            foreach(SerVivo ser in geracaoAtual)
            {
                ser.Pontuacao = simulator.Simula(ser);
            }

            List<SerVivo> melhores = Darwin(geracaoAtual);

            GravaMelhoresDaGeracao(melhores);

            List<SerVivo> newGeneration = Reproduz(melhores);

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
                        IEnumerable<SerVivo> variacoes = GeraVariacoes(ser, camada.Key, i, j);
                        nextGen.AddRange(variacoes);
                    }
                }
            }
            return nextGen;
        }

        private IEnumerable<SerVivo> GeraVariacoes(SerVivo ser, int key, int i, int j)
        {
            double atual = ser.Pesos[key][i][j];

            List<SerVivo> result = new List<SerVivo>();

            List<double> novosValores = new List<double>();

            novosValores.Add(atual + atual * 0.2);
            novosValores.Add(atual - atual * 0.2);
            novosValores.Add(atual + atual * 0.8);
            novosValores.Add(atual - atual * 0.8);

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

        private List<SerVivo> Darwin(List<SerVivo> geracaoAtual)
        {
            var result = geracaoAtual.OrderBy(x => x.Pontuacao);

            return result.Take(10).ToList();
        }

        private List<SerVivo> CriaGeracaoInicial()
        {
            int count = 0;

            List<SerVivo> geracaoInicial = new List<SerVivo>();
            while (count < _startPoupulation)
            {
                SerVivo novo = CriaSerVivoGeracaoInicial();
                geracaoInicial.Add(novo);
            }
            return geracaoInicial;
        }

        private SerVivo CriaSerVivoGeracaoInicial()
        {
            SerVivo novo = new SerVivo();

            novo.Geracao = 0;
            novo.NomeUnico = "0_" + DateTime.Now.ToString();
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
                        pesosIniciais[i][j] = GeraPesoAleatorio();
                    }
                }
                result.Add(camada, pesosIntemediarios);
            }

            double[][] pesosFinais = new double[_numeroCamadasDeNeuronios][];

            for (int i = 0; i < pesosFinais.Length; i++)
            {
                pesosFinais[i] = new double[_possiveisSaidas.Length];

                for (int j = 0; j < _possiveisSaidas.Length; j++)
                {
                    pesosIniciais[i][j] = GeraPesoAleatorio();
                }
            }
            result.Add(_numeroCamadasDeNeuronios, pesosIniciais);

            return result;
        }

        private double GeraPesoAleatorio()
        {
            Random rdm = new Random();

            return rdm.NextDouble();
        }
    }
}
