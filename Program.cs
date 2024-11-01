using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MultiTenantApp.Extensions;
using MultiTenantApp.Middleware;
using MultiTenantApp.Models;
using MultiTenantApp.Services;
using MultiTenantApp.Services.ProductService;
using MultiTenantApp.Services.TenantService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Current tenant service with scoped lifetime (created per each request)
builder.Services.AddScoped<ICurrentTenantService, CurrentTenantService>();

// adding a database service with configuration -- connection string read from appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<TenantDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

// Product CRUD service with transient lifetime
builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MultiTenantApp", Version = "v1" });

    c.OperationFilter<TenantHeaderOperationFilter>();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseMiddleware<TenantResolver>();
app.MapControllers();

app.Run();
