using MiniETicaretApi.Domain;

namespace MiniETicaretApi.Application.Interfaces;

public interface ITokenService
{
    string TokenUret(Kullanici kullanici);
}