using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using taskapi.Data;
using taskapi.Entities;
using taskapi.Services;
using taskapi.Statics;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddControllers();

builder.Services.AddCors(options => {
    
       options.AddPolicy("MyPolicy", builder => {

               builder.AllowAnyOrigin()
                                      .AllowAnyMethod()
                                                      .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<IProductAuditService, ProductAuditService>();

builder.Services.AddIdentity<User,IdentityRole>(options => {

    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;

}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders(); // Pr tugasi organaman shuni 

builder.Services.AddAuthentication(options => {

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddCookie("cookie")
.AddJwtBearer(options =>{

   options.SaveToken = true ;
   options.RequireHttpsMetadata = false;
   options.TokenValidationParameters = new TokenValidationParameters()
   {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
   };

});



builder.Services.AddDbContext<AppDbContext>(options =>{
     
    options.UseLazyLoadingProxies().UseSqlite("Data Source = data.db");
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
// builder.Services.AddSwaggerGen(options =>
// {
//     options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
//     {
//         Description = "Enter given token like this: Bearer <your_token>",
//         In = ParameterLocation.Header,
//         Name = "Authorization",
//         Type = SecuritySchemeType.ApiKey,
//         Scheme = "Bearer"
//     });

//     options.AddSecurityRequirement(new OpenApiSecurityRequirement
//     {
//         {
//             new OpenApiSecurityScheme()
//             {
//                 Reference = new OpenApiReference
//                 {
//                     Id="Bearer",
//                     Type = ReferenceType.SecurityScheme
//                 }
//             },
//             new List<string>()
//         }
//     });
// });


Vat.Value = double.Parse(builder.Configuration["VAT"]);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("MyPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

AppDbSeed.SeedUsersAndRolesAsync(app).Wait();

app.Run();
