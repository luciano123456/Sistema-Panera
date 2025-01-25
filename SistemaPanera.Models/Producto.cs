using System;
using System.Collections.Generic;

namespace SistemaPanera.Models;

public partial class Producto
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string Sku { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int IdCategoria { get; set; }

    public int IdUnidadMedida { get; set; }

    public decimal CostoUnitario { get; set; }

    public decimal CostoInsumos { get; set; }

    public decimal CostoTotal { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual ProductosCategoria IdCategoriaNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<ProductosInsumo> ProductosInsumos { get; set; } = new List<ProductosInsumo>();

    public virtual ICollection<ProductosStock> ProductosStocks { get; set; } = new List<ProductosStock>();
}
