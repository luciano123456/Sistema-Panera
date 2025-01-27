using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMProductoInsumo
    {
        public int Id { get; set; }

        public int? IdProducto { get; set; }

        public int? IdTipo { get; set; }
        public string Tipo { get; set; }

        public int? Cantidad { get; set; }

        public decimal? CostoUnitario { get; set; }

        public decimal? SubTotal { get; set; }

        public virtual Producto? IdProductoNavigation { get; set; }

        public virtual ProductosTipo? IdProductosTipoNavigation { get; set; }

    }
}
