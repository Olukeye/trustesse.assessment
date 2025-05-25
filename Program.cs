
using Trustesse_Assessment.IRepository;
using Trustesse_Assessment.Repository;
using Trustesse_Assessment.Configuration;
using Trustesse_Assessment.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using Trustesse_Assessment.AuthServices;
using Microsoft.Extensions.DependencyInjection;
using Trustesse_Assessment.ServiceExtensions;
using Trustesse_Assessment.EmailService;
using Microsoft.AspNetCore.Http.Features;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuth, Auth>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddAutoMapper(typeof(MapperInitilizer)); //Setup for DTO's
builder.Services.AddDbContext<DatabaseContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("sqlConnection"));
});
builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

var Config = builder.Configuration;

builder.Services.AddAuthentication();
builder.Services.AddAuthorization();
builder.Services.ConfigureIdentity();
builder.Services.ConfigureJWT(Config);
builder.Services.ConfigSwagger(Config);
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
//builder.Services(emailConfig);
builder.Services.Configure<FormOptions>(option =>
{
option.ValueLengthLimit = int.MaxValue;
option.MultipartBodyLengthLimit = int.MaxValue;
option.MemoryBufferThreshold = int.MaxValue;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssessmentTest v1"));
}

app.ExceptionHandlerConfiguration();

app.UseSwagger();

app.UseCors("AllowAll");

app.UseRouting();

app.UseHttpsRedirection();

app.MapControllers();

app.UseAuthentication();

app.UseAuthorization();

app.Run();
