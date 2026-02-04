FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY FamilyMeal.Api/*.csproj ./
RUN dotnet restore FamilyMeal.Api.csproj
COPY . ./
RUN dotnet publish FamilyMeal.Api/FamilyMeal.Api.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install MySQL client for database initialization
RUN apt-get update && apt-get install -y default-mysql-client && rm -rf /var/lib/apt/lists/*

# Create uploads directory and copy images
RUN mkdir -p /app/uploads/images
COPY --from=build /app/publish .
COPY uploads/ /app/uploads/

# Copy SQL init script
COPY init-data.sql /app/init-data.sql

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

# Create entrypoint script
RUN echo '#!/bin/bash\n\
echo "Initializing database..."\n\
if [ ! -f /app/.data_initialized ]; then\n\
    # Wait for MySQL to be ready\n\
    max_retries=30\n\
    retry=0\n\
    while [ $retry -lt $max_retries ]; do\n\
        if mysqladmin ping -h "$MYSQL_HOST" -u root --password="$MYSQL_PASSWORD" --silent 2>/dev/null; then\n\
            echo "MySQL is ready!"\n\
            break\n\
        fi\n\
        echo "Waiting for MySQL... ($((retry+1))/$max_retries)"\n\
        sleep 2\n\
        retry=$((retry+1))\n\
    done\n\
    # Import initial data\n\
    if [ -f /app/init-data.sql ]; then\n\
        echo "Importing initial data..."\n\
        mysql -h "$MYSQL_HOST" -u root --password="$MYSQL_PASSWORD" "$MYSQL_DATABASE" < /app/init-data.sql 2>/dev/null || true\n\
        touch /app/.data_initialized\n\
        echo "Database initialized!"\n\
    fi\n\
fi\n\
exec dotnet FamilyMeal.Api.dll' > /app/entrypoint.sh && chmod +x /app/entrypoint.sh

ENTRYPOINT ["/app/entrypoint.sh"]
