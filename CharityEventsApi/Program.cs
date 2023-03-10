using CharityEventsApi;
using CharityEventsApi.Entities;
using CharityEventsApi.Middleware;
using CharityEventsApi.Services.AccountService;
using CharityEventsApi.Services.CharityEventService;
using CharityEventsApi.Services.FundraisingService;
using CharityEventsApi.Services.DonationService;
using CharityEventsApi.Services.LocationService;
using CharityEventsApi.Services.PersonalDataService;
using CharityEventsApi.Services.UserStatisticsService;
using CharityEventsApi.Services.VolunteeringService;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CharityEventsApi.Services.SearchService;
using CharityEventsApi.Services.VolunteerService;
using CharityEventsApi.Services.ImageService;
using FluentValidation;
using CharityEventsApi.Models.Validators;
using CharityEventsApi.Models.DataTransferObjects;
using FluentValidation.AspNetCore;
using CharityEventsApi.Services.AuthUserService;
using CharityEventsApi.Services.UserContextService;
using CharityEventsApi.Seeders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var authenticationSettings = new AuthenticationSettings();

builder.Configuration.GetSection("Authentication").Bind(authenticationSettings);
builder.Services.AddSingleton(authenticationSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
}
).AddJwtBearer(
    cfg =>
    {     
        cfg.RequireHttpsMetadata = false;
        cfg.SaveToken = true;
        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuer = authenticationSettings.JwtIssuer,
            ValidAudience = authenticationSettings.JwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationSettings.JwtKey)),
        };
    }
    );



builder.Services.AddControllers();
builder.Services.AddFluentValidationAutoValidation();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CharityEventsApi",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



//Configure DBContext
builder.Services.AddDbContext<CharityEventsDbContext>(
    option => option.UseMySql(
        builder.Configuration.GetConnectionString("CharityEventsConnectionString"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("CharityEventsConnectionString"))
        )
    );

builder.Services.AddScoped<RoleSeeder>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<ICharityEventService, CharityEventService>();
builder.Services.AddScoped<IVolunteeringService, VolunteeringService>();
builder.Services.AddScoped<IFundraisingService, FundraisingService>();
builder.Services.AddScoped<CharityEventFactory>();
builder.Services.AddScoped<FundraisingFactory>();
builder.Services.AddScoped<VolunteeringFactory>();
builder.Services.AddScoped<CharityEventActivation>();
builder.Services.AddScoped<CharityEventVerification>();
builder.Services.AddScoped<CharityEventDenial>();
builder.Services.AddScoped<FundraisingActivation>();
builder.Services.AddScoped<FundraisingVerification>();
builder.Services.AddScoped<FundraisingDenial>();
builder.Services.AddScoped<VolunteeringActivation>();
builder.Services.AddScoped<VolunteeringVerification>();
builder.Services.AddScoped<VolunteeringDenial>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<ICharityEventFactoryFacade, CharityEventFactoryFacade>();
builder.Services.AddScoped<IUserStatisticsService, UserStatisticsService>();
builder.Services.AddScoped<IPersonalDataService, PersonalDataService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<IUserContextService, UserContextService>();
builder.Services.AddTransient<IAuthUserService, AuthUserService>();
builder.Services.AddTransient<PersonalDataFactory>();
builder.Services.AddTransient<PersonalDataAddressFactory>();
builder.Services.AddTransient<IPersonalDataFactoryFacade, PersonalDataFactoryFacade>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<ISearchService, SearchService>();
builder.Services.AddScoped<IVolunteerService, VolunteerService>();
builder.Services.AddScoped<IValidator<RegisterUserDto>, RegisterUserDtoValidator>();
builder.Services.AddScoped<IValidator<LoginUserDto>, LoginUserDtoValidator>();
builder.Services.AddScoped<IValidator<AddAllPersonalDataDto>, AddAllPersonalDataDtoValidator>();
builder.Services.AddScoped<IValidator<AddAllCharityEventsDto>, AddAllCharityEventsDtoValidator>();
builder.Services.AddScoped<IValidator<AddCharityEventDto>, AddCharityEventDtoValidator>();
builder.Services.AddScoped<IValidator<AddCharityEventFundraisingDto>, AddCharityEventFundraisingDtoValidator>();
builder.Services.AddScoped<IValidator<AddCharityEventVolunteeringDto>, AddCharityEventVolunteeringDtoValidator>();
builder.Services.AddScoped<IValidator<AddDonationDto>, AddDonationDtoValidator>();
builder.Services.AddScoped<IValidator<AddLocationDto>, AddLocationDtoValidator>();
builder.Services.AddScoped<IValidator<AddVolunteerDto>, AddVolunteerDtoValidator>();
builder.Services.AddScoped<IValidator<DeleteVolunteerDto>, DeleteVolunteerDtoValidator>();
builder.Services.AddScoped<IValidator<EditAllPersonalDataDto>, EditAllPersonalDataDtoValidator>();
builder.Services.AddScoped<IValidator<EditCharityEventDto>, EditCharityEventDtoValidator>();
builder.Services.AddScoped<IValidator<EditCharityEventFundraisingDto>, EditCharityEventFundraisingDtoValidator>();
builder.Services.AddScoped<IValidator<EditCharityEventVolunteeringDto>, EditCharityEventVolunteeringDtoValidator>();
builder.Services.AddScoped<IValidator<EditLocationDto>, EditLocationDtoValidator>();
builder.Services.AddScoped<AuthCharityEventDecorator>();
builder.Services.AddScoped<AuthVolunteeringDecorator>();
builder.Services.AddScoped<AuthFundraisingDecorator>();
builder.Services.AddScoped<AuthLocationDecorator>();




var app = builder.Build();



app.UseCors(x => x
             .AllowAnyMethod()
             .AllowAnyHeader()
             .AllowAnyOrigin()
             .SetIsOriginAllowed(origin => true)); // allow any origin


app.UseMiddleware<ErrorHandlingMiddleware>();

// Configure the HTTP request pipeline.

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<RoleSeeder>();

seeder.Seed();

if (app.Environment.IsDevelopment())
{

}

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }