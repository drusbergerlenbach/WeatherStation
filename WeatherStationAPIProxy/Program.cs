using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using System.IdentityModel.Tokens.Jwt;
using WeatherStationAPIProxy.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd")
//    .EnableTokenAcquisitionToCallDownstreamApi()
//    .AddDownstreamWebApi("RealApi", builder.Configuration.GetSection("RealApi"))
//    .AddInMemoryTokenCaches();

builder.Services.AddTransient<HttpClientService>();
builder.Services.AddHttpClient();

builder.Services.AddOptions();

JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
// IdentityModelEventSource.ShowPII = true;
// JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd")
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddInMemoryTokenCaches();



//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
