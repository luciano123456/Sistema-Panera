﻿using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMSubrecetasInsumo

    {
        public int Id { get; set; }

        public int IdSubreceta { get; set; }

        public int IdInsumo { get; set; }
        public string Nombre { get; set; }

        public decimal Cantidad { get; set; }

        public decimal CostoUnitario { get; set; }

        public decimal SubTotal { get; set; }

        public virtual Insumo IdInsumoNavigation { get; set; } = null!;

        public virtual Subreceta IdSubrecetaNavigation { get; set; } = null!;


    }
}
