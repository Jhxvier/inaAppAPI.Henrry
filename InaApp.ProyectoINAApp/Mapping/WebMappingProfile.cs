using AutoMapper;
using inaApp.DTOs.CategoriaProducto;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using InaApp.ProyectoINAApp.Models.Categoria;
using InaApp.ProyectoINAApp.Models.Cliente;
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
            CreateMap<CategoriaProductoResponseDTO, CategoriaIndexViewModel>();
            CreateMap<CategoriaProductoResponseDTO, CategoriaEditViewModel>();
            CreateMap<ClienteResponseDTO, ClienteIndexViewModel>();
            CreateMap<ClienteResponseDTO, ClienteEditViewModel>();

            //VIEWMODEL A DTO
            CreateMap<ProductoIndexViewModel, ProductoResponseDTO>();
            CreateMap<ProductoCreateViewModel, ProductoCreateDTO>();
            CreateMap<ProductoEditViewModel, ProductoUpdateDTO>();
            CreateMap<CategoriaCreateViewModel, CategoriaProductoCreateDTO>();
            CreateMap<CategoriaEditViewModel, CategoriaProductoUpdateDTO>();
            CreateMap<ClienteCreateViewModel, ClienteCreateDTO>();
            CreateMap<ClienteEditViewModel, ClienteUpdateDTO>();


        }
    }
}
