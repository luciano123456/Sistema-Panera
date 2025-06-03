using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMReceta
    {
        public int Id { get; set; }

        public int IdUnidadNegocio { get; set; }

        public string Sku { get; set; } = null!;

        public string Descripcion { get; set; } = null!;

        public int IdUnidadMedida { get; set; }

        public int IdCategoria { get; set; }

        public decimal? CostoSubRecetas { get; set; }

        public decimal CostoInsumos { get; set; }

        public decimal CostoPorcion { get; set; }

        public decimal? Rendimiento { get; set; }

        public decimal? CostoUnitario { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public virtual RecetasCategoria IdCategoriaNavigation { get; set; } = null!;

        public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

        public virtual ICollection<RecetasInsumo> RecetasInsumos { get; set; } = new List<RecetasInsumo>();

        public virtual ICollection<RecetasSubreceta> RecetasSubreceta { get; set; } = new List<RecetasSubreceta>();

        public string Categoria { get; set; }
        public string UnidadMedida { get; set; }
        public string UnidadNegocio { get; set; }

    }
}
