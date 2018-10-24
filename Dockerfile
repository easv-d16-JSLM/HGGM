FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

#Fix from: https://github.com/GitTools/GitVersion/issues/1473
RUN apt-get update && \
        apt-get install -y libgit2-dev && \
        ln -s /usr/lib/x86_64-linux-gnu/libgit2.so /lib/x86_64-linux-gnu/libgit2-15e1193.so

COPY . /app/
RUN ls -lah
RUN cd HGGM; dotnet restore
RUN cd HGGM; dotnet publish -c Release -o out /p:NCrunch=true

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/HGGM/out .
EXPOSE 80
HEALTHCHECK CMD curl -f http://localhost/
ENTRYPOINT ["dotnet", "HGGM.dll"]
