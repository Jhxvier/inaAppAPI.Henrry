using inaApp.Common.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    public interface IGenericServices<TResponse, TCreate, TUpdate>
    {
        Task<Response<List<TResponse>>> ObtenerTodosAsync();
        Task<Response<TResponse>> ObtenerPorIdAsync(int id);
        Task<Response<TResponse>> CrearAsync(TCreate entity);
        Task<Response<TResponse>> ActualizarAsync(TUpdate entity);
        Task<Response<bool>> EliminarAsync(int id);
    }
}