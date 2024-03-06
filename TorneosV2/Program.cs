using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using TorneosV2.Areas.Identity;
using TorneosV2.Data;
using Radzen;
using static Org.BouncyCastle.Math.EC.ECCurve;
using TorneosV2.Modelos;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
/*
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
*/
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 4;
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
}).AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<AuthenticationStateProvider,
    RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

builder.Services.AddTransient<IAddUser, RAddUser>();
builder.Services.AddTransient<IEnviarMail, REnviarMail>();

builder.Services.AddTransient<Repo<ZConfig, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z100_Org, ApplicationDbContext>>();

builder.Services.AddTransient<Repo<Z102_Grupo, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z104_GpoDet, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z110_User, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z170_Files, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z172_Registros, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z190_Bitacora, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z192_Logs, ApplicationDbContext>>();

builder.Services.AddTransient<Repo<Z300_Nombres, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z302_Contactos, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z304_Domicilio, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z309_Pariente, ApplicationDbContext>>();
//builder.Services.AddTransient<Repo<Z310_Fisico, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z360_Solicitud, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z362_SolDet, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z368_Seguimiento, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z380_Servicios, ApplicationDbContext>>();
builder.Services.AddTransient<Repo<Z390_Pais, ApplicationDbContext>>();

builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


builder.Services.AddRadzenComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

