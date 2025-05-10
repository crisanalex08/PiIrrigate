using PiIrrigateServer.Database;
using PiIrrigateServer.Managers;
using PiIrrigateServer.Mock;
using PiIrrigateServer.Models;
using PiIrrigateServer.Repositories;
using PiIrrigateServer.Services;
using PiIrrigateServer.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.  
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", builder =>
    {
        builder.WithOrigins("http://localhost:4200") // Allow Angular app
               .AllowAnyHeader()
               .AllowAnyMethod()
                .AllowCredentials();
    });
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle  
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<IZoneRepository, ZoneRepository>();
builder.Services.AddScoped<IiotDeviceManager, IotDeviceManager>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddSingleton<DataSenderMock>();
builder.Services.AddSingleton<C2DMessageSenderManager>();

builder.Services.Configure<IoTHubConfiguraiton>(builder.Configuration.GetSection("IotHubConfiguration"));
builder.Services.AddHostedService<IoTHubDataManager>();
var app = builder.Build();

// Configure the HTTP request pipeline.  
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting(); // Add this line to configure routing middleware  
app.UseCors("AllowAngularApp"); // Apply the CORS policy

app.UseAuthorization();
app.UseEndpoints(endpoints =>
{
    _ = endpoints.MapHub<LiveDataHub>("/liveDataHub");
});

app.MapControllers();

//// Resolve the DataSenderMock service
//var dataSenderMock = app.Services.GetRequiredService<DataSenderMock>();

//// Start sending mock data in a background task
//var cancellationTokenSource = new CancellationTokenSource();
//_ = Task.Run(() => dataSenderMock.StartSendingMockData(cancellationTokenSource.Token));


app.Run();
