using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class VentaProducto
    {
        public int IdProductoVenta { get; set; }
        public int CantidadProductoVenta { get; set; }
        public List<object> VentaProductos { get; set; }

        public BlazorApp.Shared.SucursalProducto? SucursalProductos { get; set; }
        public string IdUsuario {  get; set; }//propiedad para empatar IdUsuario de compra
        public string UsuarioNombre {  get; set; }//propiedad para empatar IdUsuario de compra
    }
}
