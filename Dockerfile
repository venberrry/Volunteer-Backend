FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# �������� ���������� � ���������
COPY ./certs/localhost.pfx /https/localhost.pfx

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["MakeVolunteerGreatAgain/MakeVolunteerGreatAgain.csproj", "MakeVolunteerGreatAgain/"]
RUN dotnet restore "MakeVolunteerGreatAgain/MakeVolunteerGreatAgain.csproj"
COPY . .
WORKDIR "/src/MakeVolunteerGreatAgain"
RUN dotnet build "MakeVolunteerGreatAgain.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MakeVolunteerGreatAgain.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENTRYPOINT ["dotnet", "MakeVolunteerGreatAgain.dll", "--urls", "http://0.0.0.0:80;https://0.0.0.0:443"]

