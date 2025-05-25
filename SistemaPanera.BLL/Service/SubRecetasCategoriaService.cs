﻿using SistemaPanera.DAL.Repository;
using SistemaPanera.Models;

namespace SistemaPanera.BLL.Service
{
    public class SubrecetasCategoriaService : ISubRecetasCategoriaService
    {

        private readonly ISubRecetasCategoriaRepository<SubrecetasCategoria> _contactRepo;

        public SubrecetasCategoriaService(ISubRecetasCategoriaRepository<SubrecetasCategoria> contactRepo)
        {
            _contactRepo = contactRepo;
        }
        public async Task<bool> Actualizar(SubrecetasCategoria model)
        {
            return await _contactRepo.Actualizar(model);
        }

        public async Task<bool> Eliminar(int id)
        {
            return await _contactRepo.Eliminar(id);
        }

        public async Task<bool> Insertar(SubrecetasCategoria model)
        {
            return await _contactRepo.Insertar(model);
        }

        public async Task<SubrecetasCategoria> Obtener(int id)
        {
            return await _contactRepo.Obtener(id);
        }


        public async Task<IQueryable<SubrecetasCategoria>> ObtenerTodos()
        {
            return await _contactRepo.ObtenerTodos();
        }



    }
}
