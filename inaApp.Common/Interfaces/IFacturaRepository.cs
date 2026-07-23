using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Common.Interfaces
{
    // Reutiliza las operaciones crud y declara solo las consultas propias de Factura
    public interface IFacturaRepository<T> : IGenericRepository<T>
    {
        Task AnularAsync(T factura);
    }
}
