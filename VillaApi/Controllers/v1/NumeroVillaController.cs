using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer numero villa con Id " + id);
                _response.EstatusCode = HttpStatusCode.BadRequest;
                _response.IsExitoso = false;
                return BadRequest(_response);
            }

            var numeroVilla = await _numeroVillaRepo.Obtener(x => x.VillaNo == id, incluirPropiedades: "Villa");

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
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
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
                ModelState.AddModelError("NumeroExiste", "El numero de villa ya existe");
                return BadRequest(ModelState);
            }

            if (await _villaRepo.Obtener(v => v.Id == createDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
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

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateNumeroVilla([FromBody] NumeroVillaUpdateDto updateDto, int id)
    {
        try
        {
            if (updateDto == null || id != updateDto.VillaNo)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Obtener(v => v.Id == updateDto.VillaId) == null)
            {
                ModelState.AddModelError("ClaveForanea", "El Id de la villa no existe");
                return BadRequest(ModelState);
            }

            NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

            await _numeroVillaRepo.Actualizar(modelo);
            _response.EstatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return BadRequest(_response);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteNumeroVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var numeroVilla = await _numeroVillaRepo.Obtener(x => x.VillaNo == id);

            if (numeroVilla == null)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _numeroVillaRepo.Remove(numeroVilla);

            _response.EstatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return BadRequest(_response);
    }
}