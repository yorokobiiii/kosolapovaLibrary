# ===== СТАДИЯ 1: BUILD (компиляция приложения) =====
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Копируем файл проекта и восстанавливаем зависимости
COPY ["LibraryOnline.csproj", "."]
RUN dotnet restore

# Копируем весь исходный код
COPY . .

# Публикуем приложение
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ===== СТАДИЯ 2: RUNTIME (запуск приложения) =====
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

# Копируем опубликованное приложение из стадии build
COPY --from=build /app/publish .

# Настраиваем порт
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Точка входа
ENTRYPOINT ["dotnet", "LibraryOnline.dll"]