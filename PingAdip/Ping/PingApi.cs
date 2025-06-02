namespace PingAdip.Ping {
    public class PingApi {
        public static async Task<bool> IsServiceOnlineAsync(string url) {
            try {
                using HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                var response = await client.GetAsync(url);
                return response.IsSuccessStatusCode;
            } catch {
                return false;
            }
        }
    }
}
