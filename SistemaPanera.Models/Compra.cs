using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class Compra
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public int IdLocal { get; set; }

    public DateTime Fecha { get; set; }

    public int IdProveedor { get; set; }

    public string? NumeroOrden { get; set; }

    public decimal Costo { get; set; }

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;
}
