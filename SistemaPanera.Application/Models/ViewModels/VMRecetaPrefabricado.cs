using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMRecetaPrefabricado
    {
        public int Id { get; set; }

        public int IdReceta { get; set; }

        public int IdPrefabricado { get; set; }

        public int Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public string Nombre { get; set; }

        //public virtual Prefabricado IdPrefabricadoNavigation { get; set; } = null!;

        public virtual Receta IdRecetaNavigation { get; set; } = null!;
    }


}
