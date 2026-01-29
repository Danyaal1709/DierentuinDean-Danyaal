using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure JSON options fot circular references
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configure the database context - SQL Server LocalDB
builder.Services.AddDbContext<DierentuinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DierentuinContext")));

// Add DataSeeder as service
builder.Services.AddTransient<DataSeeder>();

var app = builder.Build();

// Configure HTTP request pipeline.
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
app.MapRazorPages();
app.MapControllers(); // Add to map API controllers

// Seed database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DierentuinContext>();
    var seeder = services.GetRequiredService<DataSeeder>();
    
    // Apply migrations
    try
    {
        context.Database.Migrate();
    }
    catch
    {
        // If migrations fail
        // drop and recreate, apply migrations
        try
        {
            if (context.Database.CanConnect())
            {
                context.Database.EnsureDeleted();
            }
            // Wait for database to be deleted
            System.Threading.Thread.Sleep(500);
            context.Database.Migrate();
        }
        catch
        {
            // If migrations fail, ensure database is created
            try
            {
                context.Database.EnsureCreated();
            }
            catch
            {
                // If fails, log but continue
            }
        }
    }
    
    // Ensure database is accessible before seeding
    try
    {
        if (context.Database.CanConnect())
        {
            seeder.Seed();
        }
    }
    catch
    {
        // If seeding fails, continue. app can still run
    }
}

app.Run();
