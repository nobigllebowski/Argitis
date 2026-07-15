using Argitis.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddLocalization(options =>
{
    options.ResourcesPath = "";
});

builder.Services.AddControllersWithViews()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization();

// Регистрация EmailSettings
var emailSettings = builder.Configuration.GetSection("EmailSettings").Get<EmailSettings>();
builder.Services.AddSingleton(emailSettings!);

// Регистрация EmailService
builder.Services.AddScoped<IEmailService, EmailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var supportedCultures = new[]
{
    new CultureInfo("en"),
    new CultureInfo("fr"),
    new CultureInfo("de"),
    new CultureInfo("es"),
    new CultureInfo("fi"),
    new CultureInfo("hr"),
    new CultureInfo("it"),
    new CultureInfo("lt"),
    new CultureInfo("lv"),
    new CultureInfo("mt"),
    new CultureInfo("nl"),
    new CultureInfo("pl"),
    new CultureInfo("pt"),
    new CultureInfo("ro"),
    new CultureInfo("sk")
};

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("en"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
    RequestCultureProviders = new List<IRequestCultureProvider>
    {
        new CookieRequestCultureProvider()
    }
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.Use(async (context, next) =>
{
    var tempData = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>()
        .GetTempData(context);

    // Принудительно удаляем ключ, если он есть
    tempData.Remove("SuccessMessageKey");
    tempData.Remove("ContactSuccessMessage");
    tempData.Remove("AppointmentSuccessMessage");

    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
