using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class SubrecetasSubreceta
{
    public int Id { get; set; }

    public int IdSubrecetaPadre { get; set; }

    public int IdSubrecetaHija { get; set; }

    public decimal Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Subreceta IdSubrecetaHijaNavigation { get; set; } = null!;

    public virtual Subreceta IdSubrecetaPadreNavigation { get; set; } = null!;
}
