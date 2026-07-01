using AutoMapper;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Producto;

namespace InaApp.ProyectoINAApp.Mapping
{
    public class WebMappingProfile : Profile
    {
        public WebMappingProfile()
        {
            //DTO A VIEWMODEL
            CreateMap<ProductoResponseDTO, ProductoIndexViewModel>();
            CreateMap<ProductoResponseDTO, ProductoEditViewModel>();

            //VIEWMODEL A DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>();
        }
    }
}
