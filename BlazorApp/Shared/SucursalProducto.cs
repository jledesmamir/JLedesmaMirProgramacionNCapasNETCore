using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class SucursalProducto
    {
        public int IdSucursalProducto { get; set; }
        public int Cantidad { get; set; }
        public BlazorApp.Shared.Sucursal? Sucursal { get; set; }
        public BlazorApp.Shared.Producto? Producto { get; set; }

        public List<object> SucursalProductos { get; set; }
    }
}
