# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build

WORKDIR /source
RUN mkdir ./identity

WORKDIR /source/identity

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.csproj .

RUN dotnet restore

# copy everything else and build app
COPY . .

WORKDIR /source/identity
RUN dotnet publish -c Release -o /app --no-restore



# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Identity.dll"]