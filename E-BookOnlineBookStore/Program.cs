using E_BookOnlineBookStore.Data;
using E_BookOnlineBookStore.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
    options.Cookie.HttpOnly = true; // Make the cookie accessible only to the server
    options.Cookie.IsEssential = true; // Required for GDPR compliance
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); // The default HSTS value is 30 days.
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

/* Define Application Routes */

// Shop Routes
/* Products Routes */
app.MapControllerRoute(
    name: "getProducts",
    pattern: "Products/GetProducts",
    defaults: new { controller = "Products", action = "GetProducts" }
);


/* Routs of the application */
app.MapControllerRoute(
    name: "shop",
    pattern: "Shop",
    defaults: new { controller = "Books", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "singleBook",
    pattern: "Books/{bookId}",
    defaults: new { controller = "Books", action = "SingleBook" }
);

app.MapControllerRoute(
    name: "Orders",
    pattern: "Orders",
    defaults: new { controller = "Orders", action = "Index" }
);

//app.MapControllerRoute(
//    name: "register",
//    pattern: "Account/Register/",
//    defaults: new { controller = "Home", action = "Register" }
//);

app.MapControllerRoute(
    name: "register",
    pattern: "Account/Register/{action=Index}/{id?}",
    defaults: new { controller = "Register" }
);

/* Customer AccountRoutes */
app.MapControllerRoute(
    name: "customerDashboard",
    pattern: "Customer/{action=Index}/{id?}",
    defaults: new { controller = "Customer" }
);

/* Admin Routes */
app.MapControllerRoute(
    name: "admin",
    pattern: "Book-Admin/{action=Index}/{id?}",
    defaults: new { controller = "Admin" }
);

app.MapControllerRoute(
    name: "adminLogin",
    pattern: "ebs-Admin",
    defaults: new { controller = "Admin", action = "Login" }
);

/* Common controllers */

app.Run();
