using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;

namespace MiniETicaretApi.Application.Interfaces;

public interface IUrunService
{
    Task<Result<List<UrunResponse>>> TumunuGetirAsync(int? kategoriId = null);
    Task<Result<UrunResponse>> GetirAsync(int id);
    Task<Result<UrunResponse>> EkleAsync(UrunRequest request);
    Task<Result<UrunResponse>> GuncelleAsync(int id, UrunRequest request);
    Task<Result> SilAsync(int id);
}