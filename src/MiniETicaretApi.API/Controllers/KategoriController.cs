using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;
using MiniETicaretApi.Application.Interfaces;

namespace MiniETicaretApi.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KategoriController : ControllerBase
{
    private readonly IKategoriService _kategoriService;

    public KategoriController(IKategoriService kategoriService)
    {
        _kategoriService = kategoriService;
    }

    [HttpGet]
    public async Task<IActionResult> TumunuGetir()
    {
        var result = await _kategoriService.TumunuGetirAsync();
        return Ok(ApiResponse<List<KategoriResponse>>.Basarili_(result.Veri!));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Getir(int id)
    {
        var result = await _kategoriService.GetirAsync(id);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<KategoriResponse>.Basarili_(result.Veri!));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Ekle(KategoriRequest request)
    {
        var result = await _kategoriService.EkleAsync(request);
        return Ok(ApiResponse<KategoriResponse>.Basarili_(result.Veri!));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Guncelle(int id, KategoriRequest request)
    {
        var result = await _kategoriService.GuncelleAsync(id, request);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<KategoriResponse>.Basarili_(result.Veri!));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Sil(int id)
    {
        var result = await _kategoriService.SilAsync(id);

        if (!result.Basarili)
            return NotFound(ApiResponse<object>.Basarisiz(result.HataMesaji!));

        return Ok(ApiResponse<object>.Basarili_(null!, "Kategori silindi."));
    }
}