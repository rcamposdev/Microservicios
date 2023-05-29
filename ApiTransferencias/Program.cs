using ApiTransferencias.Models;
using Serilog;
using Newtonsoft.Json;
using ApiTransferencias.Common;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

var transferencias = new List<TransferenciaResponse>();

app.MapGet("/", () => Results.Ok(DapperUtils.Query<TransferenciaResponse>("SELECT * FROM Transferencias").ToList()));

app.MapGet("/{cuentaOrigen}", (string cuentaOrigen) => {

  string query = "SELECT * FROM Transferencias WHERE cuentaOrigen = @cuentaOrigen";

  var response = DapperUtils.Query<TransferenciaResponse>(query, new {cuentaOrigen = cuentaOrigen}).ToList();

  return Results.Ok(response);

});

app.MapPost("/RealizarTransferencia", (TransferenciaRequest request) =>
{
  try
  {   

    Kafka.RegistrarMensaje($"Realizando transferencia cuit originante {request.cuilOriginante} cuit destinatario {request.cuilDestinatario}");
    
    logger.Debug(JsonConvert.SerializeObject(request, Formatting.Indented));

    var clienteOriginante = ApiTransferencias.WS.ManagerClient.ConsultaPorCuil(request.cuilOriginante);
    
    if(clienteOriginante == null) return Results.BadRequest("El cliente Originante no es un cliente BNA");

    var clienteDestinatario = ApiTransferencias.WS.ManagerClient.ConsultaPorCuil(request.cuilDestinatario);
    
    if(clienteDestinatario == null) return Results.BadRequest("El cliente Destinatario no es un cliente BNA");
    
    var response = new TransferenciaResponse{
      resultado = "FINALIZADA",
      importe = request.importe,
      cuentaOrigen = request.cbuOrigen,
      cuentaDestino = request.cbuDestino
    };

    transferencias.Add(response);

    var query = "INSERT INTO Transferencias(resultado,importe,cuentaOrigen,cuentaDestino) VALUES (@resultado,@importe,@cuentaOrigen,@cuentaDestino)";

    DapperUtils.Execute(query, response);

    return Results.Created($"/{request.cbuOrigen}",response);
    
  }
  catch (System.Exception ex)
  {
    logger.Error(ex.Message);  
    return Results.StatusCode(500);
  }

});

app.Run();