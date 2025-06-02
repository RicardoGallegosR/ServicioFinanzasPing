namespace PingAdip.Ping {
    public class PingApi {
        public static async Task<bool> IsServiceOnlineAsync(string url, string metodo = "GET", HttpContent? content = null) {
            try {
                using HttpClient client = new();
                client.Timeout = TimeSpan.FromSeconds(10);

                metodo = metodo.ToUpperInvariant();

                HttpResponseMessage response = metodo switch  {
                    "GET" => await client.GetAsync(url),
                    "POST" => await client.PostAsync(url, content ?? new StringContent("")),
                    _ => throw new NotSupportedException($"Método HTTP '{metodo}' no soportado.")
                };

                return response.IsSuccessStatusCode;
            } catch {
                return false;
            }
        }


    }
}
