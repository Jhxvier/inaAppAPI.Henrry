using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IGenericServices<E>
    {
        Task<List<E>> ObtenerTodosAsync();
        Task<E> ObtenerPorIdAsync(int id);
        Task<E> CrearAsync(E entity);
        Task<E> ActualizarAsync(E entity);
        Task<bool> EliminarAsync(int id);
    }
}