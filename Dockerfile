#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PolyglotAPI.csproj", "."]
RUN dotnet restore "./PolyglotAPI.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./PolyglotAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

#RUN chmod +x wait-for-db.sh 
#RUN ./wait-for-db.sh db:5432 -t 60
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"
#RUN dotnet ef database update --project "./PolyglotAPI.csproj" --startup-project "./PolyglotAPI.csproj" --context PolyglotAPI.Data.ApplicationDbContext


FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PolyglotAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false



FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .


ENTRYPOINT ["dotnet", "PolyglotAPI.dll"]
#ENTRYPOINT ["sh", "-c", "dotnet ef database update && dotnet PolyglotAPI.dll"]