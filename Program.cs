using Microsoft.EntityFrameworkCore;
using smartPark.Data;
using smartPark.Repository;
using smartPark.Repository.Interfaces;
using smartPark.Services;
using smartPark.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// ======= Controllers =======
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ======= DbContext – חיבור MySQL =======
var connectionString = builder.Configuration.GetConnectionString("ParkingContext");

// AddDbContextFactory (ולא AddDbContext) כי כל ה-repositories מבקשים
// IDbContextFactory<ParkingDbContext> בקונסטרוקטור שלהם.
builder.Services.AddDbContextFactory<ParkingDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// ======= Repositories =======
builder.Services.AddScoped<IDriverRepository, DriverRepository>();
builder.Services.AddScoped<IGlobalConfigRepository, GlobalConfigRepository>();
builder.Services.AddScoped<IParkingSpotRepository, ParkingSpotRepository>();
builder.Services.AddScoped<IVehicleEntryRepository, VehicleEntryRepository>();

// ======= Services =======
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<IGlobalConfigService, GlobalConfigService>();
builder.Services.AddScoped<IParkingSpotService, ParkingSpotService>();
builder.Services.AddScoped<IVehicleEntryService, VehicleEntryService>();

// ======= CORS =======
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");

// הוסר UseAuthorization() – אין כרגע AddAuthentication/AddAuthorization מוגדרים
// ואין [Authorize] באף controller, אז הקריאה לא עשתה כלום מלבד להטעות
// שיש הגנה. יש להחזיר אותה כשתיבנה מערכת אימות (JWT וכו'), יחד עם
// app.UseAuthentication() לפניה.

app.MapControllers();
app.Run();


