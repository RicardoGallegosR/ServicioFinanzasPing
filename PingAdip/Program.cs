using PingAdip.Credenciales;
using PingAdip.Ping;
using PingAdip.BOT;
using PingAdip.BDD;
using System;

class Program {
    static async Task Main() {

        var cnx = new Conexion();
        var servicioPing = new spServicios(cnx.GetConnectionString(), "Servicios.spInsertarPing");



        url urlGen = new url();

        Credenciales bot = new Credenciales();

        var urls = new List<(string Nombre, string Url, int ServicioWebId)>
        {
            ("AdeudosFinanzasPuntosPOST", urlGen.AdeudosFinanzasPuntosPost(),1),
            ("ListadoCarritosGet", urlGen.CatalogosCarritosGet(),2),
            ("AdeudosFinanzasGet", urlGen.AdeudosFinanzasGet(),3)
        };

        foreach (var (nombre, url, servicioId) in urls) {
            bool isOnline = await PingApi.IsServiceOnlineAsync(url);
            if (isOnline) {
                Console.WriteLine($"El sistema {nombre} está en línea.");
                await servicioPing.InsertarPingAsync(servicioId, true, "");
            }
            else {
                Console.WriteLine($"El sistema {nombre} está caído.");
                await servicioPing.InsertarPingAsync(servicioId, false, "PingFallo");
            }
        }

    }
}
