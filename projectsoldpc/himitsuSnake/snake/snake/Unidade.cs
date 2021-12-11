using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace snake
{
    public class Unidade
    {
        public bool IsBody { get; set; }

        public bool IsFood { get; set; }

        public bool NoMoreBody { get; set; }

        public int Order { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
    }
}
