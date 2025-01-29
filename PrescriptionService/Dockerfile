# Use the official .NET runtime as the base image
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the .NET SDK image for building the application
FROM  mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["PrescriptionService/PrescriptionService.csproj", "PrescriptionService/"]
RUN dotnet restore "PrescriptionService/PrescriptionService.csproj"
COPY . .
WORKDIR "/src/PrescriptionService"
RUN dotnet publish "PrescriptionService.csproj" -c Release -o /app/publish

# Use the runtime image to run the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "PrescriptionService.dll"]