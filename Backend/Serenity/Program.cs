using System.Reflection;
using System.Text.Json.Serialization;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Npgsql;
using Serenity.Common;
using Serenity.Database;
using Serenity.Modules.Comments;
using Serenity.Modules.Identity;
using Serenity.Modules.Posts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DataContext>(x =>
{
    string connectionString = builder.Configuration.GetConnectionString("Database");

    x.UseNpgsql(connectionString, o =>
    {
        o.UseNodaTime();
    });
});

builder.Services.AddMvc().AddFluentValidation(x =>
{
    
});

builder.Services.AddCommentsModule(builder.Configuration);
builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddPostsModule(builder.Configuration);

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(x => x.AddProfile<MapperProfile>());

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

public partial class Program { }