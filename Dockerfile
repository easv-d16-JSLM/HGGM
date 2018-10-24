FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY * ./
WORKDIR /app/HGGM
RUN ls -lah
RUN dotnet restore
RUN ls -lah
RUN dotnet publish -c Release -o out
RUN ls -lah
# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/HGGM/out .
EXPOSE 80
HEALTHCHECK CMD curl -f http://localhost/
RUN ls -lah
ENTRYPOINT ["dotnet", "HGGM.dll"]
