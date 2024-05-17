#!/bin/bash

# Wait for the database to be available
wait_for_db() {
    local host="$1"
    local port="$2"
    local timeout=30
    local start_time=$(date +%s)
    local end_time=$((start_time + timeout))

    # Loop until timeout is reached
    while true; do
        # Check if port is open
        nc -z "$host" "$port" >/dev/null 2>&1
        result=$?
        if [ $result -eq 0 ]; then
            echo "Database is available"
            break
        fi

        # Check if timeout is reached
        local current_time=$(date +%s)
        if [ $current_time -gt $end_time ]; then
            echo "Timeout reached. Database is still unavailable."
            exit 1
        fi

        # Sleep for 1 second before checking again
        sleep 1
    done
}

# Run migrations if database is accessible
if [ -n "$DB_HOST" ] && [ -n "$DB_PORT" ]; then
    wait_for_db "$DB_HOST" "$DB_PORT"
    echo "Running database migrations..."
    dotnet ef database update --context HMSDBContext
else
    echo "No database information provided. Skipping database migrations."
fi

# Start the application
echo "Starting the application..."
exec "$@"
