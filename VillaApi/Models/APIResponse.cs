using System.Net;

namespace VillaApi.Models;

public class APIResponse
{
    public APIResponse()
    {
        ErrorMessages = new List<string>();
    }

    public HttpStatusCode EstatusCode { get; set; }
    public bool IsExitoso { get; set; }
    public List<string> ErrorMessages { get; set; }
    public object Resultado { get; set; }
    public int TotalPaginas { get; set; }
}