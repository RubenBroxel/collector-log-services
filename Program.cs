/*using CQRS.Models;
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;*/
//using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
//builder.Services.AddDbContext<CommandUsersBroxelContext>(opt => opt.UseInMemoryDatabase("CommandUsersBroxel"));
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddSingleton<IFileServices, LocalServices>();
builder.Services.AddSingleton<IJwtSecurity, JwtServices>();
builder.Services.AddSingleton<IPermissionServices, PermissionServices>();

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

string Secret= "Hola_Mundo";


//Método end point que servicio de subir archivos archivos
app.MapPost("api/logservice" ,async (Stream body ,IFileServices _fileServices) =>
{
    string tempfile = _fileServices.CreateTempfilePath();
    using var stream = File.OpenWrite(tempfile);
    await body.CopyToAsync(stream);
    return stream.Name.ToString();
});



// Endpoint para datos seguros
app.MapGet("/api/secure", (IPermissionServices _permission, HttpContext httpContext) =>
{
    var token = httpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    var principal = _permission.GetPrincipal(token, Secret);
    
    // Verifica si el token es válido
    if (principal != null)
    {
        // Accede a los datos seguros
        var username = principal.Identity.Name;
        return Results.Ok($"Hola, {username}! Este es un dato seguro.");
    }
    return Results.Unauthorized();
});

// Endpoint para generar tokens
app.MapPost("/api/auth/token", ( IJwtSecurity _jwtSevice, UserCredentials credentials) =>
{
    var jwt = _jwtSevice.IsValidUser(credentials);
    // Verifica las credenciales del usuario
    if (jwt)
    {
        // Genera y retorna el token
        var token = _jwtSevice.GenerateToken(credentials.Username, Secret);
        return Results.Ok(new { Token = token });
    }
    return Results.Unauthorized();
});

app.Run();