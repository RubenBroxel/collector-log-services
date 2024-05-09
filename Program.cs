/*using CQRS.Models;
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;*/

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<CommandUsersBroxelContext>(opt => opt.UseInMemoryDatabase("CommandUsersBroxel"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CQRS";
    config.Title = "CQRS v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "CQRS";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

/* para pruebas de Query a sql Server en infraestructura de docker
app.MapGet("/users", () =>
{
    using( var context = new CommandUsersBroxelContext() )
    {
       return context.BrxUsers.ToList(); 
    }
});
*/

app.MapPost("/upload",
    async (HttpRequest request) =>
    {
        using (var reader = new StreamReader(request.Body, System.Text.Encoding.UTF8))
        {
            // Read the raw file as a `string`.
            string fileContent = await reader.ReadToEndAsync();
            Console.WriteLine(fileContent.ToString());
            return  fileContent.ToString() +  " File Was Processed Sucessfully!";
        }
    }).Accepts<IFormFile>("text/plain");


app.MapPost("v2/stream", async (Stream body) =>
{
    string tempfile = CreateTempfilePath();
    using var stream = File.OpenWrite(tempfile);
    await body.CopyToAsync(stream);
    return stream.Name.ToString();
});

static string CreateTempfilePath()
{
    var filename = $"{Guid.NewGuid()}.log";
    var directoryPath = Path.Combine("temp", "uploads");
    Console.WriteLine("Hola");
    if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
    return Path.Combine(directoryPath, filename);
}


app.Run();