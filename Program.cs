using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // configure MVC

// Cookie Authentication - register Cookir Authentication scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.LoginPath = "/account/login";
            });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// enable serving of static files, to serve our style sheets out of the wwwroot folder
app.UseStaticFiles();

// Make route information available for authentication decisions. Authentication comes
// before EndPoints so that users are authenticated before accessing the endpoints
app.UseRouting();

// authentcation middleware, uses registered authentcation schemes
app.UseAuthentication();

// Endpoints are setup to use default routing
app.UseEndpoints(endpoints =>
        {
        endpoints.MapDefaultControllerRoute();
        });

// app.MapGet("/", () => "Hello World!");

app.Run();
