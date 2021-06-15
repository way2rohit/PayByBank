#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Pokedex/Pokedex.csproj", "Pokedex/"]
COPY ["Pokedex.Domain/Pokedex.Domain.csproj", "Pokedex.Domain/"]
COPY ["Pokedex.Tests/Pokedex.Tests.csproj", "Pokedex.Tests/"]
RUN dotnet restore "Pokedex/Pokedex.csproj"
RUN dotnet restore "Pokedex.Domain/Pokedex.Domain.csproj"
RUN dotnet restore "Pokedex.Tests/Pokedex.Tests.csproj"
COPY . .
WORKDIR "/src/Pokedex"
RUN dotnet build "Pokedex.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Pokedex.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokedex.dll"]