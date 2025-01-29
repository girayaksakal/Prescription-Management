# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image for building the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MedicineService/MedicineService.csproj", "MedicineService/"]
RUN dotnet restore "MedicineService/MedicineService.csproj"
COPY . .
WORKDIR "/src/MedicineService"
RUN dotnet publish "MedicineService.csproj" -c Release -o /app/publish

# Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MedicineService.dll"]