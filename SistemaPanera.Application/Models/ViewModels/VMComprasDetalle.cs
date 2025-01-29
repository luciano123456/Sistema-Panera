using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMComprasDetalle

    {
        public int Id { get; set; }

        public int IdInsumo { get; set; }

        public int IdCompra { get; set; }

        public decimal Cantidad { get; set; }
        public string Nombre { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public virtual Compra IdCompraNavigation { get; set; } = null!;

        public virtual Insumo IdInsumoNavigation { get; set; } = null!;


    }
}
