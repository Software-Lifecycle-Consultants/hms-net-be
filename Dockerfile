#build stage
# Use the official Microsoft .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
# Copy csproj and restore dependencies
COPY *.csproj ./
# Copy everything else and build
COPY . .
RUN dotnet restore
RUN dotnet build -c $BUILD_CONFIGURATION -o /buildOutput

#publish stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish -c $BUILD_CONFIGURATION -o /publishOutput

# Generate runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /publishOutput .
# Command to run the application when the container starts
ENTRYPOINT ["dotnet", "HMS.dll","--environment=Development"]
# Expose port 8080 and 8081 for the application(http/https)
EXPOSE 7041
EXPOSE 5010



