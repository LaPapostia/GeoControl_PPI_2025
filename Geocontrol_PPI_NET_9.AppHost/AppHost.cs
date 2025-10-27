try
{
    var builder = DistributedApplication.CreateBuilder(args);
    // $"{ReferenceExpression.Create($"Endpoint=http://sql-pass;key={sqlPassword}")}"

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

    var sqlPasswordParam = builder.AddParameter("sql-pass", secret: true);
    var sqlConnectionParam = builder.AddParameter("sql-connection", secret: true);

    var sql = builder.AddConnectionString("sql-connection-string");

    var sqldDacpacProject = builder.AddSqlProject("sql-geocontrol-project")
       .WithDacpac("../Geocontrol_PPI_NET_9.Resources/GeoControl.dacpac")
       .WithReference(sql);

    //var sqlContainer = builder.AddContainer("geocontrol-sql", "mcr.microsoft.com/mssql/server:2022-latest")
    //    .WithEnvironment("ACCEPT_EULA", "Y")
    //    .WithEnvironment("MSSQL_SA_PASSWORD", "S0p0rt3SQL")
    //    .WithEnvironment("MSSQL_PID", "Express")
    //    .WithBindMount("./bd_data", "/var/opt/mssql/data")
    //    .WithVolume("geocontrol_sql_data", "/var/opt/mssql")
    //    .WithEndpoint(1433, 1433)
    //    .WithLifetime(ContainerLifetime.Persistent);
    #endregion

    var cache = builder.AddRedis("cache")
        .WaitFor(sql);

    var apiService = builder.AddProject<Projects.Geocontrol_PPI_NET_9_ApiService>("apiservice")
        .WithHttpHealthCheck("/health")
        .WithReference(sql)
        .WaitFor(cache)
        .WaitFor(sql);

    builder.AddProject<Projects.Geocontrol_PPI_NET_9_Web>("webfrontend")
        .WithExternalHttpEndpoints()
        .WithHttpHealthCheck("/health")
        .WithReference(cache)
        .WaitFor(cache)
        .WithReference(apiService)
        .WaitFor(apiService);

    builder.Build().Run();
}
catch (Exception ex)
{
    // Handle configuration errors
    Console.WriteLine($"Database configuration failed: {ex.Message}");
}
