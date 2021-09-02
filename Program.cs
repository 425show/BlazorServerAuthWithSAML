using Microsoft.AspNetCore.Components;
using BlazorServerWithSAML.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Sustainsys.Saml2.Metadata;
using Sustainsys.Saml2;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(sharedOptions =>
{
  sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
  sharedOptions.DefaultChallengeScheme = "Saml2";
})
.AddSaml2(options =>
{
  options.SPOptions.EntityId = new EntityId("http://myrandomapplication/samltesting");
  options.IdentityProviders.Add(
    new IdentityProvider(
      new EntityId("https://sts.windows.net/b55f0c51-61a7-45c3-84df-33569b247796/"), options.SPOptions)
      {
        MetadataLocation = "https://login.microsoftonline.com/b55f0c51-61a7-45c3-84df-33569b247796/federationmetadata/2007-06/federationmetadata.xml?appid=3245199b-1a5d-42df-93ce-e64ac7f5b938"
      });
  })
.AddCookie();

builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
