using System;
using System.Collections.Generic;

namespace DL;

public partial class Sucursal
{
    public int IdSucursal { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? PaginaWeb { get; set; }

    public virtual ICollection<SucursalProducto> SucursalProductos { get; set; } = new List<SucursalProducto>();
}
