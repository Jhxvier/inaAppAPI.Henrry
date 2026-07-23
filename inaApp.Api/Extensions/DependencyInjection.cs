using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.DTOs.CategoriaProducto;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using inaApp.DTOs.Factura;
using inaApp.Entities;
using inaApp.Repository;
using inaApp.Services;
using inaApp.Services.Mapping;
using Microsoft.EntityFrameworkCore;

namespace inaApp.Api.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAplicationServices
            (this IServiceCollection services, 
            IConfiguration configuration)
        {

            //base de datos dbContext

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //profile auto mapper
            services.AddAutoMapper(cfg => { },typeof(MappingProfile));
            services.AddScoped<IFacturaService<FacturaResponseDTO, FacturaListDTO, FacturaCreateDTO>,
                FacturaService>();




            //inyecciones de dependencia de servicios
            services.AddScoped<IGenericServices<ProductoResponseDTO, ProductoCreateDTO, ProductoUpdateDTO>, ProductoService>();
            services.AddScoped<IGenericServices<ClienteResponseDTO, ClienteCreateDTO, ClienteUpdateDTO>, ClienteService>();
            services.AddScoped<IGenericServices<CategoriaProductoResponseDTO, CategoriaProductoCreateDTO, CategoriaProductoUpdateDTO>, CategoriaService>();
            services.AddScoped<IFacturaRepository<Factura>, FacturaRepository>();

            //inyecciones de dependencia de repositorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();
            services.AddScoped<IGenericRepository<Categoria>, CategoriaRepository>();

            return services;
        }
    }
}
