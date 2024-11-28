using Microsoft.AspNetCore.Authentication.JwtBearer;
using NZWalks.API.Configrations;
using NZWalks.API.Data.DataBaseConfig;
using NZWalks.API.Interfaces;
using NZWalks.API.Mappings;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using NZWalks.API.Data;
using NZWalks.API.Interfaces.Token;
using NZWalks.API.Repositories.Token;
using Microsoft.OpenApi.Models;
using NZWalks.API.Interfaces.Image;
using NZWalks.API.Repositories.ImageRepo;
using Microsoft.Extensions.FileProviders;
using NZWalks.API.Repositories.RegionDir;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();

// if you want to add token from swager then modify this function AddSwaggerGen this is provided you authorization button on top
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1",new OpenApiInfo { Title="Nz Walk Api",Version="v1"});
    option.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In =  ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference()
                {
                    Type = ReferenceType.SecurityScheme,
                    Id  = JwtBearerDefaults.AuthenticationScheme
                },
                Scheme = "Oauth2",
                Name = JwtBearerDefaults.AuthenticationScheme,
                In = ParameterLocation.Header
            },
            new List<string>()

        }
    });
});
// this way is used for direct implement connection string 
//builder.Services.AddDbContext<NZWalksDbContext>(options =>
//options.UseSqlServer("Server=.;Database=NZWalksDb;Trusted_Connection=true;MultipleActiveResultSets=true;Encrypt=False;"));

//this way is implement connection string through appSetting.Js
//builder.Services.AddDbContext<NZWalksDbContext>(option => 
//option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//this way is used connection string through static method through this way can reduced your code 
builder.Services.AddDbConnetionStringThroughExtention(builder.Configuration);
builder.Services.AddAuthConnectionStringThroughExtention(builder.Configuration);
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

// You can define like both one is commented another is used  
//builder.Services.AddScoped(typeof(IRegion),typeof(RegionRepository));
builder.Services.AddScoped<IRegion, RegionRepository>();
builder.Services.AddDependencies(); // this is a extension method we can add all intefacce inject inside this function upper line can be inject inside this 
                                    //Function but im not add because this is also a way to inject 

//add AutoMapper Injected 
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

//add identity
builder.Services.AddIdentityCore<IdentityUser>().AddRoles<IdentityRole>().AddTokenProvider<DataProtectorTokenProvider<IdentityUser>>("NZWalks")
    .AddEntityFrameworkStores<NZWalksAuthDbContext>().AddDefaultTokenProviders();

//set password Validations
builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = false;
    option.Password.RequireLowercase = false;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});


/*Add JWT Token Injection Using Authentication*/
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidateAudience = true,
    ValidateLifetime = true,
    ValidateIssuerSigningKey = true,
    ValidIssuer = builder.Configuration["Jwt:Issuer"],
    ValidAudience = builder.Configuration["Jwt:Audience"],
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
});

var app = builder.Build();

// Configure the HTTP request pipeline. Keep Ordring For Middleware 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    //this is add becuase static file cannot run directly on Browser through URL if i have get through Host in which Image Css Html files 
    //now im want to image open in my browser through static file using host Url 
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Image")),
    RequestPath = "/Image"

});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
