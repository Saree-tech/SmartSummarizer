# Build stage
FROM mcr.microsoft.com / dotnet / sdk:7.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Runtime stage
FROM mcr.microsoft.com/dotnet/runtime:7.0
WORKDIR / app
COPY--from = build / app /out .

# Note: Windows Forms requires Windows base image for GUI
# For console/API version, use this runtime image
ENTRYPOINT["dotnet", "SmartSummarizer.dll"]