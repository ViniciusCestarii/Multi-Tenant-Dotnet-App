# Multi-Tenant .NET App

This is a sample multi-tenant application that demonstrates how to build a multi-tenant application using ASP.NET Core and Entity Framework Core.

## Features

- Tenant Resolution Strategy


CurrentTenantService.cs
```cs
      var tenantInfo = await _context.Tenants.Where(x => x.Id == tenant).FirstOrDefaultAsync(); // check if tenant exists
      if (tenantInfo != null)
      {
          TenantId = tenant;
          ConnectionString = tenantInfo.ConnectionString; // optional connection string per tenant (can be null to use default database)
          return true;
      }
      else
      {
          throw new Exception("Tenant invalid"); 
      }
```

- Tenant Database Isolation if isolated = `true`

TenantService.cs
```cs
      public Tenant CreateTenant(CreateTenantRequest request)
      {

          string newConnectionString = null;
          if (request.Isolated == true)
          {
              // generate a connection string for new tenant database
              string dbName = "multiTenantAppDb-" + request.Id;
              string defaultConnectionString = _configuration.GetConnectionString("DefaultConnection");
              newConnectionString = defaultConnectionString.Replace("multi-tenant", dbName);

              // create a new tenant database and bring current with any pending migrations from ApplicationDbContext
              try
              {
                  using IServiceScope scopeTenant = _serviceProvider.CreateScope();
                  ApplicationDbContext dbContext = scopeTenant.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                  dbContext.Database.SetConnectionString(newConnectionString);
                  if (dbContext.Database.GetPendingMigrations().Any())
                  {
                      Console.ForegroundColor = ConsoleColor.Blue;
                      Console.WriteLine($"Applying ApplicationDB Migrations for New '{request.Id}' tenant.");
                      Console.ResetColor();
                      dbContext.Database.Migrate();
                  }
              }
              catch (Exception ex)
              {
                  throw new Exception(ex.Message);
              }
          }
      }
```

- EF Core Query Filters

ApplicationDbContext.cs
```cs
      // On Model Creating - multitenancy query filter, fires once on app start
      protected override void OnModelCreating(ModelBuilder builder)
      {
          builder.Entity<Product>().HasQueryFilter(a => a.TenantId == CurrentTenantId);
      }
```

## Getting Started

To get started, clone the repository and run the following commands:

```bash
git clone
```

```bash
dotnet ef database update --context ApplicationDbContext
```

```bash
dotnet run
```

Now you can navigate to `http://localhost:5284/swagger/index.html` to try the application.

## Prerequisites

.NET 8
PostgreSQL