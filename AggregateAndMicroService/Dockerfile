# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 as base
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ASPNETCORE_URLS=http://+:5271
#EXPOSE 5271



# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy csproj and restore as distinct layers
COPY ["AggregateAndMicroService.csproj", "root/"]

WORKDIR "/src/root"
RUN dotnet restore "AggregateAndMicroService.csproj"
COPY . .
RUN dotnet build "AggregateAndMicroService.csproj" -c Release -o /app/build

FROM build as publish
RUN dotnet publish "AggregateAndMicroService.csproj" -c Release -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish ./
ENTRYPOINT ["dotnet", "AggregateAndMicroService.dll"]

#WORKDIR /
# copy everything else and build app
#COPY ./. ./aspnetapp/
#WORKDIR /source/aspnetapp
#RUN dotnet publish -c release -o /app --no-restore
