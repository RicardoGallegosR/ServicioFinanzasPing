# ServicioFinanzasPing

Servicio de monitoreo de endpoints HTTP para APIs de finanzas y verificación vehicular. Evalúa disponibilidad y registra resultados en SQL Server mediante un procedimiento almacenado.

## Características

- Verificación de estado `HTTP` de servicios web externos
- Inserción de estado vía SP: `Servicios.spInsertarPing`
- Notificación opcional vía Telegram
- Configuración de conexión por `.env`

## Variables de entorno requeridas

```env
DB_SERVER=
DB_NAME=
DB_USER=
DB_PASS=
