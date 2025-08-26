using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using TravelBuddy.Infrastructure.Integration.Clients;
using Microsoft.OpenApi.Models;
using TravelBuddy.Infrastructure.Persistence;
using TravelBuddy.Application.Destinations;
using TravelBuddy.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddScoped<IDestinationService, DestinationService>();
builder.Services.AddScoped<IDestinationRepository, EfDestinationRepository>();
builder.Services.AddScoped<IOpenMeteoClient, OpenMeteoClient>();

// Configurar DbContext com Oracle
builder.Services.AddDbContext<TravelBuddyDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

// Adicionar controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelBuddy Destinations API", Version = "v1" });
});

// Configurar políticas de resiliência
var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(
    TimeSpan.FromSeconds(10),
    TimeoutStrategy.Optimistic
);

var retryPolicy = HttpPolicyExtensions
    .HandleTransientHttpError()
    .Or<TimeoutRejectedException>()
    .WaitAndRetryAsync(
        retryCount: 3,
        sleepDurationProvider: attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt)), // exponencial
        onRetry: (outcome, timespan, retryAttempt, context) =>
        {
            var logger = builder.Services.BuildServiceProvider()
                             .GetRequiredService<ILogger<Program>>();
            logger.LogWarning(
                "Retry {retryAttempt} after {delay} devido a {reason}",
                retryAttempt,
                timespan,
                outcome.Exception?.Message ?? outcome.Result?.StatusCode.ToString()
            );
        }
    );

// Registrar cliente Http com Polly
builder.Services.AddHttpClient<IOpenMeteoClient, OpenMeteoClient>()
    .AddPolicyHandler(timeoutPolicy) // Timeout primeiro
    .AddPolicyHandler(retryPolicy);  // Depois retry

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelBuddy Destinations API v1");
    });
}

app.MapControllers();
app.Run();
