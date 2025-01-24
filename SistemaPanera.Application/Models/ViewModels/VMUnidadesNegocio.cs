using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMUnidadesNegocio
    {
        public int Id { get; set; }

        public string? Nombre { get; set; }

        public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();

        public virtual ICollection<Local> Locales { get; set; } = new List<Local>();
    }
}
