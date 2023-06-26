using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Models.Dto;
using VillaApi.Models.Especificaciones;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers.v1;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class VillaController : ControllerBase
{
    private readonly ILogger<VillaController> _logger;
    private readonly IVillaRepositorio _villaRepo;
    private readonly IMapper _mapper;
    protected APIResponse _response;

    public VillaController(ILogger<VillaController> logger, IVillaRepositorio villaRepo, IMapper mapper)
    {
        _logger = logger;
        _villaRepo = villaRepo;
        _mapper = mapper;
        _response = new();
    }


    [HttpGet]
    [ResponseCache(CacheProfileName = "Default30")]
    [ProducesResponseType(StatusCodes.Status200OK)]
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

    [HttpGet("VillasPaginado")]
    [ResponseCache(CacheProfileName = "Default30")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<APIResponse> GetVillasPaginado([FromQuery] Parametros parametros)
    {
        try
        {
            var villaList = _villaRepo.ObtenerTodosPaginado(parametros);
            _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villaList);
            _response.EstatusCode = HttpStatusCode.OK;
            _response.TotalPaginas = villaList.MetaData.TotalPages;

            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpGet("{id:int}", Name = "GetVilla")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> GetVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer la villa con Id " + id);
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaRepo.Obtener(x => x.Id == id);

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

    [HttpPost]
    [Authorize(Roles = "master", AuthenticationSchemes = "Bearer")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<APIResponse>> CrearVilla([FromBody] VillaCreateDto? createDto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (createDto == null)
                return BadRequest(createDto);

            if (await _villaRepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
            {
                ModelState.AddModelError("NombreExiste", "La villa con ese nombre ya existe");
                return BadRequest(ModelState);
            }

            Villa modelo = _mapper.Map<Villa>(createDto);

            modelo.FechaCreacion = DateTime.Now;
            modelo.FechaActualizacion = DateTime.Now;

            await _villaRepo.Crear(modelo);
            _response.Resultado = modelo;
            _response.EstatusCode = HttpStatusCode.Created;

            return CreatedAtRoute("GetVilla", new { id = modelo.Id }, _response);
        }
        catch (Exception ex)
        {
            _response.IsExitoso = false;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return _response;
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateVilla([FromBody] VillaUpdateDto updateDto, int id)
    {
        try
        {
            if (updateDto == null || id != updateDto.Id)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            if (await _villaRepo.Obtener(v => v.Id == id) == null)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            Villa modelo = _mapper.Map<Villa>(updateDto);


            await _villaRepo.Actualizar(modelo);
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
    [Authorize(Roles = "admin")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteVilla(int id)
    {
        try
        {
            if (id == 0)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

            var villa = await _villaRepo.Obtener(x => x.Id == id);

            if (villa == null)
            {
                _response.IsExitoso = false;
                _response.EstatusCode = HttpStatusCode.NotFound;
                return NotFound(_response);
            }

            await _villaRepo.Remove(villa);

            _response.EstatusCode = HttpStatusCode.NoContent;
            return Ok(_response);
        }
        catch (Exception ex)
        {
            _response.EstatusCode = HttpStatusCode.NoContent;
            _response.ErrorMessages = new List<string>() { ex.ToString() };
        }

        return BadRequest(_response);
    }

    [HttpPatch("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdatePartialVilla(JsonPatchDocument<VillaUpdateDto> patchDto, int id)
    {
        if (patchDto == null || id == 0)
            return BadRequest();

        var villa = await _villaRepo.Obtener(v => v.Id == id, tracked: false);

        VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

        if (villa == null)
            return BadRequest();

        patchDto.ApplyTo(villaDto, ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        Villa modelo = _mapper.Map<Villa>(villaDto);

        await _villaRepo.Actualizar(modelo);
        _response.EstatusCode = HttpStatusCode.NoContent;

        return Ok(_response);
    }
}