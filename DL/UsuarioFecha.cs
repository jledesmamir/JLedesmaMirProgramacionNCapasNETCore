using System;
using System.Collections.Generic;

namespace DL;

public partial class UsuarioFecha
{
    public int IdUsuarioFecha { get; set; }

    public int? IdUsuario { get; set; }

    public DateTime? FechaIngreso { get; set; }

    public DateTime? FechaSalida { get; set; }

    public string? Observaciones { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
