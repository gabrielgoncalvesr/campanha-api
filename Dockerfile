FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7275

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Fidelicard.Campanha/Fidelicard.Campanha.csproj", "Fidelicard.Campanha/"]
COPY ["Fidelicard.Campanha.Core/Fidelicard.Campanha.Core.csproj", "Fidelicard.Campanha.Core/"]
COPY ["Fidelicard.Campanha.Infra/Fidelicard.Campanha.Infra.csproj", "Fidelicard.Campanha.Infra/"]
RUN dotnet restore "Fidelicard.Campanha/Fidelicard.Campanha.csproj"
COPY . .
WORKDIR "/src/Fidelicard.Campanha"
RUN dotnet build "Fidelicard.Campanha.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fidelicard.Campanha.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fidelicard.Campanha.dll"]
