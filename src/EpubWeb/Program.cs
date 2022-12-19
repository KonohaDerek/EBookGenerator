using Prometheus;
using Serilog;
using Serilog.Enrichers;
using System.Reflection;
using EpubWeb.Entities;
using Microsoft.EntityFrameworkCore;
using EpubWeb.Behaviors;
using FluentValidation;

var EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var configuration = new ConfigurationBuilder()
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{EnvironmentName}.json", optional: true, reloadOnChange: true)
                 .AddEnvironmentVariables()
                 .AddCommandLine(args)
                 .Build();

Log.Logger = new LoggerConfiguration()
.ReadFrom.Configuration(configuration)
.Enrich.With(new ThreadIdEnricher())
.Enrich.WithProperty("API Version", "1.0.1")
.CreateLogger();
try
{
    Log.Information("Starting Serilog demo web host");

    var builder = WebApplication.CreateBuilder(args);
    
     // builder.Services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration.GetConnectionString()));
    builder.Services.AddDbContext<AppDbContext>(options =>
    {
        options.UseInMemoryDatabase("TestMediatR");
    });

    builder.Services.AddControllers();
    builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
    builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
   
     // Pipeline
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
    builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LogBehavior<,>));
      
    // builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MetricsProcessor<,>));
    // builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
    // builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
    // builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(RequestPostProcessorBehavior<,>));

    WebApplication app = builder.Build();
   // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
         // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScheduleTemplate v1"));
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();

    //app.UseAuthorization();

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapMetrics();
        // endpoints.MapHealthChecks("/health");
        endpoints.MapControllers();
        endpoints.MapFallbackToFile("index.html");;
    });
    
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message, "Host terminated unexpectedy!");

}
finally
{
    Log.CloseAndFlush();
}
