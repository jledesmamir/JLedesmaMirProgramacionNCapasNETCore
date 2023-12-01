using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    [Serializable]
    public class Area
    {
        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public List<object> Areas { get; set; }
    }
}
