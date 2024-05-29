#!/bin/bash
# Wait for the MySQL server to be available
echo "Waiting for MySQL..."
while ! nc -z mysql 3306; do   
  sleep 1
done
echo "MySQL started"

# Run EF Core database update
dotnet ef database update --context HMSDBContext

# Start the application
dotnet HMS.dll --environment=Development