namespace MiniETicaretApi.Application.Common;

public class ApiResponse<T>
{
    public bool Basarili { get; set; }
    public T? Veri { get; set; }
    public string? Mesaj { get; set; }

    public static ApiResponse<T> Basarili_(T veri, string? mesaj = null) =>
        new() { Basarili = true, Veri = veri, Mesaj = mesaj };

    public static ApiResponse<T> Basarisiz(string mesaj) =>
        new() { Basarili = false, Veri = default, Mesaj = mesaj };
}