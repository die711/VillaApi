using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers.v2;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("2.0")]
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
    
  
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "valor1", "valor2" };
    }
}