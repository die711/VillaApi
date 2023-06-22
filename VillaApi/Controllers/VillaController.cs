using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly ILogger<VillaController> _logger;
    private readonly IVillaRepositorio _villaRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaController(ILogger<VillaController> logger,IVillaRepositorio villaRepo, IMapper mapper)
    {
        _logger = logger;
        _villaRepo = villaRepo;
        _mapper = mapper;
        _response = new();
    }
    
    [HttpGet]
    public async Task<ActionResult<APIResponse>> GetVillas()
    {
        try
        {
            _logger.LogInformation("Obtener las villas");

            IEnumerable<Villa> villaList = await _villaRepo.ObtenerTodos();
            _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.EstatusCode = HttpStatusCode.OK;
            
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa con Id "+ id);
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            
            var villa =await _villaRepo.Obtener(x => x.Id == id);

            if (villa == null)
            {
                _response.EstatusCode = HttpStatusCode.NotFound;
                _response.IsExitoso = false;
                return NotFound(_response);
            }
            

            _response.Resultado = _mapper.Map<VillaDto>(villa);
            _response.EstatusCode = HttpStatusCode.OK;
            return Ok(_response);

        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }
    
    
    
}