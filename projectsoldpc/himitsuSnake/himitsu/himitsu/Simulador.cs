using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace himitsu
{
    public class Simulador
    {
        Func<string, double> _metodoDeAvaliacao;
        Func<SaidaDeIteracao[],double[]> _metodoReiterador;
        double[] _dadosIniciais;
        Action _metodoIniciador;
        SaidaDeIteracao[] _possiveisSaidas;

        public Simulador( double[] dadosIniciais, Func<string, double> metodoAvaliacao, 
            Func<SaidaDeIteracao[], double[]> metodoReiterador, SaidaDeIteracao[] possiveisSaidas, Action metodoIniciador)
        {
            _dadosIniciais = dadosIniciais;
            _metodoDeAvaliacao = metodoAvaliacao;
            _metodoReiterador = metodoReiterador;
            _possiveisSaidas = possiveisSaidas;
            _metodoIniciador = metodoIniciador;
        }

        public double Simula(SerVivo simulado, bool usaMetodoIniciador)
        {
            if(usaMetodoIniciador)
                _metodoIniciador();
            Iterador it = new Iterador(simulado);

            it.DadosIniciais = _dadosIniciais;
            it.PossiveisSaidas = _possiveisSaidas;
            int count = 1;

            SaidaDeIteracao[] saidasDaIteracao = it.Iterate();

            double[] newInput = _metodoReiterador(saidasDaIteracao);
            double[] oldInput = null;
            if (newInput != null)
                oldInput = new double[newInput.Length];
            while (newInput != null)
            {
                if (!TemMesmosValores(oldInput, newInput))
                {
                    oldInput = new double[newInput.Length];

                    newInput.CopyTo(oldInput, 0);

                    it.DadosIniciais = newInput;
                    saidasDaIteracao = it.Iterate();
                    count++;
                }
                else
                {
                    Thread.Sleep(50);
                }
                    newInput = _metodoReiterador(saidasDaIteracao);
            }

            double avaliacao = _metodoDeAvaliacao(simulado.NomeUnico);
            simulado.NumeroIteracoes = count;
            //SerVivo result = new SerVivo();

            //result.Pontuacao = avaliacao;
            //result.Pesos = it.Pesos;


            return avaliacao;
        }

        private bool TemMesmosValores(double[] oldInput, double[] newInput)
        {
            if (oldInput == null || newInput == null) return false;

            for (int i = 0; i < oldInput.Length; i++)
            {
                if (oldInput[i] != newInput[i])
                    return false;
            }

            return true;
        }
    }
}
