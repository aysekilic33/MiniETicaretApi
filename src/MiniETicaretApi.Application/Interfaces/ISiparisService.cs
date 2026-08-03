using MiniETicaretApi.Application.Common;
using MiniETicaretApi.Application.DTOs;

namespace MiniETicaretApi.Application.Interfaces;

public interface ISiparisService
{
    Task<Result<SiparisResponse>> OlusturAsync(int kullaniciId, SiparisOlusturRequest request);
    Task<Result<List<SiparisResponse>>> KullanicininSiparisleriniGetirAsync(int kullaniciId);
    Task<Result<SiparisResponse>> GetirAsync(int id, int kullaniciId, bool adminMi);
}