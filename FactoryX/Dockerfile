FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /factoryXroot

# copy csproj file and restore
COPY ./*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./* ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2
WORKDIR /factoryXroot
COPY --from=build /factoryXrootasdasdasd/out C:
ENTRYPOINT ["dotnet", "FactoryX.dll"]