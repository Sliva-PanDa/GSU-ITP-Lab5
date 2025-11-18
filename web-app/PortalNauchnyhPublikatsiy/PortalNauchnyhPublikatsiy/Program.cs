using Microsoft.EntityFrameworkCore;
using PortalNauchnyhPublikatsiy.Infrastructure.Data;
using PortalNauchnyhPublikatsiy.Application.Interfaces;
using PortalNauchnyhPublikatsiy.Infrastructure.Repositories;
using PortalNauchnyhPublikatsiy.Application.Services;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IPublicationRepository, PublicationRepository>();
builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
builder.Services.AddScoped<IPublicationService, PublicationService>();
builder.Services.AddScoped<IJournalConferenceRepository, JournalConferenceRepository>();
builder.Services.AddScoped<IJournalConferenceService, JournalConferenceService>();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITeacherService, TeacherService>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
// Тестовый коммит для активации Git