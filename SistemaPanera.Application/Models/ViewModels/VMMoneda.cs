using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMMoneda
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public decimal Cotizacion { get; set; }

        public string? Image { get; set; }

    }
}
