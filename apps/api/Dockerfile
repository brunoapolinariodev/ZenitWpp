FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY ["src/ZenitWpp.Api/ZenitWpp.Api.csproj", "src/ZenitWpp.Api/"]
COPY ["src/ZenitWpp.Application/ZenitWpp.Application.csproj", "src/ZenitWpp.Application/"]
COPY ["src/ZenitWpp.Domain/ZenitWpp.Domain.csproj", "src/ZenitWpp.Domain/"]
COPY ["src/ZenitWpp.Infrastructure/ZenitWpp.Infrastructure.csproj", "src/ZenitWpp.Infrastructure/"]
RUN dotnet restore "src/ZenitWpp.Api/ZenitWpp.Api.csproj"
COPY . .
RUN dotnet build "src/ZenitWpp.Api/ZenitWpp.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "src/ZenitWpp.Api/ZenitWpp.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ZenitWpp.Api.dll"]
