using Serilog;
using TaskNowSoftware.Automapper;
using TaskNowSoftware.DbContext;
using TaskNowSoftware.Extensions;
using TaskNowSoftware.IRepository;
using TaskNowSoftware.Middlewares;
using TaskNowSoftware.Repository;
using TaskNowSoftware.Services;

var builder = WebApplication.CreateBuilder(args);

// Adding the Serilogs
var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//JWT service configurations using extension method
builder.Services.ConfigureJwt(builder.Configuration);

//registering autommaper here
builder.Services.AddAutoMapper(typeof(MapperInitializer));

//Injections
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBalanceRepository, BalanceRepository>();
builder.Services.AddSingleton<AuthManagerConfig>();

var app = builder.Build();

// Authorization Middleware
app.UseMiddleware<AuthenticationMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
