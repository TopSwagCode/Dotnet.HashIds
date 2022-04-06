FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Dotnet.HashIds/Dotnet.HashIds.csproj", "Dotnet.HashIds/"]
RUN dotnet restore "Dotnet.HashIds/Dotnet.HashIds.csproj"
COPY . .
WORKDIR "/src/Dotnet.HashIds"
RUN dotnet build "Dotnet.HashIds.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Dotnet.HashIds.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dotnet.HashIds.dll"]
