#!/bin/bash

# Run database migration
dotnet ef database update --context HMSDBContext

# Start the application
dotnet HMS.dll --environment=Development
