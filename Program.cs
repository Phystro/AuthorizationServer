var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // configure MVC

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// enable serving of static files, to serve our style sheets out of the wwwroot folder
app.UseStaticFiles();

app.UseRouting();

// Endpoints are setup to use default routing
app.UseEndpoints(endpoints =>
        {
        endpoints.MapDefaultControllerRoute();
        });

app.MapGet("/", () => "Hello World!");

app.Run();
