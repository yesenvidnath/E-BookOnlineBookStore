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

app.MapControllerRoute(
    name: "register",
    pattern: "Register",
    defaults: new { controller = "Home", action = "Register" }
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


app.Run();
