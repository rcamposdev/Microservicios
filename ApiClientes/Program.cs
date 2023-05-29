using ApiClientes.Models;
using ApiClientes.Common;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

app.MapGet("/", () => Results.Ok(DapperUtils.Query<Cliente>("SELECT * FROM Clientes").ToList()));

app.MapGet("/{cuil}", (long cuil) =>
    DapperUtils.QueryFirstOrDefault<Cliente>("SELECT * FROM Clientes WHERE cuil = @cuil", new { cuil })
    is Cliente cliente
        ? Results.Ok(cliente)
        : Results.NotFound()
    );

app.MapPost("/add", (Cliente cliente) =>
{
    bool existClient = DapperUtils.ExecuteScalar<int>("SELECT COUNT(*) FROM Clientes WHERE cuil = @cuil", new { cliente.cuil }) > 0;

    if(existClient) return Results.BadRequest($"Ya existe un cliente con cuil {cliente.cuil}");

    logger.Information($"Insert del cliente con cuil : ${cliente.cuil}");

    string query = "INSERT INTO Clientes(nombre,apellido,cuil,nroDocumento,esEmpleadoBna,paisOrigen) VALUES (@nombre,@apellido,@cuil,@nroDocumento,@esEmpleadoBna,@paisOrigen)";
    
    DapperUtils.Execute(query, cliente);
    
    return Results.Created($"/{cliente.cuil}", cliente);
});

app.MapDelete("/{cuil}", (long cuil) =>
{
    logger.Information($"Delete del cliente con cuil : ${cuil}");
    DapperUtils.Execute("DELETE FROM Clientes WHERE cuil = @cuil", new { cuil });   
    return Results.Ok(DapperUtils.Query<Cliente>("SELECT * FROM Clientes").ToList());
});

app.Run();
