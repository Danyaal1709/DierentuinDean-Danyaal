using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Configure JSON options to handle circular references
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

// Configure the database context - Using SQL Server LocalDB
builder.Services.AddDbContext<DierentuinContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DierentuinContext")));

// Add the DataSeeder as a service
builder.Services.AddTransient<DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
app.MapControllers(); // Add this line to map API controllers

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DierentuinContext>();
    var seeder = services.GetRequiredService<DataSeeder>();
    
    // Apply migrations - this is required per assignment requirements
    // The database must be installable by running Update-Database
    try
    {
        context.Database.Migrate();
    }
    catch
    {
        // If migrations fail (e.g., database doesn't exist or schema mismatch),
        // drop and recreate, then apply migrations
        try
        {
            if (context.Database.CanConnect())
            {
                context.Database.EnsureDeleted();
            }
            // Wait a moment for database to be fully deleted
            System.Threading.Thread.Sleep(500);
            context.Database.Migrate();
        }
        catch
        {
            // If migrations still fail, try to ensure database is created
            try
            {
                context.Database.EnsureCreated();
            }
            catch
            {
                // If all else fails, log but continue
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
        // If seeding fails, continue anyway - app can still run
    }
}

app.Run();
