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
    .AddEntityFrameworkStores<ApplicationDbContext>();

/* declarar o servi�o das vars de sess�o */
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options => {
      options.IdleTimeout = TimeSpan.FromSeconds(120);
      options.Cookie.HttpOnly = true;
      options.Cookie.IsEssential = true;
   }
   );


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
