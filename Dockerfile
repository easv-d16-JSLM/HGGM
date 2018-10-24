FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

COPY * app/
RUN ls -lah
RUN cd HGGM; dotnet restore
RUN cd HGGM; dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/HGGM/out .
EXPOSE 80
HEALTHCHECK CMD curl -f http://localhost/
ENTRYPOINT ["dotnet", "HGGM.dll"]
