using Serilog;
using SafeCommerce.ProxyApi.Helpers;
using SafeCommerce.ProxyApi.Helpers.Middlewares;

var builder = WebApplication.CreateBuilder(args);

builder.Services.InjectServices(builder.Configuration, builder.Host);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<API_Helper_ProxyForwardUserIpMiddleware>();

app.UseCors(builder.Configuration.GetSection("Cors:Policy:Name").Value!);

app.UseMiddleware<API_Helper_JwtCookieToHeaderMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.UseMiddleware<API_Helper_ProxyForwardAntiForgeryToken>();

app.UseMiddleware<API_Helper_CookieForwardingToClientMiddleware>();

app.UseMiddleware<API_HELPER_MetadataLoggingMiddleware>();

app.MapControllers();

app.UseHsts();

app.UseSerilogRequestLogging();


app.Run();