FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ["WebApi/WebApi.csproj", "WebApi/"]
RUN dotnet restore "WebApi/WebApi.csproj"

COPY . .

WORKDIR "/src/WebApi"

RUN dotnet publish "WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "WebApi.dll"]