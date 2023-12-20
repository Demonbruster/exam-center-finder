using ExamCenterFinder.Api.Application;
using ExamCenterFinder.Api.Application.Services;
using ExamCenterFinder.Api.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ExamCenterFinderDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddTransient<IExamCenterRepository, ExamCenterRepository>();
builder.Services.AddTransient<IExamSlotsRepository, ExamSlotsRepository>();
builder.Services.AddTransient<IZipCodeCenterPointRepository, ZipCodeCenterPointRepository>();
builder.Services.AddTransient<IDistanceCalculatorService, DistanceCalculatorService>();
builder.Services.AddTransient<IAvailabilityService, AvailabilityService>();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.AddConsole(); // You can add other logging providers as needed.
});


builder.Services.AddControllers();
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

app.MapControllers();

app.Run();
