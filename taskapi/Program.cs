using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taskapi.Data;
using taskapi.Entities;
using taskapi.Services;
using taskapi.Statics;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
.AddEntityFrameworkStores<AppDbContext>(); // Pr tugasi organaman shuni 

builder.Services.AddDbContext<AppDbContext>(options =>{
     
    options.UseLazyLoadingProxies().UseSqlite("Data Source = data.db");
});

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
