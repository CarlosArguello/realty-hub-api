FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY PropertyHub.sln ./
COPY src/Domain/*.csproj src/Domain/
COPY src/Application/*.csproj src/Application/
COPY src/Infrastructure/*.csproj src/Infrastructure/
COPY src/Api/*.csproj src/Api/

RUN dotnet restore PropertyHub.sln

COPY src ./src

RUN dotnet publish src/Api/Api.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

EXPOSE 5008

COPY --from=build /app/publish ./

COPY run.sh /run.sh
RUN chmod +x /run.sh

CMD dotnet Api.dll --seed; dotnet Api.dll
# ENTRYPOINT ["/run.sh"]
# ENTRYPOINT ["dotnet", "Api.dll"]






