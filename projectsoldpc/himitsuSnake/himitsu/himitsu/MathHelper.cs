using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public static class MathHelper
    {
        public static double GetMedia(List<double> valores)
        {
            if(valores == null)
            {
                throw new InvalidOperationException("quer tirar média de null mesmo???");
            }
            double soma = 0;
            foreach(double valor in valores)
            {
                soma = soma + valor;
            }
            double media = soma / valores.Count;
            return media;
        }

        internal static double GetMediaFiltrandoExtremos(List<double> valores)
        {
            double media = GetMedia(valores);

            List<double> valoresFiltrados = new List<double>();

            foreach(double v in valores)
            {
                if(Math.Abs(v- media)/media <= 1)
                {
                    valoresFiltrados.Add(v);
                }
            }
            double mediaDepois = GetMedia(valoresFiltrados);
            return mediaDepois;
        }
    }
}
