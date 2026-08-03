namespace MiniETicaretApi.Application.Common;

public class Result
{
    public bool Basarili { get; }
    public string? HataMesaji { get; }

    protected Result(bool basarili, string? hataMesaji)
    {
        Basarili = basarili;
        HataMesaji = hataMesaji;
    }

    public static Result Basarili_() => new(true, null);
    public static Result Basarisiz(string hataMesaji) => new(false, hataMesaji);
}

public class Result<T> : Result
{
    public T? Veri { get; }

    private Result(bool basarili, T? veri, string? hataMesaji)
        : base(basarili, hataMesaji)
    {
        Veri = veri;
    }

    public static Result<T> Basarili_(T veri) => new(true, veri, null);
    public static new Result<T> Basarisiz(string hataMesaji) => new(false, default, hataMesaji);
}