using TravelBuddy.Infrastructure.Integration;
using TravelBuddy.Infrastructure.Integration.Clients;
using Microsoft.OpenApi.Models; // necessário para SwaggerGen


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrar cliente OpenMeteo
builder.Services.AddHttpClient<OpenMeteoClient>();


var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.Run();
