using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class ProductosStock
{
    public int Id { get; set; }

    public int? IdLocal { get; set; }

    public int? IdProducto { get; set; }

    public string? TipoMovimiento { get; set; }

    public decimal? Ingreso { get; set; }

    public decimal? Egreso { get; set; }

    public virtual Local? IdLocalNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
