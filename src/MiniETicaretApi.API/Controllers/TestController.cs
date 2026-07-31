using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MiniETicaretApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet("gizli")]
    [Authorize]
    public IActionResult GizliVeri()
    {
        var kullaniciEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        return Ok(new { mesaj = "Bu veriyi sadece giriş yapmış kullanıcılar görebilir!", email = kullaniciEmail });
    }
}