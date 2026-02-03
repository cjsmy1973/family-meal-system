FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY FamilyMeal.Api/*.csproj ./
RUN dotnet restore FamilyMeal.Api.csproj
COPY . ./
RUN dotnet publish FamilyMeal.Api/FamilyMeal.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000
ENTRYPOINT ["dotnet", "FamilyMeal.Api.dll"]
