FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app
COPY ./ ./
RUN dotnet publish -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY --from=build /app/published/ ./
ENTRYPOINT ["dotnet", "GrpcGreeter.dll"]