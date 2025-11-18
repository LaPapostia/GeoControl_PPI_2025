try
{
    /// Create the builder for the application
    var builder = DistributedApplication.CreateBuilder(args);
    
    /// Define the docker behaviour component
    builder.AddDockerComposeEnvironment("env")
               .WithDashboard(dashboard =>
               {
                   dashboard.WithHostPort(8080)
                            .WithForwardedHeaders(enabled: true);
               });

    #region Database Configuration
    //var sql = builder.AddDockerfile("geocontrol-sql-docker", "../Geocontrol_PPI_NET_9.Resources", "../Geocontrol_PPI_NET_9.Resources/Dockerfile")
    //    .WithBindMount("./bd_data", "/var/opt/mssql/data")
    //    .WithVolume("geocontrol_sql_data", "/var/opt/mssql")
    //    //.WithArgs(["-p 1433:1433", "-e MSSQL_SA_PASSWORD=S0p0rt3SQL"])
    //    .WithLifetime(ContainerLifetime.Persistent);

    /// Assign the values secrets params
    var sqlUserParam = builder.AddParameter("sql-user", secret: true);
    var sqlPasswordParam = builder.AddParameter("sql-pass", secret: true);
    var sqlConnectionParam = builder.AddParameter("sql-connection", secret: true);
    var sqlDatabaseParam = builder.AddParameter("sql-database", secret: true);

    /// Assign the value of the connection string in base of the secret value
    var sql = builder.AddConnectionString("sql-connection-string", 
        ReferenceExpression.Create($"Server={sqlConnectionParam};User Id={sqlUserParam};Password={sqlPasswordParam};TrustServerCertificate=True;Initial Catalog={sqlDatabaseParam};"));
    // $"{ReferenceExpression.Create($"Endpoint=http://sql-pass;key={sqlPassword}")}"

    //var sqldDacpacProject = builder.AddSqlProject("sql-geocontrol-project")
    //   .WithDacpac("../Geocontrol_PPI_NET_9.Resources/GeoControl.dacpac")
    //   .WithReference(sql);
    #endregion

    /// Assign the cache project manager
    var cache = builder.AddRedis("cache")
        .WaitFor(sql);

    /// Assign the API service project
    var apiService = builder.AddProject<Projects.Geocontrol_PPI_NET_9_ApiService>("apiservice")
        .WithHttpPort
        .WithHttpHealthCheck("/health")
        .WithReference(sql)
        .WaitFor(cache)
        .WaitFor(sql);

    /// Assign the web front-end project
    builder.AddProject<Projects.Geocontrol_PPI_NET_9_Web>("webfrontend")
        .WithHttpHealthCheck("/health")
        .WithReference(cache)
        .WaitFor(cache)
        .WithReference(apiService)
        .WaitFor(apiService);

    /// Build and run the application
    builder.Build().Run();
}
catch (Exception ex)
{
    /// Handle configuration errors
    Console.WriteLine($"Database configuration failed: {ex.Message}");
}
