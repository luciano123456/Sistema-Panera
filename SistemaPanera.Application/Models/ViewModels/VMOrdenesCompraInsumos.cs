using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SistemaPanera.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMOrdenesComprasInsumo
    {
        public int Id { get; set; }

        public int IdOrdenCompra { get; set; }

        public int IdInsumo { get; set; }

        public int? IdProveedorLista { get; set; }

        public decimal CantidadPedida { get; set; }

        public decimal CantidadEntregada { get; set; }

        public decimal CantidadRestante { get; set; }

        public decimal PrecioLista { get; set; }

        public decimal Subtotal { get; set; }

        public int IdEstado { get; set; }

        public string? NotaInterna { get; set; }
        public string? Nombre { get; set; }
        public string? Proveedor { get; set; }


        [ValidateNever]
        public virtual OrdenesComprasInsumosEstado IdEstadoNavigation { get; set; }

        [ValidateNever]
        public virtual Insumo IdInsumoNavigation { get; set; }

        [ValidateNever]
        public virtual OrdenesCompra IdOrdenCompraNavigation { get; set; }

        [ValidateNever]
        public virtual ProveedoresInsumos IdProveedorListaNavigation { get; set; }

    }

}
