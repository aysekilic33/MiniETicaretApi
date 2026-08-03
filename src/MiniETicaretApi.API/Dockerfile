# Hata ayıklama kapsayıcınızı özelleştirme ve Visual Studio’nun daha hızlı hata ayıklama için görüntülerinizi derlemek üzere bu Dockerfile'ı nasıl kullandığı hakkında bilgi edinmek için https://aka.ms/customizecontainer sayfasına bakın.

# Bu aşama, VS'den hızlı modda çalıştırıldığında kullanılır (Hata ayıklama yapılandırması için varsayılan olarak ayarlıdır)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# Bu aşama, hizmet projesini oluşturmak için kullanılır
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MiniETicaretApi.API/MiniETicaretApi.API.csproj", "src/MiniETicaretApi.API/"]
COPY ["src/MiniETicaretApi.Application/MiniETicaretApi.Application.csproj", "src/MiniETicaretApi.Application/"]
COPY ["src/MiniETicaretApi.Domain/MiniETicaretApi.Domain.csproj", "src/MiniETicaretApi.Domain/"]
COPY ["src/MiniETicaretApi.Infrastructure/MiniETicaretApi.Infrastructure.csproj", "src/MiniETicaretApi.Infrastructure/"]
RUN dotnet restore "./src/MiniETicaretApi.API/MiniETicaretApi.API.csproj"
COPY . .
WORKDIR "/src/src/MiniETicaretApi.API"
RUN dotnet build "./MiniETicaretApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Bu aşama, son aşamaya kopyalanacak hizmet projesini yayımlamak için kullanılır
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MiniETicaretApi.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Bu aşama üretimde veya VS'den normal modda çalıştırıldığında kullanılır (Hata Ayıklama yapılandırması kullanılmazken varsayılan olarak ayarlıdır)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MiniETicaretApi.API.dll"]