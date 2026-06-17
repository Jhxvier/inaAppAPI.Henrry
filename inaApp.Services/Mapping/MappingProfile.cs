using AutoMapper;
using inaApp.DTOs.CategoriaProducto;
using inaApp.DTOs.Cliente;
using inaApp.DTOs.Producto;
using inaApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace inaApp.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            //DE DTOCREATE A ENTITY
            CreateMap<ProductoCreateDTO, Producto>()
                .ForMember(destino => destino.CategoriaId, opcion => opcion.MapFrom(origen => origen.CategoriaProductoId));
            CreateMap<ClienteCreateDTO, Cliente>();
            CreateMap<CategoriaProductoCreateDTO, Categoria>();



            //DE DTOUPDATE A ENTITY

            CreateMap<ProductoUpdateDTO, Producto>()
                .ForMember(destino => destino.CategoriaId, opcion => opcion.MapFrom(origen => origen.CategoriaProductoId));
            CreateMap<ClienteUpdateDTO, Cliente>();
            CreateMap<CategoriaProductoUpdateDTO, Categoria>();


            //ENTITY A DTOs Response

            CreateMap<Producto, ProductoResponseDTO>()
                .ForMember(destino => destino.CategoriaProductoId, opcion => opcion.MapFrom(origen => origen.CategoriaId))
                .ForMember(destino => destino.CategoriaProductoNombre, opcion => opcion.MapFrom(origen => origen.Categoria.Nombre));
            CreateMap<Cliente, ClienteResponseDTO>();
            CreateMap<Categoria, CategoriaProductoResponseDTO>();



        }

    }
}
