#!/bin/sh
###############################################################
clear
script="== dotnet-build =="
CHAR="#"
LEN=$(printf "%s" "$script" | wc -c)
line=$(printf "%${LEN}s" | tr " " "$CHAR")
printf "\n%s\n%s\n%s\n\n" "$line" "$script" "$line"
set -x
###############################################################
dotnet build --configuration Release
###############################################################
set +x
