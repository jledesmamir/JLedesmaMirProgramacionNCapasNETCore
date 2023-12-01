using System;
using System.Collections.Generic;

namespace DL;

public partial class Usuario
{
    public int IdUsuario { get; set; }

    public string Curp { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string EmailEmpresarial { get; set; } = null!;

    public string Sexo { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Celular { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public byte[]? Foto { get; set; }

    public int? IdUsuarioModificacion { get; set; }

    public virtual Usuario? IdUsuarioModificacionNavigation { get; set; }

    public virtual ICollection<Usuario> InverseIdUsuarioModificacionNavigation { get; set; } = new List<Usuario>();

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();

    public virtual ICollection<UsuarioFecha> UsuarioFechas { get; set; } = new List<UsuarioFecha>();

    public virtual ICollection<UsuarioProducto> UsuarioProductoIdUsuarioModificacionNavigations { get; set; } = new List<UsuarioProducto>();

    public virtual ICollection<UsuarioProducto> UsuarioProductoIdUsuarioNavigations { get; set; } = new List<UsuarioProducto>();
}
