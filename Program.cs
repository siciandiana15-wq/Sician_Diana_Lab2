using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sician_Diana_Lab2.Data;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<Sician_Diana_Lab2Context>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Sician_Diana_Lab2Context")
        ?? throw new InvalidOperationException("Connection string 'Sician_Diana_Lab2Context' not found.")
    ));

builder.Services.AddDbContext<LibraryIdentityContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Sician_Diana_Lab2Context")
        ?? throw new InvalidOperationException("Connection string 'Sician_Diana_Lab2Context' not found.")
    ));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<LibraryIdentityContext>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
