using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;

namespace MiniETicaretApi.Application.Interfaces;

public interface IKategoriService
{
    Task<Result<List<KategoriResponse>>> TumunuGetirAsync();
    Task<Result<KategoriResponse>> GetirAsync(int id);
    Task<Result<KategoriResponse>> EkleAsync(KategoriRequest request);
    Task<Result<KategoriResponse>> GuncelleAsync(int id, KategoriRequest request);
    Task<Result> SilAsync(int id);
}