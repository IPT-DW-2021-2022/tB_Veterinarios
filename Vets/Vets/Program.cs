using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Vets.Data;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

/* declarar a exist�ncia da base de dados */
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


/* declara��o da exist�ncia do servi�o da Autentica��o
 * e declara��o da classe com os dados do utilizador registado */
builder.Services.AddDefaultIdentity<ApplicationUser>(   // builder.Services.AddDefaultIdentity<IdentityUser>(
   options => options.SignIn.RequireConfirmedAccount = true)
   .AddRoles<IdentityRole>() // adiciona a capacidade da App a identificar 'perfis' de utilizador
   .AddEntityFrameworkStores<ApplicationDbContext>();


/* declarar o servi�o das vars de sess�o */
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
   options.IdleTimeout = TimeSpan.FromSeconds(120);
   options.Cookie.HttpOnly = true;
   options.Cookie.IsEssential = true;
});


// https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-6.0&tabs=aspnetcore2x&viewFallbackFrom=aspnetcore-2.1
builder.Services.Configure<IdentityOptions>(options => {
   // Default Lockout settings.
   options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
   options.Lockout.MaxFailedAccessAttempts = 5;
   options.Lockout.AllowedForNewUsers = true;
   // Password settings
   options.Password.RequireDigit = true;
   options.Password.RequiredLength = 6;
   options.Password.RequireNonAlphanumeric = false;
   options.Password.RequireUppercase = false;
   options.Password.RequireLowercase = true;
   options.Password.RequiredUniqueChars = 1;
   // Default User settings.
   options.User.AllowedUserNameCharacters =
           "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
   options.User.RequireUniqueEmail = false;
});


// configurar o uso de 'cookies'
builder.Services.ConfigureApplicationCookie(options => {
   options.AccessDeniedPath = "/Identity/Account/AccessDenied";
   options.Cookie.Name = "YourAppCookieName";
   options.Cookie.HttpOnly = true;
   options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
   options.LoginPath = "/Identity/Account/Login";
   // ReturnUrlParameter requires 
   //using Microsoft.AspNetCore.Authentication.Cookies;
   options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
   options.SlidingExpiration = true;
});




builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
   app.UseMigrationsEndPoint();
}
else {
   app.UseExceptionHandler("/Home/Error");
   // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
   app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// dar 'ordem' de uso do servi�o das vars. de sess�o
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
