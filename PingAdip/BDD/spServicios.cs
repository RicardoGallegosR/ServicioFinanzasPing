using System.Data;
using Microsoft.Data.SqlClient;


namespace PingAdip.BDD {
    class spServicios {
        private readonly string _connection;
        private readonly string _storedProcedure;

        public spServicios(string connection, string storedProcedure) {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            _storedProcedure = storedProcedure ?? throw new ArgumentNullException(nameof(storedProcedure));
        }
        public async Task InsertarPingAsync(int servicioWebId, bool estatus=true, string mensaje = "") {
            using SqlConnection conn = new SqlConnection(_connection);
            using SqlCommand cmd = new SqlCommand(_storedProcedure, conn);
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

    }
}
