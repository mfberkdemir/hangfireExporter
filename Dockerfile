FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY hangfireExporter/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime AS runtime
WORKDIR /app
COPY --from=build /app/hangfireExporter/out .
ENTRYPOINT ["dotnet", "hangfireExporter.dll"]
ENV dataProvider=${dataProvider}
ENV connectionString=${connectionString}
ENV dbName=${dbName}