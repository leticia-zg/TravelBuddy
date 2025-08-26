using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TravelBuddy.Application.Reservations;
using TravelBuddy.Domain.Repositories;
using TravelBuddy.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TravelBuddy Reservations API", Version = "v1" });
});

builder.Services.AddDbContext<TravelBuddyDbContext>(options =>
    options.UseOracle(builder.Configuration.GetConnectionString("OracleConnection")));

builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<IReservationRepository, EfReservationRepository>();
builder.Services.AddScoped<IDestinationRepository, EfDestinationRepository>();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TravelBuddy Reservations API v1");
    });
}

app.MapControllers();

app.Run();
