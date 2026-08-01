using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using inaApp.Common.Interfaces;
using inaApp.Entities;
using inaApp.Data;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Repository
{
    public class ProductoRepository : IGenericRepository<Producto>
    {

        //inyección de dependencia para acceder al contexto de la base de datos
        private readonly ApplicationDbContext _dbContext;

        //constructor para inicializar el contexto de la base de datos
        public ProductoRepository(ApplicationDbContext context)
        {

            _dbContext = context;
        }



        public async Task<Producto> ActualizarAsync(Producto entity)
        {
            try 
            {
                _dbContext.Producto.Update(entity); // actualizar el producto en el contexto de la base de datos
                await _dbContext.SaveChangesAsync(); // guardar los cambios en la base de datos
                return await ObtenerPorIdAsync(entity.Id); // retornar el producto actualizado con su categoría
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Producto> CrearAsync(Producto entity)
        {
            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                // Se utiliza un valor temporal único porque el Id de identidad
                // todavía no existe antes del primer guardado.
                entity.Codigo = $"TMP-{Guid.NewGuid():N}"[..30];
                _dbContext.Producto.Add(entity);
                await _dbContext.SaveChangesAsync();

                // El código definitivo se deriva del Id generado por SQL Server.
                entity.Codigo = $"PROD-{entity.Id}";
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return await ObtenerPorIdAsync(entity.Id); // retornar el producto creado con su categoría
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> EliminarAsync(int id)
        {
            try 
            {
                //obtener el producto por id utilizando el método ObtenerPorIdAsync
                var producto = await ObtenerPorIdAsync(id); 

                if (producto == null) // si el producto no existe, retornar false
                {
                    return false;
                }

                //borrado logico
                producto.estado = false; // cambiar el estado del producto a false para indicar que está eliminado
                _dbContext.Producto.Update(producto); // actualizar el producto en el contexto de la base de datos
                await _dbContext.SaveChangesAsync(); // guardar los cambios en la base de datos

                return true; // retornar true para indicar que el producto fue eliminado exitosamente
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Producto> ObtenerPorIdAsync(int id)
        {
            try
            {
                return await _dbContext.Producto
                    .AsNoTracking() // no realizar seguimiento de cambios en la entidad para mejorar el rendimiento
                      .Include(x => x.Categoria)
                      .Where(x => x.Id == id && x.estado == true)
                      .SingleOrDefaultAsync(); // obtener el producto por id y estado activo (estado == true)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Producto>> ObtenerTodosAsync()
        {

            try
            {
                return await _dbContext.Producto
                    .AsNoTracking()
                    .Include(x => x.Categoria)
                    .Where(x => x.estado == true)
                    .ToListAsync(); // expresion lambda para filtrar los productos activos (estado == true)
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

