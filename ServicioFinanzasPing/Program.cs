using ServicioFinanzasPing;
using PingAdip.Monitoreo;


var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddTransient<MonitoreoServicio>(); 
builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();
