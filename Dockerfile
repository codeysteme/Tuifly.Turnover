#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Application/TuiFly.Turnover.Application.csproj", "src/Application/"]
COPY ["src/Domain/TuiFly.Turnover.Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/TuiFly.Turnover.Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "src/Application/TuiFly.Turnover.Application.csproj"
COPY . .
WORKDIR "/src/src/Application"
RUN dotnet build "TuiFly.Turnover.Application.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TuiFly.Turnover.Application.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TuiFly.Turnover.Application.dll"]