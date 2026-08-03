using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;

namespace MiniETicaretApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UrunController : ControllerBase
{
    private readonly IUrunService _urunService;

    public UrunController(IUrunService urunService)
    {
        _urunService = urunService;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir([FromQuery] int? kategoriId)
    {
        var result = await _urunService.TumunuGetirAsync(kategoriId);
        return Ok(ApiResponse<List<UrunResponse>>.Basarili_(result.Veri!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var result = await _urunService.GetirAsync(id);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<UrunResponse>.Basarili_(result.Veri!));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Ekle(UrunRequest request)
    {
        var result = await _urunService.EkleAsync(request);

        if (!result.Basarili)
            return BadRequest(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<UrunResponse>.Basarili_(result.Veri!));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, UrunRequest request)
    {
        var result = await _urunService.GuncelleAsync(id, request);

        if (!result.Basarili)
            return BadRequest(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<UrunResponse>.Basarili_(result.Veri!));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id)
    {
        var result = await _urunService.SilAsync(id);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<object>.Basarili_(null!, "Ürün silindi."));
    }
}