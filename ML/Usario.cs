using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usario
    {
        public string Username{ get; set; }
        public string email{ get; set; }
        public string Id{ get; set; }
        public List<string> Roles { get; set; }

        public VentaProducto VentaProducto { get; set;}

    }
}
