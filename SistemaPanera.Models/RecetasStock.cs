using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class RecetasStock
{
    public int Id { get; set; }

    public int IdLocal { get; set; }

    public int IdReceta { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Ingreso { get; set; }

    public decimal Egreso { get; set; }

    public virtual Local IdLocalNavigation { get; set; } = null!;
}
