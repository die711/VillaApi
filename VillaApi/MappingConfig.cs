using AutoMapper;
using VillaApi.Models;
using VillaApi.Models.Dto;

namespace VillaApi;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>().ReverseMap();
        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();

        CreateMap<NumeroVilla, NumeroVillaDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaCreateDto>().ReverseMap();
        CreateMap<NumeroVilla, NumeroVillaUpdateDto>().ReverseMap();

        CreateMap<UsuarioAplicacion, UsuarioDto>().ReverseMap();
    }
}