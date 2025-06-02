namespace PingAdip.BOT {
    public class Credenciales {
        // Puedes llenarlos desde appsettings o aquí mismo (privado o protegido)
        private readonly string TOKEN = "8084713896:AAGRv5xXlaopPdCTQkQLpVecX49XLJTQgzc"; 
        private readonly string CHAT_ID = "-4833553426";     // tu grupo o usuario

        public string ObtenerEnlaceGetUpdates() {
            return $"https://api.telegram.org/bot{TOKEN}/getUpdates";
        }

        public async Task EnviarMensajeTelegramAsync(string mensaje) {
            string url = $"https://api.telegram.org/bot{TOKEN}/sendMessage";

            using var client = new HttpClient();
            var parametros = new Dictionary<string, string>
            {
                { "chat_id", CHAT_ID },
                { "text", mensaje }
            };

            var contenido = new FormUrlEncodedContent(parametros);
            await client.PostAsync(url, contenido);
        }
    }
}
