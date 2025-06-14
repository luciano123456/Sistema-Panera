using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMOrdenesComprasInsumosEstado
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<OrdenesCompra> OrdenesCompras { get; set; } = new List<OrdenesCompra>();


    }
}
