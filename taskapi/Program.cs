using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taskapi.Data;
using taskapi.Entities;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentity<User,IdentityRole>(options => {

    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;

}).AddRoles<IdentityRole>()
.AddEntityFrameworkStores<AppDbContext>(); // Pr tugasi organaman shuni 

builder.Services.AddDbContext<AppDbContext>(options =>{
     
    options.UseSqlite("Data Source = data.db");
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// await AppDbSeed.InitializeUserAsync(app);
// await AppDbSeed.InitializeRoleAsync(app);
// AppDbSeed.SeedUsersAndRolesAsync(app).Wait();
app.Run();
