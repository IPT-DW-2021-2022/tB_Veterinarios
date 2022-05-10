using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Vets.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

/* declarar a existência da base de dados */
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

/* declaração da existência do serviço da Autenticação */
builder.Services.AddDefaultIdentity<IdentityUser>(
   options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

/* declarar o serviço das vars de sessão */
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

// dar 'ordem' de uso do serviço das vars. de sessão
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
