FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["CashRegister.WebApi/CashRegister.WebApi.csproj", "CashRegister.WebApi/"]
RUN dotnet restore "CashRegister.WebApi/CashRegister.WebApi.csproj"
COPY . .
WORKDIR "/src/CashRegister.WebApi"
RUN dotnet build "CashRegister.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CashRegister.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CashRegister.WebApi.dll"]