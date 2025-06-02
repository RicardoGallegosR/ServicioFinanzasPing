using System.Data;
using Microsoft.Data.SqlClient;


namespace PingAdip.BDD {
    class spServicios {
        private readonly string _connection;

        public spServicios(string connection) {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        public async Task InsertarPingAsync(int servicioWebId, bool estatus=true, string mensaje = "") {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = new SqlCommand("Servicios.spInsertarPing", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@ServicioWebId", SqlDbType.TinyInt).Value = servicioWebId;
            cmd.Parameters.Add("@Estatus", SqlDbType.Bit).Value = estatus;
            cmd.Parameters.Add("@Mensaje", SqlDbType.VarChar, 250).Value = (object)mensaje ?? DBNull.Value;

            try {
                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            } catch (Exception ex) {
                Console.WriteLine($"Error en InsertarPingAsync: {ex.Message}");
                throw;
            }
        }
        public async Task<List<(int ServicioWebId, string Url, string Metodo)>> spServiciosGetAsync() {
            var servicios = new List<(int, string, string)>();

            using var conn = new SqlConnection(_connection);
            using var cmd = new SqlCommand("Servicios.spServicioWebGet", conn) {
                CommandType = CommandType.StoredProcedure
            };

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                int id = reader.GetInt32(0);
                string url = reader.GetString(1);
                string metodo = reader.GetString(2);
                servicios.Add((id, url, metodo));
            }

            return servicios;
        }


        public async Task<List<(bool Estatus, DateTime Fecha)>> spUltimosPingsGetAsync(int servicioWebId) {
            var resultados = new List<(bool, DateTime)>();
            using var conn = new SqlConnection(_connection);
            using var cmd = new SqlCommand("Servicios.spServicioWebEstatusGet", conn) {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@ServicioWebId", servicioWebId);
            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync()) {
                bool estatus = reader.GetBoolean(0);
                DateTime fecha = reader.GetDateTime(1);
                resultados.Add((estatus, fecha));
            }
            return resultados;
        }

        public async Task InsertarNotificacionAsync(int servicioWebId, byte alertaId) {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = new SqlCommand("Servicios.spNotificacionSet", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ServicioWebId", servicioWebId);
            cmd.Parameters.AddWithValue("@AlertaId", alertaId);

            await conn.OpenAsync();
            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<Dictionary<string, byte>> ObtenerAlertasAsync() {
            var alertas = new Dictionary<string, byte>();

            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = new SqlCommand("Servicios.spAlertasGet", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            await conn.OpenAsync();
            using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync()) {
                string descripcion = reader.GetString(reader.GetOrdinal("Alerta"));
                byte id = Convert.ToByte(reader["AlertaId"]);
                alertas[descripcion] = id;
            }

            return alertas;
        }

        public async Task<bool> PuedeNotificarAsync(int servicioWebId, int minutosEspera = 20) {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = new SqlCommand("Servicios.spNotificacionGet", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ServicioWebId", servicioWebId);

            await conn.OpenAsync();
            var resultado = await cmd.ExecuteScalarAsync();

            if (resultado == null || resultado == DBNull.Value) {
                return true;
            }

            DateTime ultimaNotificacion = Convert.ToDateTime(resultado);
            return (DateTime.Now - ultimaNotificacion).TotalMinutes >= minutosEspera;
        }





    }
}
