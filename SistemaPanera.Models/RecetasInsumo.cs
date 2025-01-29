using System;
using System.Collections.Generic;


namespace SistemaPanera.Models;
public partial class RecetasInsumo
{
    public int Id { get; set; }

    public int IdReceta { get; set; }
    public int IdInsumo { get; set; }


    public int Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;

    public virtual Receta IdRecetaNavigation { get; set; } = null!;

}
