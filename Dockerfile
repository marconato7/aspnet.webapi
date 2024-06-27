FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /aspnetwebapi

COPY [ "./aspnet.webapi.csproj", "./" ]
RUN dotnet restore "./aspnet.webapi.csproj"

COPY . .
RUN dotnet build "./aspnet.webapi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./aspnet.webapi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
ENV ASPNETCORE_HTTP_PORTS=5001
EXPOSE 5001
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT [ "dotnet", "aspnet.webapi.dll" ]
