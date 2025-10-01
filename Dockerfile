# Estágio de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/Fcg.User.Service.Api/Fcg.User.Service.Api.csproj", "src/Fcg.User.Service.Api/"]
COPY ["src/Fcg.User.Service.Application/Fcg.User.Service.Application.csproj", "src/Fcg.User.Service.Application/"]
COPY ["src/Fcg.User.Service.Domain/Fcg.User.Service.Domain.csproj", "src/Fcg.User.Service.Domain/"]
COPY ["src/Fcg.User.Service.Infra/Fcg.User.Service.Infra.csproj", "src/Fcg.User.Service.Infra/"]

# Restaurar dependências
RUN dotnet restore "src/Fcg.User.Service.Api/Fcg.User.Service.Api.csproj"

# Copiar tudo
COPY . .

# Build
WORKDIR "/src/src/Fcg.User.Service.Api"
RUN dotnet build "Fcg.User.Service.Api.csproj" -c Release -o /app/build

# Estágio de publicação
FROM build AS publish
RUN dotnet publish "Fcg.User.Service.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Estágio final
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fcg.User.Service.Api.dll"]