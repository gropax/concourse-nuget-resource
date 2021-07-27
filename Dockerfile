FROM mcr.microsoft.com/dotnet/runtime:5.0-alpine AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Check/Check.csproj", "Check/"]
COPY ["Resource/Resource.csproj", "Resource/"]
RUN dotnet restore "Check/Check.csproj" -r linux-musl-x64 
COPY . .
WORKDIR "/src/Check"
RUN dotnet build "Check.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Check.csproj" -c Release -o /app/publish -r linux-musl-x64

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN mkdir /opt/resource && \
	ln -s /app/Check /opt/resource/check
RUN /bin/sh