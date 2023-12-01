using System;
using System.Collections.Generic;

namespace DL;

public partial class ProductoInventario
{
    public int IdProductoInventario { get; set; }

    public int? IdProducto { get; set; }

    public decimal? Cantidad { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }

    public virtual ICollection<UsuarioProducto> UsuarioProductos { get; set; } = new List<UsuarioProducto>();
}
