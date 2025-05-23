using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;
public partial class RecetasSubreceta
{
    public int Id { get; set; }

    public int IdReceta { get; set; }

    public int IdSubReceta { get; set; }

    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual Receta IdRecetaNavigation { get; set; } = null!;

    public virtual Subreceta IdSubRecetaNavigation { get; set; } = null!;
}
