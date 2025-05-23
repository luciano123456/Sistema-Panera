using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMPrefabricado

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

        public decimal CostoTotal { get; set; }

        public DateTime FechaActualizacion { get; set; }

        //public virtual PrefabricadosCategoria IdCategoriaNavigation { get; set; } = null!;

        public virtual UnidadesMedida IdUnidadMedidaNavigation { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

        //public virtual ICollection<PrefabricadosInsumo> PrefabricadosInsumos { get; set; } = new List<PrefabricadosInsumo>();

        //public virtual ICollection<PrefabricadosStock> PrefabricadosStocks { get; set; } = new List<PrefabricadosStock>();

    }
}
