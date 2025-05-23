using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class SubrecetasInsumo
{
    public int Id { get; set; }

    public int IdSubreceta { get; set; }

    public int IdInsumo { get; set; }

    public decimal Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual Subreceta IdSubrecetaNavigation { get; set; } = null!;
}
