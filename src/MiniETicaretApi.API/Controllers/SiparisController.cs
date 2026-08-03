using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;

namespace MiniETicaretApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SiparisController : ControllerBase
{
    private readonly ISiparisService _siparisService;

    public SiparisController(ISiparisService siparisService)
    {
        _siparisService = siparisService;
    }

    private int KullaniciId => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    private bool AdminMi => User.IsInRole("Admin");

    [HttpPost]
    public async Task<IActionResult> Olustur(SiparisOlusturRequest request)
    {
        var result = await _siparisService.OlusturAsync(KullaniciId, request);

        if (!result.Basarili)
            return BadRequest(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<SiparisResponse>.Basarili_(result.Veri!));
    }

    [HttpGet]
    public async Task<IActionResult> KullanicininSiparisleri()
    {
        var result = await _siparisService.KullanicininSiparisleriniGetirAsync(KullaniciId);
        return Ok(ApiResponse<List<SiparisResponse>>.Basarili_(result.Veri!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var result = await _siparisService.GetirAsync(id, KullaniciId, AdminMi);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<SiparisResponse>.Basarili_(result.Veri!));
    }
}