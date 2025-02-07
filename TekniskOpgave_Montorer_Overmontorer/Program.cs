using TekniskOpgave.DAL.Interfaces;
using TekniskOpgave.DAL.SQL;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Dependency Injection
builder.Services.AddScoped<IMontorerSqlDataAccess, MontorerSqlDataAccess>();
builder.Services.AddScoped<IOvermontorerSqlDataAccess, OvermontorerSqlDataAccess>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "Montorer",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
