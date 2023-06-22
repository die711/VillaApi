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

    [HttpGet]
    public async Task<ActionResult<APIResponse>> GetNumeroVillas()
    {
        try
        {
            _logger.LogInformation("Obtener numeros de villas");
            IEnumerable<NumeroVilla> numeroVillas = await _numeroVillaRepo.ObtenerTodos(incluirPropiedades: "Villa");
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

}