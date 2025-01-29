using System;
using System.Collections.Generic;


namespace SistemaPanera.Models;
public partial class Prefabricado
{
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string? Sku { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdUnidadMedida { get; set; }

    public int IdCategoria { get; set; }

    public decimal CostoTotal { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual PrefabricadosCategoria IdCategoriaNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<PrefabricadosInsumo> PrefabricadosInsumos { get; set; } = new List<PrefabricadosInsumo>();

    public virtual ICollection<PrefabricadosStock> PrefabricadosStocks { get; set; } = new List<PrefabricadosStock>();

    public virtual ICollection<RecetasPrefabricado> RecetasPrefabricados { get; set; } = new List<RecetasPrefabricado>();
}
