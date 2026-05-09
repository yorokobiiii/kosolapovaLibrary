# ===== СТАДИЯ 1: BUILD (компиляция приложения) =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["LibraryOnline.csproj", "."]
RUN dotnet restore "./LibraryOnline.csproj"

# Копируем весь исходный код
COPY . .

# Собираем приложение
RUN dotnet build "./LibraryOnline.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Публикуем приложение
FROM build AS publish
RUN dotnet publish "./LibraryOnline.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# ===== СТАДИЯ 2: RUNTIME (запуск приложения) =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Создаём пользователя без прав root для безопасности
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

# Копируем опубликованное приложение
COPY --from=publish /app/publish .

# Настраиваем порт
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "LibraryOnline.dll"]