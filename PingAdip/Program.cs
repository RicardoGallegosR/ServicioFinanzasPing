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

        bool isUp = await PingApi.IsServiceOnlineAsync(url2);

        if (isUp) {
            Console.WriteLine("El sistema está en línea.");
            Console.WriteLine($"{bot.ObtenerEnlaceGetUpdates()}");
            await bot.EnviarMensajeTelegramAsync("⚠️ El servicio falló: Finanzas CDMX");
        } else {
            Console.WriteLine("El sistema está caído.");

    }
}
