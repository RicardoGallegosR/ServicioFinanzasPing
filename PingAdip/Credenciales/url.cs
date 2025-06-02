namespace PingAdip.Credenciales {
    public class url {
        private string placa = "ABC1234";
        public string AdeudosFinanzasGet() {
            return $"https://tramites.cdmx.gob.mx/fotocivicas/public/api/consultar-infracciones/{placa}";
        }
        public string CatalogosCarritosGet() {
            return $"https://verificacionvehicular.sedema.cdmx.gob.mx/Listado/";
        }

        public string AdeudosFinanzasPuntosPost() {
            return $"https://tramites.cdmx.gob.mx/fotocivicas/public/api/update-puntos";
        }
    }
}
