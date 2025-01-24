using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMLocal
    {
        public int Id { get; set; }

        public int IdUnidadNegocio { get; set; }

        public string Nombre { get; set; } = null!;
        public string UnidadNegocio { get; set; } = null!;

        public virtual UnidadesNegocio IdUnidadNegocioNavigation { get; set; } = null!;

    }
}
