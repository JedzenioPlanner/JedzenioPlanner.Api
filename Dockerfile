FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build-env
WORKDIR /app

COPY . .
RUN rm .git -rf
RUN rm .github -rf
RUN rm build -rf
RUN rm output -rf
RUN rm .gitignore
RUN rm .nuke
RUN rm build.cmd
RUN rm build.ps1
RUN rm build.sh
RUN rm Dockerfile
RUN rm README.md
RUN rm LICENSE.txt
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build-env /app/out .
ENV ASPNETCORE_ENVIRONMENT=docker
ENTRYPOINT ["dotnet", "JedzenioPlanner.Api.dll"]