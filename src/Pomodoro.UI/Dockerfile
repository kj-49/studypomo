#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Pomodoro.UI/Pomodoro.UI.csproj", "Pomodoro.UI/"]
COPY ["Pomodoro.Library/Pomodoro.Library.csproj", "Pomodoro.Library/"]
RUN dotnet restore "./Pomodoro.UI/./Pomodoro.UI.csproj"
COPY . .
WORKDIR "/src/Pomodoro.UI"
RUN dotnet build "./Pomodoro.UI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Pomodoro.UI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pomodoro.UI.dll"]