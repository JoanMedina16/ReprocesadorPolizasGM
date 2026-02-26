using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
namespace D365API
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [Serializable]
    public class RegistroPoliza
    {
        [XmlElement]
        public string IdPoliza { get; set; }
        [XmlElement]
        public string Estatus { get; set; }
        [XmlElement]
        public DateTime Fecha { get; set; }


        // Hacer que Evento sea opcional para deserialización
        [XmlElement(IsNullable = true)]
        public string Evento
        {
            get;
            set;
        }

        // Campo estático para controlar comportamiento
        private static bool _ignorarEventoAlDeserializar = false;

        // Método para establecer si ignorar Evento
        public static void ConfigurarIgnorarEvento(bool ignorar)
        {
            _ignorarEventoAlDeserializar = ignorar;
        }

        // Este método es llamado por XmlSerializer después de deserializar
        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            if (_ignorarEventoAlDeserializar)
            {
                Evento = null;
            }
        }


        [XmlElement]
        public string NumPoliza { get; set; }

        public RegistroPoliza()
        {
        }

        public RegistroPoliza(string idPoliza, string estatus, DateTime fecha, string evento, string numpoliza)
        {
            IdPoliza = idPoliza;
            Estatus = estatus;
            Fecha = fecha;
            Evento = evento;
            NumPoliza = numpoliza;
        }
    }

    [Serializable]
    [XmlRoot("RegistrosPolizas")]
    public class ListaRegistrosPolizas
    {
        [XmlArray("Polizas")]
        [XmlArrayItem("Poliza")]
        public List<RegistroPoliza> Polizas { get; set; }

        public DateTime FechaCreacion { get; set; }
        public int TotalRegistros { get; set; }

        public ListaRegistrosPolizas()
        {
            Polizas = new List<RegistroPoliza>();
            FechaCreacion = DateTime.Now;
            TotalRegistros = 0;
        }
    }
}
