using PingAdip.Credenciales;
using PingAdip.Ping;
using PingAdip.BOT;
using PingAdip.BDD;

namespace PingAdip.Monitoreo {
    public class MonitoreoServicio {
        public async Task Monitor() { 
            var cnx = new Conexion();
            var servicioPing = new spServicios(cnx.GetConnectionString());
            var bot = new BOT.Credenciales();
            var urlGen = new url();

            var servicios = await servicioPing.spServiciosGetAsync();
            var alertaTipos = await servicioPing.ObtenerAlertasAsync();

            foreach (var (id, url, metodo) in servicios) {
                try {
                    // 1. Hacer ping al servicio
                    bool isOnline = await PingApi.IsServiceOnlineAsync(url, metodo);
                    Console.WriteLine($"[{metodo}] Servicio ID {id} => {(isOnline ? "En línea" : "Caído")}");

                    // 2. Registrar el resultado
                    await servicioPing.InsertarPingAsync(id, isOnline, isOnline ? "" : "PingFallo");

                    // 3. Analizar los últimos 5 pings
                    var historial = await servicioPing.spUltimosPingsGetAsync(id);
                    int total = historial.Count;
                    int fallos = historial.Count(p => !p.Estatus);

                    // 4. Determinar estado
                    string estado = "Estable";
                    if (fallos == 5) estado = "Caído";
                    else if (fallos >= 3) estado = "Intermitente";

                    if (estado == "Estable") {
                        var ultimaAlerta = await servicioPing.ObtenerUltimaAlertaDescripcionAsync(id);

                        if (ultimaAlerta == "Caído" || ultimaAlerta == "Intermitente") {
                            await bot.EnviarMensajeTelegramAsync($"El servicio {url} ha sido restablecido.");
                            if (alertaTipos.TryGetValue("Recuperado", out byte alertaRecuperado)) {
                                await servicioPing.InsertarNotificacionAsync(id, alertaRecuperado);
                            } else {
                                Console.WriteLine("No se encontró la alerta 'Recuperado' en el diccionario.");
                            }

                        }

                        Console.WriteLine($"Servicio {url} está estable.");
                        continue;
                    }

                    // 5. Notificar si es necesario
                    if (alertaTipos.TryGetValue(estado, out byte alertaId)) {
                        var ultimaAlerta = await servicioPing.ObtenerUltimaAlertaDescripcionAsync(id);

                        if (ultimaAlerta != estado) {
                            await servicioPing.InsertarNotificacionAsync(id, alertaId);
                            await bot.EnviarMensajeTelegramAsync($"{estado}: El servicio {url} ha fallado {fallos}/{total} veces.");
                        } else {
                            Console.WriteLine($"Ya se notificó que el servicio {url} está en estado '{estado}'. No se vuelve a notificar.");
                        }
                    }

                } catch (Exception ex) {
                    Console.WriteLine($"Error con el servicio ID {id}: {ex.Message}");
                }
            }
        }
    }
}
