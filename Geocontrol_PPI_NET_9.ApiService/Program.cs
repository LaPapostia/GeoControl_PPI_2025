using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.AddSqlServerClient(connectionName: "sql-connection-string");

builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Geocontrol API",
        Version = "v1",
        Description = "API de gestión de datos para Geocontrol PPI"
    });
});

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    //app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Geocontrol API v1");
        options.RoutePrefix = string.Empty;
    });

    app.MapOpenApi();
}

app.UseStaticFiles();
app.UseRouting();
app.MapDefaultEndpoints();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();