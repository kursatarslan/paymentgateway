FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
# copy csproj and restore as distinct layers
COPY ./*.csproj .
RUN dotnet restore
# Copy everything
COPY . .
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c release -o out --no-cache

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://*:5001
ENV ASPNETCORE_ENVIRONMENT="Production"
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "PaymentGateWayService.dll"]
