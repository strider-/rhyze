#! /usr/bin/env bash

# Turning off history expansion, as it might screw
# with the password.
set +H

DB_NAME=Rhyze
SCRIPT="IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'$DB_NAME') BEGIN CREATE DATABASE [$DB_NAME] END;"

# Connecting to the running container & use sqlcmd
# to run the create database command.
docker container exec -it rhyze_sql /opt/mssql-tools/bin/sqlcmd \
	-S localhost \
	-U sa \
	-P "B43tn4t!0n" \
	-q "EXIT($SCRIPT)"

[ $? -eq 0 ] && echo "Database '$DB_NAME' has been created." || echo "Database creation failed!"
