using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace D365API
{
    public class GestorPolizas
    {
        private string directorioBase;
        private string archivoBitacoraActual;
        private string archivoProcesadosActual;
        private ListaRegistrosPolizas registrosBitacora;
        private ListaRegistrosPolizas registrosProcesados;
        private readonly ReaderWriterLockSlim fileLock = new ReaderWriterLockSlim();

        public GestorPolizas()
        {
            directorioBase = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Polizas");
            Directory.CreateDirectory(directorioBase);
        }

        // Método principal para agregar registros a la bitácora
        public void AgregarRegistro(string idPoliza, string estatus, DateTime fecha,
                                   string evento, string folio, string numPoliza, DateTime fechaArchivo)
        {
            try
            {
                fileLock.EnterWriteLock();

                // Determinar archivo de bitácora por fecha
                string archivoBitacora = ObtenerArchivoBitacora(fechaArchivo);

                // Si cambia el archivo, cargarlo
                if (archivoBitacoraActual != archivoBitacora || registrosBitacora == null)
                {
                    CargarBitacora(archivoBitacora);
                }

                // Crear nuevo registro
                var nuevoRegistro = new RegistroPoliza(idPoliza, estatus, fecha,
                    $"Folio: {folio} {evento}", numPoliza);

                // Evitar duplicados exactos
                if (!registrosBitacora.Polizas.Any(p => p.IdPoliza == idPoliza &&
                                                       p.Evento == nuevoRegistro.Evento &&
                                                       p.Fecha == fecha))
                {
                    registrosBitacora.Polizas.Add(nuevoRegistro);
                    registrosBitacora.TotalRegistros = registrosBitacora.Polizas.Count;
                    GuardarBitacora();
                }
            }
            finally
            {
                if (fileLock.IsWriteLockHeld) fileLock.ExitWriteLock();
            }
        }

        // Versión async
        public async Task AgregarRegistroAsync(string idPoliza, string estatus, DateTime fecha,
                                              string evento, string folio, string numPoliza, DateTime fechaArchivo  )
        {
            await Task.Run(() => AgregarRegistro(idPoliza, estatus, fecha, evento, folio, numPoliza, fechaArchivo));
        }

        // Cargar bitácora por fecha
        public void CargarBitacora(DateTime fecha)
        {
            CargarBitacora(ObtenerArchivoBitacora(fecha));
        }

        private void CargarBitacora(string archivo)
        {
            archivoBitacoraActual = archivo;
            registrosBitacora = CargarArchivo(archivo);
        }

        // Cargar procesados por fecha
        public void CargarProcesados(DateTime fecha)
        {
            string archivo = ObtenerArchivoProcesados(fecha);
            archivoProcesadosActual = archivo;
            registrosProcesados = CargarArchivo(archivo);
        }

        // Método genérico para cargar cualquier archivo
        private ListaRegistrosPolizas CargarArchivo(string archivo)
        {
            if (!File.Exists(archivo))
                return new ListaRegistrosPolizas();

            if (new FileInfo(archivo).Length == 0)
                return new ListaRegistrosPolizas();

            try
            {
                var settings = new XmlReaderSettings { CheckCharacters = false };
                var serializer = new XmlSerializer(typeof(ListaRegistrosPolizas));

                using (var reader = XmlReader.Create(archivo, settings))
                {
                    return (ListaRegistrosPolizas)serializer.Deserialize(reader);
                }
            }
            catch
            {
                return new ListaRegistrosPolizas();
            }
        }

        // Guardar bitácora actual
        private void GuardarBitacora()
        {
            GuardarArchivo(archivoBitacoraActual, registrosBitacora);
        }

        // Guardar procesados actuales
        private void GuardarProcesados()
        {
            GuardarArchivo(archivoProcesadosActual, registrosProcesados);
        }

        // Método genérico para guardar
        private void GuardarArchivo(string archivo, ListaRegistrosPolizas registros)
        {
            if (string.IsNullOrEmpty(archivo)) return;

            var serializer = new XmlSerializer(typeof(ListaRegistrosPolizas));
            using (var writer = new StreamWriter(archivo))
            {
                serializer.Serialize(writer, registros);
            }
        }

        // Obtener nombre de archivo de bitácora por fecha
        private string ObtenerArchivoBitacora(DateTime fecha)
        {
            return Path.Combine(directorioBase, $"Bitacora_{fecha:yyyyMMdd}.xml");
        }

        // Obtener nombre de archivo de procesados por fecha
        private string ObtenerArchivoProcesados(DateTime fecha)
        {
            return Path.Combine(directorioBase, $"Procesados_{fecha:yyyyMMdd}.xml");
        }

        // Obtener registros con estatus "1" de la bitácora actual
        public List<GMTApi.RespuestaPoliza> ObtenerCorrectos()
        {
            try
            {
                fileLock.EnterReadLock();

                if (registrosBitacora == null)
                    return new List<GMTApi.RespuestaPoliza>();

                return registrosBitacora.Polizas
                    .Where(p => p.Estatus == "1")
                    .Select(p => new GMTApi.RespuestaPoliza
                    {
                        IdPoliza = p.IdPoliza,
                        Estatus = p.Estatus
                    })
                    .ToList();
            }
            finally
            {
                if (fileLock.IsReadLockHeld) fileLock.ExitReadLock();
            }
        }

        // Mover registros de bitácora a procesados (eliminar de bitácora)
        public bool MoverAPprocesados(List<GMTApi.RespuestaPoliza> polizasMover, DateTime fecha)
        {
            try
            {
                fileLock.EnterWriteLock();

                // Cargar bitácora de la fecha
                CargarBitacora(fecha);

                // Cargar procesados de la misma fecha
                CargarProcesados(fecha);

                if (polizasMover == null || polizasMover.Count == 0)
                    return false;

                //var idsAMover = polizasMover.Select(p => p.IdPoliza).ToHashSet();
                var idsAMover = new HashSet<string>(polizasMover.Select(p => p.IdPoliza));

                // Encontrar registros a mover
                var registrosAMover = registrosBitacora.Polizas
                    .Where(p => idsAMover.Contains(p.IdPoliza))
                    .ToList();

                if (registrosAMover.Count == 0)
                    return false;

                // Mover a procesados
                registrosProcesados.Polizas.AddRange(registrosAMover);
                registrosProcesados.TotalRegistros = registrosProcesados.Polizas.Count;

                // Eliminar de bitácora
                registrosBitacora.Polizas.RemoveAll(p => idsAMover.Contains(p.IdPoliza));
                registrosBitacora.TotalRegistros = registrosBitacora.Polizas.Count;

                // Guardar ambos archivos
                GuardarBitacora();
                GuardarProcesados();

                return true;
            }
            finally
            {
                if (fileLock.IsWriteLockHeld) fileLock.ExitWriteLock();
            }
        }

        // Buscar en bitácora por rango de fechas
        public List<RegistroPoliza> BuscarEnBitacora(DateTime inicio, DateTime fin)
        {
            return BuscarEnArchivos(inicio, fin, "Bitacora_");
        }

        // Buscar en procesados por rango de fechas
        public List<RegistroPoliza> BuscarEnProcesados(DateTime inicio, DateTime fin)
        {
            return BuscarEnArchivos(inicio, fin, "Procesados_");
        }

        private List<RegistroPoliza> BuscarEnArchivos(DateTime inicio, DateTime fin, string prefijo)
        {
            var resultados = new List<RegistroPoliza>();

            for (DateTime fecha = inicio.Date; fecha <= fin.Date; fecha = fecha.AddDays(1))
            {
                string archivo = Path.Combine(directorioBase, $"{prefijo}{fecha:yyyyMMdd}.xml");

                if (!File.Exists(archivo)) continue;

                try
                {
                    var registrosDia = CargarArchivoTemporal(archivo);
                    resultados.AddRange(registrosDia.Where(p => p.Fecha.Date == fecha.Date));
                }
                catch { }
            }

            return resultados;
        }

        private List<RegistroPoliza> CargarArchivoTemporal(string archivo)
        {
            var registros = CargarArchivo(archivo);
            return registros.Polizas;
        }

        // Información de archivos actuales
        public string InfoActual()
        {
            string info = "Archivos cargados:\n";

            if (!string.IsNullOrEmpty(archivoBitacoraActual))
                info += $"Bitácora: {Path.GetFileName(archivoBitacoraActual)} " +
                       $"({registrosBitacora?.TotalRegistros ?? 0} registros)\n";

            if (!string.IsNullOrEmpty(archivoProcesadosActual))
                info += $"Procesados: {Path.GetFileName(archivoProcesadosActual)} " +
                       $"({registrosProcesados?.TotalRegistros ?? 0} registros)";

            return info;
        }

        // Listar archivos de bitácora
        public List<string> ListarBitacoras()
        {
            return ListarArchivos("Bitacora_*.xml");
        }

        // Listar archivos de procesados
        public List<string> ListarProcesados()
        {
            return ListarArchivos("Procesados_*.xml");
        }

        private List<string> ListarArchivos(string patron)
        {
            return Directory.Exists(directorioBase)
                ? Directory.GetFiles(directorioBase, patron)
                          .Select(Path.GetFileName)
                          .ToList()
                : new List<string>();
        }

        // Limpiar bitácora actual
        public void LimpiarBitacora()
        {
            try
            {
                fileLock.EnterWriteLock();
                if (registrosBitacora != null)
                {
                    registrosBitacora = new ListaRegistrosPolizas();
                    GuardarBitacora();
                }
            }
            finally
            {
                if (fileLock.IsWriteLockHeld) fileLock.ExitWriteLock();
            }
        }

        // Limpiar procesados actuales
        public void LimpiarProcesados()
        {
            try
            {
                fileLock.EnterWriteLock();
                if (registrosProcesados != null)
                {
                    registrosProcesados = new ListaRegistrosPolizas();
                    GuardarProcesados();
                }
            }
            finally
            {
                if (fileLock.IsWriteLockHeld) fileLock.ExitWriteLock();
            }
        }

        public List<GMTApi.RespuestaPoliza> MostrarRegistrosCorrectos(DateTime date)
        {
            try
            {
                fileLock.EnterReadLock();

                // Cargar bitácora del día actual si no está cargada
                if (registrosBitacora == null || string.IsNullOrEmpty(archivoBitacoraActual))
                {
                    CargarBitacora(date);
                }

                // Versión con LINQ (más compacta)
                return registrosBitacora.Polizas
                    .Where(p => p.Estatus == "1")
                    .Select(p => new GMTApi.RespuestaPoliza
                    {
                        IdPoliza = p.IdPoliza,
                        Estatus = p.Estatus
                    })
                    .ToList();
            }
            finally
            {
                if (fileLock.IsReadLockHeld)
                    fileLock.ExitReadLock();
            }
        }
    }
}
