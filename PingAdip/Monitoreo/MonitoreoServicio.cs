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
                        Console.WriteLine($"Servicio {url} está estable.");
                        continue;
                    }

                    // 5. Notificar si es necesario
                    if (alertaTipos.TryGetValue(estado, out byte alertaId)) {
                        if (await servicioPing.PuedeNotificarAsync(id)) {
                            await servicioPing.InsertarNotificacionAsync(id, alertaId);
                            await bot.EnviarMensajeTelegramAsync($"⚠️ {estado}: El servicio {url} ha fallado {fallos}/{total} veces.");
                        } else {
                            Console.WriteLine($"Ya se notificó el estado '{estado}' recientemente para {url}.");
                        }
                    } else {
                        Console.WriteLine($"❗ No se encontró tipo de alerta para estado: {estado}");
                    }

                } catch (Exception ex) {
                    Console.WriteLine($"Error con el servicio ID {id}: {ex.Message}");
                }
            }
        }
    }
}
