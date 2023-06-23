using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VillaApi.Models;
using VillaApi.Models.Dto;
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse>> GetNumeroVillas()
    {
        try
        {
            _logger.LogInformation("Obtener numeros de villas");
            IEnumerable<NumeroVilla> numeroVillas = await _numeroVillaRepo.ObtenerTodos(incluirPropiedades: "Villa");

            _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillas);
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

    [HttpGet("Villa/{villaId:int}")]
    public async Task<ActionResult<APIResponse>> GetNumeroVillasByVillaId(int villaId)
    {
        try
        {
            _logger.LogInformation("Obtener numeros de villas");

            IEnumerable<NumeroVilla> numeroVillas =
                await _numeroVillaRepo.ObtenerTodos(x => x.VillaId == villaId, incluirPropiedades: "Villa");

            _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numeroVillas);
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

    [HttpGet("id", Name = "GetNumeroVilla")]
    public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer numero villa con Id "+id);
                _response.EstatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            var numeroVilla = await _numeroVillaRepo.Obtener(x => x.VillaNo == id, incluirPropiedades:"Villa");

            if (numeroVilla == null)
            {
                _response.EstatusCode = HttpStatusCode.NotFound;
                _response.IsExitoso = false;
                return NotFound();
            }

            _response.Resultado = _mapper.Map<NumeroVillaDto>(numeroVilla);
            _response.EstatusCode = HttpStatusCode.OK;
            return Ok(_response);
            
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.EstatusCode = HttpStatusCode.OK;
        }

        return _response;
    }

    [HttpPost]
    public async Task<ActionResult<APIResponse>> CrearNumeroVilla([FromBody] NumeroVillaCreateDto? createDto)
    {
        try
        {
            if (createDto == null)
                return BadRequest(createDto);
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _numeroVillaRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
            {
                ModelState.AddModelError("NumeroExiste","El numero de villa ya existe");
                return BadRequest(ModelState);
            }

            if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea","El Id de la villa no existe");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

            modelo.FechaCreacion = DateTime.Now;
            modelo.FechaActualizacion = DateTime.Now;

            await _numeroVillaRepo.Crear(modelo);
            _response.Resultado = modelo;
            _response.EstatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetNumeroVilla", new { id = modelo.VillaNo }, _response);

        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }
    
}