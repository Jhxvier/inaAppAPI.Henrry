using AutoMapper;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models;

namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DTO A VIEWMODEL
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();

            //VIEWMODEL A DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
        }
    }
}
