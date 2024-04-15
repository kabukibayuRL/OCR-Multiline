FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj files and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR /src
RUN dotnet build "OCR-Multiline.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OCR-Multiline.csproj" -c Release -o /app/publish

# Start with a new base image
FROM base AS final
WORKDIR /app

# Copy the published application to the final image
COPY --from=publish /app/publish ./

# Install required packages
RUN apt-get -y update \
    && apt-get -y install wget

# Set the entry point for the application
ENTRYPOINT ["dotnet", "OCR-Multiline.dll"]
