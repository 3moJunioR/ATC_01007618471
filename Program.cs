using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using EventBookingAPI.Data;

var b = WebApplication.CreateBuilder(args);

b.Services.AddControllers();

b.Services.AddDbContext<ApplicationDbContext>(o =>
    o.UseSqlServer(b.Configuration.GetConnectionString("DefaultConnection")));

b.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = b.Configuration["Jwt:Issuer"],
            ValidAudience = b.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(b.Configuration["Jwt:Key"]))
        };
    });

b.Services.AddLocalization(o => o.ResourcesPath = "Resources");

b.Services.AddControllers().AddDataAnnotationsLocalization().AddViewLocalization();

b.Services.Configure<RequestLocalizationOptions>(o =>
{
    var langs = new[] { "en-US", "ar-SA" };
    o.SetDefaultCulture(langs[0]).AddSupportedCultures(langs).AddSupportedUICultures(langs);
});

b.Services.AddEndpointsApiExplorer();
b.Services.AddSwaggerGen();

var app = b.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TempC, string? Sum)
{
    public int TempF => 32 + (int)(TempC / 0.5);
}