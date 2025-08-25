var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// HttpClients para APIs
builder.Services.AddHttpClient("reservations", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiEndpoints:Reservations"]!);
});
builder.Services.AddHttpClient("destinations", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiEndpoints:Destinations"]!);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
