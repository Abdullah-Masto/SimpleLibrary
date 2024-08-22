using Library_Data;
using Library_Data.Repos;
using Library_Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using SimpleLibrary.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllers(options =>
{
    options.ReturnHttpNotAcceptable = true;
}).AddXmlDataContractSerializerFormatters()
  .AddNewtonsoftJson();

builder.Services.AddDbContext<LibraryContext>(
    options => options.UseSqlServer(builder.Configuration["ConnectionStrings:LibraryDbConnectionString"]));

// Register the HttpClient and StackOverflowService
builder.Services.AddHttpClient<StackOverflowService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


// Add controllers with views (includes API controllers)
builder.Services.AddControllersWithViews();

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

//

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

Console.WriteLine(app.Environment.EnvironmentName);

app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
