using Voyagoo;
using Voyagoo.Entities;
using Voyagoo.Persistence;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.

builder.Services.AddDependences(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.UseStaticFiles();

app.MapControllers();

app.Run();
