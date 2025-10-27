# Imagen base de SQL Server 2022
FROM mcr.microsoft.com/mssql/server:2022-latest

# Variables de entorno requeridas por SQL Server
ENV ACCEPT_EULA=Y
ENV MSSQL_PID=Express
ENV MSSQL_SA_PASSWORD=S0p0rt3SQL

# Crear directorios de datos y backups
RUN mkdir -p /var/opt/mssql/data /var/opt/mssql/backups

# Exponer el puerto SQL Server
EXPOSE 1433

# Healthcheck (igual que en docker-compose)
HEALTHCHECK --interval=5s --timeout=3s --retries=10 --start-period=15s \
  CMD /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P S0p0rt3SQL -C -Q "SELECT 'Healthcheck OK'" || exit 1

# Ejecutar SQL Server
CMD ["/opt/mssql/bin/sqlservr"]
