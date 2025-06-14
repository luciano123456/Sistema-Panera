using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class SubrecetasSubreceta
{
    public int Id { get; set; }

    public int IdSubRecetaPadre { get; set; }

    public int IdSubRecetaHija { get; set; }

    public decimal Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal Subtotal { get; set; }

    public virtual Subreceta IdSubRecetaHijaNavigation { get; set; } = null!;

    public virtual Subreceta IdSubRecetaPadreNavigation { get; set; } = null!;
}
