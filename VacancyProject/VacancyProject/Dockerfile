FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["VacancyProject/VacancyProject.csproj", "VacancyProject/"]
RUN dotnet restore "VacancyProject/VacancyProject.csproj"
COPY . .
WORKDIR "/src/VacancyProject"
RUN dotnet build "VacancyProject.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VacancyProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VacancyProject.dll"]
