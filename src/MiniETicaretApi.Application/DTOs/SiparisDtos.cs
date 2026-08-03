namespace MiniETicaretApi.Application.DTOs;

public record SiparisKalemiRequest(int UrunId, int Adet);

public record SiparisOlusturRequest(List<SiparisKalemiRequest> Kalemler);

public record SiparisKalemiResponse(int UrunId, string UrunAdi, int Adet, decimal BirimFiyat, decimal ToplamFiyat);

public record SiparisResponse(
    int Id,
    DateTime OlusturmaTarihi,
    string Durum,
    decimal ToplamTutar,
    List<SiparisKalemiResponse> Kalemler);