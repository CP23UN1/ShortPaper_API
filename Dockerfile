# Use an image with .NET 7.0 SDK
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env

# Set the working directory in the container
WORKDIR /app

# Copy the project file and the rest of the application code
COPY ./ShortPaper_API/ ./

# Restore dependencies and build the application
RUN dotnet restore && \
    dotnet publish -c Release -o out && \
    rm -rf obj/ && \
    rm -rf bin/

# Create a runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build-env /app/out .

# Expose the port the app will run on
EXPOSE 3000

# Define the entry point for the application
ENTRYPOINT ["dotnet", "ShortPaper_API.dll"]
