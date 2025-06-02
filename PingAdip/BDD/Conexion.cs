using DotNetEnv;

namespace PingAdip.BDD {
    public class Conexion {
        public Conexion() {
            var envPath = Path.Combine(AppContext.BaseDirectory, ".env");
            if (File.Exists(envPath))
                Env.Load(envPath);
        }
        public string GetConnectionString() {
            string server = Environment.GetEnvironmentVariable("DB_SERVER");
            string database = Environment.GetEnvironmentVariable("DB_NAME");
            string user = Environment.GetEnvironmentVariable("DB_USER");
            string pass = Environment.GetEnvironmentVariable("DB_PASS");

            if (string.IsNullOrEmpty(server) || string.IsNullOrEmpty(database) ||
                string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass)) {
                throw new InvalidOperationException("Faltan variables de entorno necesarias en el archivo .env.");
            }

            return $"Data Source={server};Initial Catalog={database};User ID={user};Password={pass};TrustServerCertificate=True;";
        }
    }
}
