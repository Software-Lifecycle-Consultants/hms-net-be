#!/bin/bash

# Wait for the MySQL server to be up and running
until /opt/mssql-tools/bin/sqlcmd -S mysql -U root -P 'P@ssw0rd' -Q 'SELECT 1' > /dev/null 2>&1; do
    echo "Waiting for MySQL server to start..."
    sleep 5
done

echo "Applying database migrations..."
dotnet ef database update



