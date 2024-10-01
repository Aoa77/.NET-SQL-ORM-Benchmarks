#!/bin/sh
######################################################
clear
script="== dotnet-run =="
CHAR="#"
LEN=$(printf "%s" "$script" | wc -c)
line=$(printf "%${LEN}s" | tr " " "$CHAR")
printf "\n%s\n%s\n%s\n\n" "$line" "$script" "$line"
set -x
##############################################################################
dotnet run --project "ormb.runner/ormb.runner.csproj" --configuration Release
##############################################################################
set +x
