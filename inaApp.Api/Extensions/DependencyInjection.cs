using inaApp.Common.Interfaces;
using inaApp.Data;
using inaApp.Entities;
using inaApp.Repository;
using inaApp.Services;
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




            //inyecciones de dependencia de servicios
            services.AddScoped<IGenericServices<Producto>, ProductoService>();
            services.AddScoped<IGenericServices<Cliente>, ClienteService>();



            //inyecciones de dependencia de repositorios
            services.AddScoped<IGenericRepository<Producto>, ProductoRepository>();
            services.AddScoped<IGenericRepository<Cliente>, ClienteRepository>();

            return services;
        }
    }
}
