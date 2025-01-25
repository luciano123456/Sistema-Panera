using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class ProductosInsumo
{
    public int Id { get; set; }

    public int? IdProducto { get; set; }

    public int? IdTipo { get; set; }

    public int? Cantidad { get; set; }

    public decimal? CostoUnitario { get; set; }

    public decimal? SubTotal { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
