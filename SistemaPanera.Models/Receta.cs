using System;
using System.Collections.Generic;


namespace SistemaPanera.Models;
public partial class Receta
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string Sku { get; set; } = null!;

    public string Descripcion { get; set; } = null!;

    public int IdCategoria { get; set; }

    public int IdUnidadMedida { get; set; }

    public decimal CostoPrefabricados { get; set; }

    public decimal CostoInsumos { get; set; }

    public decimal CostoTotal { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual RecetasCategoria IdCategoriaNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<RecetasInsumo> RecetasInsumos { get; set; } = new List<RecetasInsumo>();

    public virtual ICollection<RecetasPrefabricado> RecetasPrefabricados { get; set; } = new List<RecetasPrefabricado>();
}
