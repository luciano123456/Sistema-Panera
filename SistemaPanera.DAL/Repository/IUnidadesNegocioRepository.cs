﻿using SistemaPanera.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SistemaPanera.DAL.Repository
{
    public interface IUnidadesNegocioRepository<TEntityModel> where TEntityModel : class
    {
        Task<bool> Eliminar(int id);
        Task<bool> Actualizar(UnidadesNegocio model);
        Task<bool> Insertar(UnidadesNegocio model);
        Task<UnidadesNegocio> Obtener(int id);
        Task<IQueryable<UnidadesNegocio>> ObtenerTodos();
    }
}
