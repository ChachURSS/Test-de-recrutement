using Microsoft.EntityFrameworkCore;
using AngularWithASP.Server.Data;
using Serilog;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

try
{
    Log.Information("Starting web application");

    var builder = WebApplication.CreateBuilder(args);
    var configuration = builder.Configuration;
    var services = builder.Services;

    //Add support to logging with SERILOG
    builder.Host.UseSerilog((context, configuration) =>
        configuration.ReadFrom.Configuration(context.Configuration));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();

    if (builder.Environment.IsDevelopment())
    {
        builder.Services.AddSwaggerGen(
            c=>
            {
                c.EnableAnnotations();
            });

    }

    if (configuration["DB_PROVIDER"] == "SQLite")
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("SqliteConnection")));
    }
    else
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
    }

    var app = builder.Build();

    //Add support to logging request with SERILOG
    app.UseSerilogRequestLogging();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    using (var serviceScope = app.Services.CreateScope())
    {
        var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.Migrate();
    }

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}

