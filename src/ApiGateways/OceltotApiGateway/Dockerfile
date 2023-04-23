#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateways/OceltotApiGateway/OceltotApiGateway.csproj", "ApiGateways/OceltotApiGateway/"]
RUN dotnet restore "ApiGateways/OceltotApiGateway/OceltotApiGateway.csproj"
COPY . .
WORKDIR "/src/ApiGateways/OceltotApiGateway"
RUN dotnet build "OceltotApiGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OceltotApiGateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OceltotApiGateway.dll"]