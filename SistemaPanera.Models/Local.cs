using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class Local
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<RecetasStock> RecetasStocks { get; set; } = new List<RecetasStock>();

    public virtual ICollection<SubrecetasStock> SubrecetasStocks { get; set; } = new List<SubrecetasStock>();
}
