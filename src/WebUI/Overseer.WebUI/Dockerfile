FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/WebUI/Overseer.WebUI/Overseer.WebUI.csproj", "src/WebUI/Overseer.WebUI/"]
RUN dotnet restore "src/WebUI/Overseer.WebUI/Overseer.WebUI.csproj"
COPY . .
WORKDIR "/src/src/WebUI/Overseer.WebUI"
RUN dotnet build "Overseer.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Overseer.WebUI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Overseer.WebUI.dll"]
