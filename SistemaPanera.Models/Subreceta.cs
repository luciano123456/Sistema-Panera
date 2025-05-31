using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaPanera.Models;

public partial class Subreceta
{

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public int IdUnidadNegocio { get; set; }

    public string? Sku { get; set; }

    public string Descripcion { get; set; } = null!;

    public int IdUnidadMedida { get; set; }

    public int IdCategoria { get; set; }

    public decimal CostoPorcion { get; set; }

    public decimal? CostoSubRecetas { get; set; }

    public decimal? CostoInsumos { get; set; }

    public decimal? Rendimiento { get; set; }

    public decimal? CostoUnitario { get; set; }

    public DateTime FechaActualizacion { get; set; }

    public virtual SubrecetasCategoria IdCategoriaNavigation { get; set; } = null!;

    public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

    public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    public virtual ICollection<RecetasSubreceta> RecetasSubreceta { get; set; } = new List<RecetasSubreceta>();

    public virtual ICollection<SubrecetasInsumo> SubrecetasInsumos { get; set; } = new List<SubrecetasInsumo>();

    public virtual ICollection<SubrecetasStock> SubrecetasStocks { get; set; } = new List<SubrecetasStock>();

    public virtual ICollection<SubrecetasSubreceta> SubrecetasSubrecetaIdSubRecetaHijaNavigations { get; set; } = new List<SubrecetasSubreceta>();

    public virtual ICollection<SubrecetasSubreceta> SubrecetasSubrecetaIdSubRecetaPadreNavigations { get; set; } = new List<SubrecetasSubreceta>();
}
