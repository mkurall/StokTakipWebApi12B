using Microsoft.EntityFrameworkCore;
using StokTakipWebApi12B.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StokTakipWebApi12B.Protokol;
using System.Net;
using Newtonsoft.Json;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<StokTakipDbContext>(options => options.UseSqlServer(), ServiceLifetime.Singleton);


//Burasý swagger sayfasýndan jwt token test etmek için
builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});



//Oturum açma için gerekli
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
})
.AddJwtBearer("Bearer", options =>
{
    var serverSecret = new SymmetricSecurityKey(Encoding.UTF8.
        GetBytes("Bu gizli bir kelime"));

    options.TokenValidationParameters = new
        TokenValidationParameters
    {
        IssuerSigningKey = serverSecret,
        ValidIssuer = "stoktakip.com",
        ValidAudience = "stokstakip.com",
    };

    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            context.NoResult();
            context.Response.StatusCode = 500;
            context.Response.ContentType = "text/plain";
            context.Response.WriteAsync(context.Exception.ToString()).Wait();
            return Task.CompletedTask;
        },
        OnChallenge = async context =>
        {
            // Call this to skip the default logic and avoid using the default response
            context.HandleResponse();

            context.Response.Headers.Append("my-custom-header", "custom-value");
            ApiCevap cevap = new ApiCevap();
            cevap.BasariliMi = false;
            cevap.HataMesaji = "Bu metodu kullanabilmek için kimlik doðrýlamasý gerekli";

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            await context.Response.WriteAsync(JsonConvert.SerializeObject(cevap));
        }
    };
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    // runs on each request
    options.ForwardDefaultSelector = context =>
    {
        // filter by auth type
        string authorization = context.Request.Headers[HeaderNames.Authorization];

        if (authorization == null) return "Bearer";

        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase))
            return "Bearer";

        // otherwise always check for cookie auth
        return "Cookies";
    };
}); ;



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();//Bu oturum açma için gerekli

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
