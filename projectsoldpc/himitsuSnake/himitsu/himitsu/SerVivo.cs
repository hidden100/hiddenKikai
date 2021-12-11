using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public class SerVivo
    {
        public string NomeUnico { get; set; }
        public double Pontuacao { get; set; }
        public Dictionary<int,double[][]> Pesos { get; set; }
        public int Geracao { get; set; }

        internal SerVivo Clone()
        {
            SerVivo clone = new SerVivo();

            clone.Geracao = Geracao;
            clone.NomeUnico = NomeUnico;
            clone.Pesos = ClonePesos(Pesos);
            clone.Pontuacao = Pontuacao;

            return clone;
        }
        public int NumeroIteracoes { get; set; }

        private Dictionary<int, double[][]> ClonePesos(Dictionary<int, double[][]> pesos)
        {
            Dictionary<int, double[][]> clone = new Dictionary<int, double[][]>();

            foreach(KeyValuePair<int, double[][]> camada in pesos)
            {
                double[][] linha = new double[camada.Value.Length][];
                for(int i = 0; i < camada.Value.Length; i++)
                {
                    linha[i] = new double[camada.Value[i].Length];
                    for(int j = 0; j < linha[i].Length; j++)
                    {
                        linha[i][j] = camada.Value[i][j];
                    }
                }
                clone.Add(camada.Key, linha);
            }
            return clone;
        }
    }
}
