using Contatos.Infra.Services;
using Dapper;
using Npgsql;
using Regioes.Api.Middlewares;
using Regioes.Core.Models;
using Regioes.Infra.Consumers;
using Regioes.Infra.Repositories;
using Regioes.Infra.Repositories.Interfaces;
using Regioes.Infra.Services;
using Regioes.Infra.Services.Interfaces;
using System.Data;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

        var connectionString = configuration.GetValue<string>("ConnectionStringPostgres");

        var builder = WebApplication.CreateBuilder(args);
     
        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddSingleton<IServices<Regiao>, RegiaoServices>();
        builder.Services.AddSingleton<IRepository<Regiao>, RegiaoRepository>();
        builder.Services.AddExceptionHandler<ExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddMemoryCache();
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddApplicationInsightsTelemetry();
        builder.Services.AddHostedService<InclusaoConsumer>();
        builder.Services.AddHostedService<UpdateConsumer>();
        builder.Services.AddHostedService<DeleteConsumer>();

        builder.Services.AddHealthChecks();


        DefaultTypeMap.MatchNamesWithUnderscores = true;

        builder.Services.AddSingleton<IDbConnection>((connection) => new NpgsqlConnection(connectionString));
                
        var app = builder.Build();
         
        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{
        app.UseSwagger();
        app.UseSwaggerUI();
        //}

        app.MapHealthChecks("/health");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler();

        app.Run();
    }

    //public static IHostBuilder CreateHostBuilder(string[] args) =>
    //        Host.CreateDefaultBuilder(args)
    //            .ConfigureServices((hostContext, services) =>
    //            {
    //                services.AddHostedService<InclusaoConsumer>();
    //            });
        
}

