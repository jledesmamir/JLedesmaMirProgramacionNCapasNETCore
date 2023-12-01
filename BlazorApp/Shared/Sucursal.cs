using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class Sucursal
    {
        public int IdSucursal { get; set; }
        public string? Nombre { get; set; }
        public string? Direccion { get; set; }
        public string? PaginaWeb { get; set; }

        public BlazorApp.Shared.Producto? Producto { get; set; }
        public List<object> Sucurales { get; set; }
    }
}
