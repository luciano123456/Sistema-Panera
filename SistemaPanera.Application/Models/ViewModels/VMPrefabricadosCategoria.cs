using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMPrefabricadosCategoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();
    }
}
