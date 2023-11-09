using Microsoft.AspNetCore.Http.Connections;
using WebServer.Hubs;

string webroot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    Args = args,
    WebRootPath = webroot
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAppServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseRouting();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API");
});
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<InstrumentHub>("hub/instrument-hub", options =>
{
    options.Transports = HttpTransportType.WebSockets;
});


string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot");
DefaultFilesOptions fileOpt = new DefaultFilesOptions();
fileOpt.DefaultFileNames.Add("/index.html");
app.UseDefaultFiles(fileOpt);
app.UseStaticFiles();

app.Run();
