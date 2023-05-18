FROM mcr.microsoft.com/dotnet/sdk:6.0  AS build
WORKDIR /app
COPY ./ ./
RUN dotnet publish -c Release -o published

FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app
RUN apt-get update && apt-get install -y wget

# RUN GRPC_HEALTH_PROBE_VERSION=v0.4.18 && \
#     wget -qO/bin/grpc_health_probe https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 && \
#     chmod +x /bin/grpc_health_probe

COPY --from=build /app/published/ ./
ENTRYPOINT ["dotnet", "GrpcGreeter.dll"]