using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMProductosTipo
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
    }
}
