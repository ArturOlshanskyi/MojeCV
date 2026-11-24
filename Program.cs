using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MojeCV.Data;
using MojeCV.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CvDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<CvDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CvDbContext>();
    await context.Database.MigrateAsync();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

    foreach (var role in new[] { "Admin", "User" })
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    var admin = await userManager.FindByEmailAsync("admin@rezerwacje.pl");
    if (admin == null)
    {
        admin = new AppUser
        {
            UserName = "admin@rezerwacje.pl",
            Email = "admin@rezerwacje.pl",
            FirstName = "Administrator",
            LastName = "Systemu",
            EmailConfirmed = true
        };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Admin");
    }

    if (!await context.Services.AnyAsync())
    {
        context.Services.AddRange(
            new Service { Name = "Strzyżenie damskie", Description = "Profesjonalne strzyżenie i stylizacja włosów.", Price = 120, DurationMinutes = 60 },
            new Service { Name = "Strzyżenie męskie", Description = "Strzyżenie klasyczne lub nowoczesne.", Price = 70, DurationMinutes = 45 },
            new Service { Name = "Koloryzacja", Description = "Koloryzacja i pielęgnacja włosów.", Price = 220, DurationMinutes = 120 });
        await context.SaveChangesAsync();

        var services = await context.Services.ToListAsync();
        var start = DateTime.Today.AddDays(1).AddHours(9);
        foreach (var service in services)
        {
            for (var day = 0; day < 7; day++)
            {
                for (var hour = 9; hour <= 15; hour += 2)
                {
                    context.ReservationSlots.Add(new ReservationSlot
                    {
                        ServiceId = service.Id,
                        StartTime = start.AddDays(day).AddHours(hour - 9),
                        Capacity = 1,
                        IsAvailable = true
                    });
                }
            }
        }
        await context.SaveChangesAsync();
    }
}

app.Run();
