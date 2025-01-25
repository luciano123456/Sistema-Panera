using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMInsumo
    {
        public int Id { get; set; }

        public int IdUnidadNegocio { get; set; }

        public string? Sku { get; set; }

        public string Descripcion { get; set; }

        public string UnidadNegocio { get; set; }
        public string UnidadMedida { get; set; }
        public string Categoria { get; set; }

        public int IdUnidadMedida { get; set; }

        public int IdCategoria { get; set; }

        public decimal CostoUnitario { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public virtual InsumosCategoria IdCategoriaNavigation { get; set; } = null!;

        public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    }
}
