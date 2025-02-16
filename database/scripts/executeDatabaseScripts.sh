#!/bin/sh

saPassword=$1
serverAddress="127.0.0.1"

scriptFiles=("Soilution_Logs/Create_Database.sql" "Soilution_Logs/Create_Tables.sql" "Soilution_Logs/Add_User.sql" "Soilution_Data/Create_Database.sql" "Soilution_Data/Create_Tables.sql" "Soilution_Data/Add_User.sql")

echo "Executing database scripts"

for scriptFile in ${scriptFiles[@]}; do
    scriptPath="./scripts/$scriptFile"
    echo "running $scriptPath"
    /opt/mssql-tools18/bin/sqlcmd -S $serverAddress -U sa -P $saPassword -i $scriptPath -C
done