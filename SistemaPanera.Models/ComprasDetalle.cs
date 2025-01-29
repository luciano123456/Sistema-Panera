using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class ComprasDetalle
{
    public int Id { get; set; }

    public int IdInsumo { get; set; }

    public int IdCompra { get; set; }

    public decimal Cantidad { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal SubTotal { get; set; }

    public virtual Compra IdCompraNavigation { get; set; } = null!;

    public virtual Insumo IdInsumoNavigation { get; set; } = null!;
}
