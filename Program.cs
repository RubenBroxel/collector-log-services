/*using CQRS.Models;
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;*/

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<CommandUsersBroxelContext>(opt => opt.UseInMemoryDatabase("CommandUsersBroxel"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//Bloque: Inyecciones de Dependencias
builder.Services.AddSingleton<IFileServices, LocalServices>();
builder.Services.AddSingleton<IGcpServices, GcpServices>();
builder.Services.AddSingleton<IJwtSecurity, JwtServices>();
builder.Services.AddSingleton<IPermissionServices, PermissionServices>();

//Bloque: Variables de Entorno de configuración de GCP para los Buckets
var GCP   = builder.Configuration.GetSection("GCP-ENV-LOG:GCP-DEV");
var Local = builder.Configuration.GetSection("GCP-ENV-LOG:LOCAL-STORAGE");


var bucket      = GCP.GetValue<string>("GcpBucketName");
var path        = GCP.GetValue<string>("LocalPath");
var credential  = GCP.GetValue<string>("GcpCredential");
var type        = GCP.GetValue<string>("GcpFileType");
var folder      = GCP.GetValue<string>("GcpBucketFolder");
var principal   = Local.GetValue<string>("PrincipalPath");
var user        = Local.GetValue<string>("FolderUsers");


builder.Services.AddEndpointsApiExplorer();

//Bloque: Configuración para Swagger
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "CQRS";
    config.Title = "CQRS v1";
    config.Version = "v1";
});
//Fin Bloque

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

//Método: Endpoint con la responsabilidad de subir archivos que vengan de Apps de Broxel para mandarlos a GCP
app.MapPost("api/logservice", async ( Stream logFile, IFileServices _fileServices, IGcpServices _gcpServices) =>
{
    LogFile    tempFile   = new LogFile();
    GcpLogFile gcpLogFile = new GcpLogFile(); 

    tempFile.FileName = $"{Guid.NewGuid()}.log";
    tempFile.filePath = [principal, user]; 

    string tempfile = _fileServices.CreateTempfilePath(tempFile.FileName,tempFile.filePath);
    using var stream = File.OpenWrite(tempfile);
    await logFile.CopyToAsync(stream);

    gcpLogFile.GcpBucket     = bucket;
    gcpLogFile.GcpCredential = credential;
    gcpLogFile.FileLocalPath = principal+"/"+user;
    gcpLogFile.FileType      = type;
    gcpLogFile.FileName      =tempFile.FileName;
    //stream.Name.ToString();
    stream.Close();
    await _gcpServices.SendToGcpBucketAsync(gcpLogFile);
});

app.Run();