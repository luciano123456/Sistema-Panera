using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMSubreceta

    {
        public int Id { get; set; }

        public int IdUnidadNegocio { get; set; }

        public string? Sku { get; set; }

        public string Descripcion { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!;
        public string UnidadNegocio { get; set; } = null!;

        public int IdUnidadMedida { get; set; }

        public int IdCategoria { get; set; }

        public decimal CostoPorcion { get; set; }

        public decimal? Rendimiento { get; set; }

        public decimal? CostoUnitario { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public virtual SubrecetasCategoria IdCategoriaNavigation { get; set; } = null!;

        public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

        public virtual ICollection<RecetasSubreceta> RecetasSubreceta { get; set; } = new List<RecetasSubreceta>();

        public virtual ICollection<SubrecetasInsumo> SubrecetasInsumos { get; set; } = new List<SubrecetasInsumo>();

        public virtual ICollection<SubrecetasStock> SubrecetasStocks { get; set; } = new List<SubrecetasStock>();

        public virtual ICollection<SubrecetasSubreceta> SubrecetasSubrecetaIdSubrecetaHijaNavigations { get; set; } = new List<SubrecetasSubreceta>();

        public virtual ICollection<SubrecetasSubreceta> SubrecetasSubrecetaIdSubrecetaPadreNavigations { get; set; } = new List<SubrecetasSubreceta>();

    }
}
