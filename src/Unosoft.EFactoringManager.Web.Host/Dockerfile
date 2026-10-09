FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/Unosoft.EFactoringManager.Web.Host/Unosoft.EFactoringManager.Web.Host.csproj", "src/Unosoft.EFactoringManager.Web.Host/"]
COPY ["src/Unosoft.EFactoringManager.Web.Core/Unosoft.EFactoringManager.Web.Core.csproj", "src/Unosoft.EFactoringManager.Web.Core/"]
COPY ["src/Unosoft.EFactoringManager.Application/Unosoft.EFactoringManager.Application.csproj", "src/Unosoft.EFactoringManager.Application/"]
COPY ["src/Unosoft.EFactoringManager.Core/Unosoft.EFactoringManager.Core.csproj", "src/Unosoft.EFactoringManager.Core/"]
COPY ["src/Unosoft.EFactoringManager.EntityFrameworkCore/Unosoft.EFactoringManager.EntityFrameworkCore.csproj", "src/Unosoft.EFactoringManager.EntityFrameworkCore/"]
WORKDIR "/src/src/Unosoft.EFactoringManager.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/Unosoft.EFactoringManager.Web.Host", "src/Unosoft.EFactoringManager.Web.Host"]
COPY ["src/Unosoft.EFactoringManager.Web.Core", "src/Unosoft.EFactoringManager.Web.Core"]
COPY ["src/Unosoft.EFactoringManager.Application", "src/Unosoft.EFactoringManager.Application"]
COPY ["src/Unosoft.EFactoringManager.Core", "src/Unosoft.EFactoringManager.Core"]
COPY ["src/Unosoft.EFactoringManager.EntityFrameworkCore", "src/Unosoft.EFactoringManager.EntityFrameworkCore"]
WORKDIR "/src/src/Unosoft.EFactoringManager.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "Unosoft.EFactoringManager.Web.Host.dll"]
