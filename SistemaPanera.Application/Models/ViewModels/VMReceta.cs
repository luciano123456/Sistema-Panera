using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMReceta
    {
    public int Id { get; set; }

        public int IdUnidadNegocio { get; set; }

        public string Sku { get; set; } = null!;

        public string Descripcion { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public string UnidadMedida { get; set; } = null!;
        public string UnidadNegocio { get; set; } = null!;

        public int IdCategoria { get; set; }

        public int IdUnidadMedida { get; set; }

        public decimal CostoSubrecetas { get; set; }

        public decimal CostoInsumos { get; set; }

        public decimal CostoTotal { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public virtual RecetasCategoria IdCategoriaNavigation { get; set; } = null!;

        public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

        public virtual ICollection<RecetasInsumo> RecetasInsumos { get; set; } = new List<RecetasInsumo>();

        public virtual ICollection<RecetasStock> RecetasStocks { get; set; } = new List<RecetasStock>();
        public virtual ICollection<RecetasSubreceta> RecetasSubrecetas { get; set; } = new List<RecetasSubreceta>();

    }
}
