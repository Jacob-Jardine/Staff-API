#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Staff Service/Staff Service.csproj", "Staff Service/"]
RUN dotnet restore "Staff Service/Staff Service.csproj"
COPY . .
WORKDIR "/src/Staff Service"
RUN dotnet build "Staff Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Staff Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Staff Service.dll"]