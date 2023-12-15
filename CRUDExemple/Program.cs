using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<IContriesService, CountrirSersice>();

builder.Services.AddSingleton<IContriesService, CountrirSersice>();
builder.Services.AddScoped<IPersonService, PersonService>();

builder.Services.AddDbContext<DbContextPersons>(optins =>
{
    optins.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PersonDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
						
	//OR

	//optins.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection"));

});



//Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDAtaBase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False

var app = builder.Build();

if (builder.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage(); 
}

Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
app.UseRouting();
app.MapControllers();
app.UseStaticFiles();

app.Run();
