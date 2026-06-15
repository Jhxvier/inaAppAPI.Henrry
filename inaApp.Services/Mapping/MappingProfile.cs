using AutoMapper;
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
            CreateMap<ProductoCreateDTO, Producto>();
            CreateMap<ClienteCreateDTO, Cliente>();



            //DE DTOUPDATE A ENTITY

            CreateMap<ProductoUpdateDTO, Producto>();
            CreateMap<ClienteUpdateDTO, Cliente>();


            //ENTITY A DTOs Response

            CreateMap<Producto, ProductoResponseDTO>();
            CreateMap<Cliente, ClienteResponseDTO>();



        }

    }
}
