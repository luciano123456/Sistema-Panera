using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class PrefabricadosStock
{
    public int Id { get; set; }

    public int IdLocal { get; set; }

    public int IdPrefabricado { get; set; }

    public string TipoMovimiento { get; set; } = null!;

    public decimal Ingreso { get; set; }

    public decimal Egreso { get; set; }

    public virtual Local IdLocalNavigation { get; set; } = null!;

    public virtual Prefabricado IdPrefabricadoNavigation { get; set; } = null!;
}
