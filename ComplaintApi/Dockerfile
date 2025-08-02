FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["ComplaintApi/ComplaintApi.csproj", "ComplaintApi/"]
RUN dotnet restore "ComplaintApi/ComplaintApi.csproj"
COPY . .
WORKDIR "/src/ComplaintApi"
RUN dotnet build "ComplaintApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ComplaintApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ComplaintApi.dll"]
