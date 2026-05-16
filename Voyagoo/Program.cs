using Voyagoo;
using Voyagoo.Entities;
using Voyagoo.Persistence;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDependences(builder.Configuration);

var app = builder.Build();




app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var feature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();
        if (feature?.Error != null)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                message = feature.Error.Message,
                innerException = feature.Error.InnerException?.Message,
                stackTrace = feature.Error.StackTrace
            });
        }
    });
});








// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();


app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();

app.MapControllers();

app.Run();
