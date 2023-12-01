using System;
using System.Collections.Generic;

namespace DL;

public partial class VentaProducto
{
    public int IdProductoVenta { get; set; }

    public int CantidadProductoVenta { get; set; }

    public string? IdUsuario { get; set; }

    public int? IdSucursalProducto { get; set; }

    public virtual SucursalProducto? IdSucursalProductoNavigation { get; set; }

    public virtual AspNetUser? IdUsuarioNavigation { get; set; }
}
