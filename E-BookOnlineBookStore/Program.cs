using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.MapControllerRoute(
    name: "manageBooks",
    pattern: "admin/manage-books",
    defaults: new { controller = "ManageBooks", action = "Index" }
);

app.MapControllerRoute(
    name: "manageOrders",
    pattern: "admin/manage-orders",
    defaults: new { controller = "ManageOrders", action = "Index" }
);


app.Run();
