FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY ./WebApplication1/ ./
RUN ls -la
COPY ./Lib ../Lib
RUN dotnet publish -c Release -o out
RUN ls -la

FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["/usr/bin/dotnet", "WebApplication1.dll"]
