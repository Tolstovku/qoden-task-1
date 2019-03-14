FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY *.csproj ./
COPY NuGet.Config ./
RUN dotnet restore --configfile NuGet.Config

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["/usr/bin/dotnet", "WebApplication1.dll"]
