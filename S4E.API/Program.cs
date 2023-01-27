using S4E.ADO;
using S4E.ADO.Models;
using S4E.ADO.Models.Dto.AssociadoDto;
using S4E.ADO.Profiles;
using S4E.ADO.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<AssociadoService, AssociadoService>();
builder.Services.AddScoped<Mapper, Mapper>();

builder.Services.AddScoped<Associado, Associado>();
builder.Services.AddScoped<CreateAssociadoDto, CreateAssociadoDto>();
builder.Services.AddScoped<GetAssociadoDto, GetAssociadoDto>();
builder.Services.AddScoped<ReadAssociadoDto, ReadAssociadoDto>();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
