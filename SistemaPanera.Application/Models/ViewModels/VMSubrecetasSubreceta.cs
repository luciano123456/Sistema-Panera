using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMSubrecetasSubreceta
    {
        public int Id { get; set; }

        public int IdSubRecetaPadre { get; set; }

        public int IdSubRecetaHija { get; set; }

        public decimal Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal SubTotal { get; set; }
        public string Nombre { get; set; }

        public virtual Subreceta IdSubRecetaHijaNavigation { get; set; } = null!;

        public virtual Subreceta IdSubRecetaPadreNavigation { get; set; } = null!;

    }
}
