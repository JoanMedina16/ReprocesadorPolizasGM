using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365API.INFOD
{
    public class LOGI_Credencial_D365
    {
        /// <summary>
        /// Representa la URL/Endpoint hacia el cual se realiza la peticion
        /// </summary>
        public string api { get; set; }
        /// <summary>
        /// Tipo de proceso a ejecutar al realizar la accion en el body
        /// </summary>
        public string grant_type { get; set; }
        /// <summary>
        /// Identificador del usuario conectado a Azure (valor constante)
        /// </summary>
        public string client_id { get; set; }
        /// <summary>
        /// Credeencial del usuario que se loguea para crear documentos
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// Credencial del usuario que se loguea para crear documentos
        /// </summary>
        public string password { get; set; }       
        /// <summary>
        /// Resurce hace referencia al ambiente de pruebas, sandbox o producción
        /// </summary>
        public string resource { get; set; }
        /// <summary>
        /// Valor utilizado para emitir el login de AZURE 
        /// </summary>
        public string api_login { get; set; }

        public string ciad365 { get; set; }
        public string aprobador { get; set; }

        public string cuentaviaticos { get; set; }
    }
}
