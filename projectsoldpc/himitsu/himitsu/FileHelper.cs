using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace himitsu
{
    public static class FileHelper
    {
        static string _directory = @"C:\Users\hiddenman\Desktop\teste";
        public static void GravaGeracao(List<SerVivo> toSave, string filePath = null)
        {
            if(string.IsNullOrEmpty(filePath))
            {
                filePath = _directory;
            }

            Directory.CreateDirectory(filePath);
            string serialized = JsonConvert.SerializeObject(toSave);

            string fileName = filePath + "/" + toSave[0].Geracao;
            File.WriteAllText(fileName, serialized);
        }
        public static List<SerVivo> PegaGeracao(int geracao, string filePath = null)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                filePath = _directory;
            }
            string fileName = filePath + "/" + geracao;
            string jsonString = File.ReadAllText(fileName);
            List<SerVivo> result = JsonConvert.DeserializeObject<List<SerVivo>>(jsonString);

            return result;
        }
    }
}
