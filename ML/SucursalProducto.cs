using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class SucursalProducto
    {
        public int IdSucursalProducto { get; set; }
        public int Cantidad { get; set; }
        public ML.Sucursal? Sucursal { get; set; }
        public ML.Producto? Producto { get; set; }

        public List<object> SucursalProductos { get; set; }
    }
}
