using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Tablas
{
    public class LOGI_Usuarios_INFO
    {
        public string sServer { get; set; }
        public string sDatabase { get; set; }
        public int iUsuario { get; set; }
        public int iUsuariocrea { get; set; }
        public string sUsuario { get; set; }
        public int sUsuarioExcep { get; set; }
        public string sNickName { get; set; }
        public string sNombre { get; set; }
        public string sApellidos { get; set; }
        public string sCorreo { get; set; }
        public string sContrasenia { get; set; }
        public int iSucursal { get; set; }
        public string sSucursal { get; set; }
        public int iPerfil { get; set; }
        public int iActivo { get; set; }
        public int iEliminado { get; set; }
        public int iAdministrador { get; set; }
        public string sPermisosmod { get; set; }
        public string sPermisodiario { get; set; }
        public string sFechaalta { get; set; }
        public DateTime dtFechAlta { get; set; }
        public DateTime dtFechaUltModificacion { get; set; }
        public DateTime dtFechaBaja { get; set; }
        public int ctcentro { get; set; }
        public int subctcentro { get; set; }
        public string centrocosto { get; set; }
    }
}
