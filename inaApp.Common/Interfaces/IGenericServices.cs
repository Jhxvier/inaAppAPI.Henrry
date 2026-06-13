using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IGenericServices<TResponse, TCreate, TUpdate>
    {
        Task<List<TResponse>> ObtenerTodosAsync();
        Task<TResponse> ObtenerPorIdAsync(int id);
        Task<TResponse> CrearAsync(TCreate entity);
        Task<TResponse> ActualizarAsync(TUpdate entity);
        Task<bool> EliminarAsync(int id);
    }
}