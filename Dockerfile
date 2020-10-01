FROM mcr.microsoft.com/dotnet/core/sdk:3.1
WORKDIR /src
COPY . .
RUN dotnet restore

RUN dotnet build "src/Benchmark.csproj" -c Release -o /src/bin
RUN dotnet publish "src/Benchmark.csproj" -c Release -o /src/bin/publish

WORKDIR /src/bin/publish
ENTRYPOINT ["dotnet", "Benchmark.dll"]