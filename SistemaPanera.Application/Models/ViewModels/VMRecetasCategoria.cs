﻿using SistemaPanera.Models;

namespace SistemaPanera.Application.Models.ViewModels
{
    public class VMRecetasCategoria
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public virtual ICollection<Receta> Recetas { get; set; } = new List<Receta>();
    }
}
