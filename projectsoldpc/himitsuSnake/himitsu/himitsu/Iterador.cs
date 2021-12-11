using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public class Iterador
    {
        public double[] DadosIniciais;
        public SaidaDeIteracao[] PossiveisSaidas;
        SerVivo _Simulado;
        private double _middle = 0;
        private double _neutralRange = 50;


        public Iterador(SerVivo simulado)
        {
            _Simulado = simulado;
        }

        public SaidaDeIteracao[] Iterate()
        {
            double[] camadaAnterior = new double[DadosIniciais.Length];
            DadosIniciais.CopyTo(camadaAnterior, 0);

            for (int camada = 0; camada < _Simulado.Pesos.Count; camada++)
            {
                double[] novaCamada = new double[_Simulado.Pesos[camada][0].Length];

                for (int i = 0; i < novaCamada.Length; i++)
                {
                    novaCamada[i] = EncontraElementoDaNovaCamada(i, _Simulado.Pesos[camada], camadaAnterior);
                    if(camada < _Simulado.Pesos.Count - 1)
                    {
                        novaCamada[i] = Get10Fromdouble(novaCamada[i]);
                    }
                }

                camadaAnterior = new double[novaCamada.Length];
                novaCamada.CopyTo(camadaAnterior, 0);
            }
            SaidaDeIteracao[] result = TransformaUltimaCamadaEmSaida(camadaAnterior);

            return result;
        }

        private SaidaDeIteracao[] TransformaUltimaCamadaEmSaida(double[] camadaAnterior)
        {
            if (PossiveisSaidas.Length != camadaAnterior.Length)
            {
                throw new InvalidOperationException("O tamanho do array de acoes possiveis nao bate com o numero de elementos na ultima camada da iteração");
            }
            int saidaEscolhida = 0;
            double maior = double.MinValue;
            for (int i = 0; i < PossiveisSaidas.Length; i++)
            {
                if (camadaAnterior[i] > maior)
                {
                    maior = camadaAnterior[i];
                    saidaEscolhida = i;
                }
            }

            SaidaDeIteracao[] result = new SaidaDeIteracao[PossiveisSaidas.Length];
            for (int i = 0; i < PossiveisSaidas.Length; i++)
            {
                SaidaDeIteracao atual = new SaidaDeIteracao();

                atual.Name = PossiveisSaidas[i].Name;
                atual.Tipo = PossiveisSaidas[i].Tipo;
                atual.Estado = GerEstadoDeSaida(i == saidaEscolhida);

                result[i] = atual;
            }

            return result;
        }

        private State GerEstadoDeSaida(bool escolhido)
        {
            if (escolhido)
            {
                return State.Positive;
            }
            return State.Neutral;
        }
    

        private State GerEstadoDeSaidoFromdouble(double valor, TypeOfOutput tipo)
        {
            switch(tipo)
            {
                case TypeOfOutput.BiState:
                    return GetBiStateFromdouble(valor);
                case TypeOfOutput.TriState:
                    return GetTriStateFromdouble(valor);
                default:
                    throw new InvalidOperationException("Estado desconhecido");
            }
        }

        private State GetTriStateFromdouble(double valor)
        {
            double max = _neutralRange + _middle;
            double min = _middle - _neutralRange;

            if(valor > max)
            {
                return State.Positive;
            }
            if(valor < min)
            {
                return State.Negative;
            }
            return State.Neutral;
        }

        private State GetBiStateFromdouble(double valor)
        {
            if(valor > _middle)
            {
                return State.Positive;
            }
            else
            {
                return State.Negative;
            }
        }

        private double Get10Fromdouble(double valor)
        {
            if (valor > _middle)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        private double EncontraElementoDaNovaCamada(int numeroDoNeuronioNaCamada, double[][] pesos, double[] camadaAnterior)
        {
            double soma = 0;
            for(int i = 0; i < camadaAnterior.Length; i++)
            {
                soma = pesos[i][numeroDoNeuronioNaCamada]*camadaAnterior[i] + soma;
            }
            return soma;
        }
    }
}
