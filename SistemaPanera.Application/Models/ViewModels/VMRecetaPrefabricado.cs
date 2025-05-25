using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMRecetaSubreceta
    {
        public int Id { get; set; }

        public int IdReceta { get; set; }

        public int IdSubreceta { get; set; }

        public int Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public string Nombre { get; set; }

        //public virtual Subreceta IdSubrecetaNavigation { get; set; } = null!;

        public virtual Receta IdRecetaNavigation { get; set; } = null!;
    }


}
