using Microsoft.AspNetCore.Mvc;
using VillaApi.Models;
using VillaApi.Repositorio.IRepositorio;

namespace VillaApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class VillaController : ControllerBase
{
    private readonly IVillaRepositorio _villaRepo;

    public VillaController(IVillaRepositorio villaRepo)
    {
        _villaRepo = villaRepo;
    }


    [HttpGet]
    public async Task<List<Villa>> GetVillas()
    {
        var cinco = await  _villaRepo.ObtenerTodos();
        return cinco;
    }
}