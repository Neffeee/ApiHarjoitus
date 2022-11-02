using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebAPIHarjoitusKoodi.Models;
using WebAPIHarjoitusKoodi.Services.Interfaces;
using WebAPIHarjoitusKoodi.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

//Mahdollistaa liittymisen palvelimelle.
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy",
   builder => builder.AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
});



// ------Connection string luetaan app settings.json tiedostosta--------------

builder.Services.AddDbContext<NorthwindOriginalContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("paikallinen") // T‰h‰n lis‰t‰‰n sen connectionstringin nimi mihin halutaan yhdist‰‰.
    ));




// --------------- Tuodaan appSettings.jsoniin tekem‰mme AppSettings m‰‰ritys ----------
var appSettingsSection = builder.Configuration.GetSection("AppSettings");
builder.Services.Configure<AppSettings>(appSettingsSection);




// --------------- JWT Autentikaatio ---------------------------------------------------

var appSettings = appSettingsSection.Get<AppSettings>();
var key = Encoding.ASCII.GetBytes(appSettings.Key);

builder.Services.AddAuthentication(au =>
{
    au.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    au.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(jwt =>
{
    jwt.RequireHttpsMetadata = false;
    jwt.SaveToken = true;
    jwt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

    builder.Services.AddScoped<IAuthenticateService, AuthenticateService>();


// ---------------- JWT Autentikaatio m‰‰ritys p‰‰ttyy ------------------------------------




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

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("MyCorsPolicy");

app.MapControllers();

app.Run();
