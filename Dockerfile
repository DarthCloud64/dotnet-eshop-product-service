# Step 1: Build using the SDK image
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish -o out

# Step 2 final build using the aspnet runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT [ "dotnet", "dotnet-eshop-product-service-webapi.dll" ]
EXPOSE 80