using CarBox.Infrastructure;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Softgen.Infrastructure.ExceptionHandler;
using Softgen.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationManager configuration = builder.Configuration;
IWebHostEnvironment environment = builder.Environment;

builder.Services.AddDbContext<DefaultDbContext>(opt =>
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<DbContext, DefaultDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddAutoMapper(c => c.AddProfile<MappingProfile>(), typeof(Program));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ITeacherService, TeacherService>();
builder.Services.AddTransient<IGroupService, GroupService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var _logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Configuration.GetSection("Serilog"))
                    .WriteTo.File(Path.Combine(environment.ContentRootPath, "Logs/Log.log"), rollingInterval: RollingInterval.Day)
                    .CreateLogger();

builder.Host.UseSerilog(_logger);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
    db.Database.EnsureDeleted();
    if (!db.Database.CanConnect())
        db.Database.Migrate();
}

app.ConfigureExceptionHandler(_logger);

app.UseAuthorization();

app.MapControllers();

app.Run();
