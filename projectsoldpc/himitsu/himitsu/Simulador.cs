using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public class Simulador
    {
        bool _simulando;
        Func<string, double> _metodoDeAvaliacao;
        Func<SaidaDeIteracao[],double[]> _metodoReiterador;
        double[] _dadosIniciais;
        SaidaDeIteracao[] _possiveisSaidas;

        public Simulador( double[] dadosIniciais, Func<string, double> metodoAvaliacao, 
            Func<SaidaDeIteracao[], double[]> metodoReiterador, SaidaDeIteracao[] possiveisSaidas)
        {
            _dadosIniciais = dadosIniciais;
            _metodoDeAvaliacao = metodoAvaliacao;
            _metodoReiterador = metodoReiterador;
            _possiveisSaidas = possiveisSaidas;
        }

        public double Simula(SerVivo simulado)
        {

            Iterador it = new Iterador(simulado);

            it.DadosIniciais = _dadosIniciais;
            it.PossiveisSaidas = _possiveisSaidas;


            SaidaDeIteracao[] saidasDaIteracao = it.Iterate();

            double[] newInput = _metodoReiterador(saidasDaIteracao);

            while (newInput != null)
            {
                it.DadosIniciais = newInput;
                saidasDaIteracao = it.Iterate();
                newInput = _metodoReiterador(saidasDaIteracao);
            }

            double avaliacao = _metodoDeAvaliacao(simulado.NomeUnico);

            //SerVivo result = new SerVivo();

            //result.Pontuacao = avaliacao;
            //result.Pesos = it.Pesos;


            return avaliacao;
        }
    }
}
