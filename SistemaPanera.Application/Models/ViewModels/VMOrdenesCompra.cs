using SistemaPanera.Models;
using System.ComponentModel.DataAnnotations;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMOrdenesCompra
    {
        public int Id { get; set; }

        [Required]
        public int IdUnidadNegocio { get; set; }

        [Required]
        public int IdLocal { get; set; }

        [Required]
        public DateTime FechaEmision { get; set; }

        public DateTime? FechaEntrega { get; set; }

        [Required]
        public int IdProveedor { get; set; }

        [Required]
        public int IdEstado { get; set; }

        public string? NotaInterna { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El costo total debe ser mayor a cero")]
        public decimal CostoTotal { get; set; }

        [Required]
        public List<VMOrdenesComprasInsumo> OrdenesComprasInsumos { get; set; } = new();
    }


}
