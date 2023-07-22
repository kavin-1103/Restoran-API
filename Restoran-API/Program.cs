using Microsoft.EntityFrameworkCore;
using Restaurant_Reservation_Management_System_Api.Data;
using Restaurant_Reservation_Management_System_Api.Controllers;
using Restaurant_Reservation_Management_System_Api.Services.Admin.TableService;
using Restaurant_Reservation_Management_System_Api.Services.User.ReservationService;
using Restaurant_Reservation_Management_System_Api.Services.User.CustomerServices;
using Restaurant_Reservation_Management_System_Api.Services.Admin.MenuCategoryService;
using Restaurant_Reservation_Management_System_Api.Services.Admin.FoodItemService;
using Restaurant_Reservation_Management_System_Api.Services.User.OrderServices;
using Microsoft.AspNetCore.Identity;
using Restaurant_Reservation_Management_System_Api.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Restaurant_Reservation_Management_System_Api.Services.Auth;
using Restaurant_Reservation_Management_System_Api.Repository;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using EmailService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddDbContext<RestaurantDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<RestaurantDbContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = false;
	options.Password.RequireUppercase = false;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 6;
	options.Password.RequiredUniqueChars = 1;
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
	{
		Description = "Standard Authorization Header using Bearer Scheme , e.g , \"bearer {token}\"",
		In = ParameterLocation.Header,
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey

	});
	c.OperationFilter<SecurityRequirementsOperationFilter>();
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<ITableServicesAdmin, TableServicesAdmin>();
builder.Services.AddScoped<IMenuCategoryServicesAdmin, MenuCategoryServicesAdmin>();
builder.Services.AddScoped<IFoodItemServicesAdmin, FoodItemServicesAdmin>();
builder.Services.AddScoped<IReservationServicesUser, ReservationServicesUser>();
builder.Services.AddScoped<ICustomerServicesUser, CustomerServicesUser>();
builder.Services.AddScoped<IOrderServicesUser, OrderServicesUser>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IAuthService, AuthService>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

builder.Services.AddControllers();
builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
	 options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
	 {
		 AuthenticationType = "Jwt",
		 ValidateIssuer = true,
		 ValidateAudience = true,
		 ValidateLifetime = true,
		 ValidateIssuerSigningKey = true,
		 ValidIssuer = builder.Configuration["Jwt:Issuer"],
		 ValidAudience = builder.Configuration["Jwt:Audience"],
		 IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))

	 };
 });

var app = builder.Build();

app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

// Configure the HTTP request pipeline.
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
