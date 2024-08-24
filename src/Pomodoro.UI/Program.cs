using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pomodoro.Library.Authorization;
using Pomodoro.Library.Data;
using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using Pomodoro.Library.Models.Utility;
using Pomodoro.Library.Services;
using Pomodoro.Library.Services.Interfaces;
using Pomodoro.UI.Middleware;
using VnLibrary.Services.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AddPageRoute("/Timer/Index", "");

    options.Conventions
        .AuthorizeFolder("/") // Require auth for all paths
        .AuthorizeAreaFolder("Registered", "/");

});

builder.Services.AddRazorComponents();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // Make the session cookie essential
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Set session timeout
});


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration.GetSection("Google:ClientId").Value;
    googleOptions.ClientSecret = builder.Configuration.GetSection("Google:ClientSecret").Value;
});

// Resouce authorization handlers
builder.Services.AddAuthorizationHandlers();

// Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("Pomodoro"), new MySqlServerVersion(new Version(8, 0, 33))));

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();
builder.Services.AddScoped<IEmailSender, EmailSender>();

// Must add after Identity.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

#region App Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IStudyTaskService, StudyTaskService>();
builder.Services.AddScoped<ITaskLabelService, TaskLabelService>();
builder.Services.AddScoped<ITaskPriorityService, TaskPriorityService>();
builder.Services.AddScoped<ICourseService, CourseService>();
#endregion

#region Auto Mapper
builder.Services.AddAutoMapper(typeof(MappingProfile));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.UseSession();
app.UseMiddleware<PreferredThemeMiddleware>();

app.MapGet("/api/", () => {
    var a = "test";
    return Results.Ok();
});

app.Run();
