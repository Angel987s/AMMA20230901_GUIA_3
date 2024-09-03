using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Add services to the container.

// Configuración para la autenticación por cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Configura el nombre del parámetro de URL para redireccionamiento no autorizado
        options.ReturnUrlParameter = "unauthorized";

        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                // Cambia el código de estado a No autorizado
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                // Establece el tipo de contenido como JSON (u otro formato deseado)
                context.Response.ContentType = "application/json";

                var message = new
                {
                    error = "No autorizado",
                    message = "Debe iniciar sesión para acceder a este recurso."
                };

                // Serializa el objeto 'message' en formato JSON
                var jsonMessage = JsonSerializer.Serialize(message);

                // Escribe el mensaje JSON en la respuesta HTTP
                return context.Response.WriteAsync(jsonMessage);
            }
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();

app.Run();
