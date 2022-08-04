using ReadingListApi.Services.ReadingList;
using ReadingListApi.Services.Screenshot;
using Screenshots;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddSingleton<Urlbox>(sp => new Urlbox(builder.Configuration["Urlbox:ApiKey"], builder.Configuration["Urlbox:ApiSecret"]));

builder.Services.AddSingleton<IScreenshotService, UrlboxScreenshotService>();
builder.Services.AddSingleton<IReadingListService, ReadingListService>();

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

Directory.CreateDirectory(app.Configuration["ScreenshotsFolder"]);

app.UseAuthorization();

app.MapControllers();

app.Run();
