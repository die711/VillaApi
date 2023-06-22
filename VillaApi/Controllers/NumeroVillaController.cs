using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NumeroVillaController : ControllerBase
{
    private readonly ILogger<NumeroVillaController> _logger;
    private readonly IVillaRepositorio _villaRepo;
    private readonly INumeroVillaRepositorio _numeroVillaRepo;
    private readonly IMapper _mapper;
    private APIResponse _response;

    public NumeroVillaController(ILogger<NumeroVillaController> logger, IVillaRepositorio villaRepo,
                                 INumeroVillaRepositorio numeroVillaRepo, IMapper mapper)
    {
        _logger = logger;
        _villaRepo = villaRepo;
        _numeroVillaRepo = numeroVillaRepo;
        _mapper = mapper;
        _response = new();
    }
    
    
}