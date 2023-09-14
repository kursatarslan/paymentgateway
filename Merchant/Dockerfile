FROM mcr.microsoft.com/dotnet/sdk:5.0.103 AS build
WORKDIR /build

RUN curl -sL https://deb.nodesource.com/setup_14.x |  bash -
RUN apt-get install -y nodejs

# copy csproj and restore as distinct layers
COPY ./*.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /build
RUN dotnet publish -c release -o published --no-cache
COPY ./cert .

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
ENV ASPNETCORE_URLS=http://*:5008
ENV ASPNETCORE_ENVIRONMENT="Production"
WORKDIR /app
COPY --from=build /build/published ./
ENTRYPOINT ["dotnet", "Merchant.dll"]
