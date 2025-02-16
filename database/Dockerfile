FROM mcr.microsoft.com/mssql/server:2022-latest AS base
ARG SA_PASSWORD

WORKDIR /src

COPY . .

RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started" && bash ./scripts/executeDatabaseScripts.sh $SA_PASSWORD