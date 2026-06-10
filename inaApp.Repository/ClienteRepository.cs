using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Repository
{
    public class ClienteRepository : IGenericRepository<Cliente>
    {
        private readonly ApplicationDbContext _dbContext;

        public ClienteRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Cliente> ActualizarAsync(Cliente entity)
        {
            try
            {
                _dbContext.Cliente.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Cliente> CrearAsync(Cliente entity)
        {
            try
            {
                _dbContext.Cliente.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try
            {
                var cliente = await ObtenerPorIdAsync(id);

                if (cliente == null)
                {
                    return false;
                }

                cliente.Estado = false;
                _dbContext.Cliente.Update(cliente);
                await _dbContext.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Cliente> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _dbContext.Cliente.Where(x => x.Id == id && x.Estado == true).SingleOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Cliente>> ObtenerTodosAsync()
        {
            try
            {
                return await _dbContext.Cliente.AsNoTracking().Where(x => x.Estado == true).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
