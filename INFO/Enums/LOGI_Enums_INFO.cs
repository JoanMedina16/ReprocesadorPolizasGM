using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INFO.Enums
{
   public enum eDocumentoPolizas
    {
        [Description("Combustibles costo")]
        [DefaultValue("lm_asientos_detalle")]
        COMBUSTIBLES_COSTO = 1,

        [Description("Combustibles proveedor")]
        [DefaultValue("lm_asientos_detalle")]
        COMBUSTIBLES_PROVEEDOR = 2,

        [Description("Facturacion portes")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        FACTURACION_DE_PORTES = 3,

        [Description("Facturacion varia")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        FACTURACION_VARIA = 4,

        [Description("Notas de credito")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        NOTAS_DE_CREDITO = 5,

        [Description("Refacturación varia")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        REFACTURACION_VARIA = 6,

        [Description("Refacturación viajes")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        REFACTURACION_DE_PORTES = 7,

        [Description("Cancelación de facturas")]
        [DefaultValue("lm_asientos_cxc_detalle")]
        CANCELACION_DE_FACTURAS = 8,

        [Description("Comprobacion de viaticos")]
        [DefaultValue("lm_asientos_gto_detalle")]
        COMPROBACIÓN_DE_VIATICOS = 9,


        [Description("Comprobación mano de obra")]
        [DefaultValue("lm_asientos_mo_detalle")]
        MANO_DE_OBRA = 11,

        [Description("Cancelación mano de obra")]
        [DefaultValue("lm_asientos_mo_detalle")]
        CANCELACION_MANO_DE_OBRA = 12,


        [Description("Carga inicial")]
        [DefaultValue("CI")]
        CARGA_INICIAL = 99,
    }

    public enum eDocumentoTMS
    {
        [Description("SIN_CLASIFICACION")]
        [DefaultValue("SIN_CLASIFICACION")]
        SIN_CLASIFICACION = 0,

        [Description("FACTURACIÓN DE INGRESOS")]
        [DefaultValue("FACTURACIÓN DE INGRESOS")]
        FACTURACION_DE_VIAJES = 1, 

        [Description("NOTAS DE CREDITO")]
        [DefaultValue("NOTAS DE CREDITO")]
        NOTAS_DE_CREDITO = 2,

        [Description("CANCELACIÓN DE INGRESOS")]
        [DefaultValue("CANCELACIÓN DE INGRESOS")]
        CANCELACION_DE_INGRESOS = 3,

        [Description("REGISTRO DE PASIVOS")]
        [DefaultValue("REGISTRO DE PASIVOS")]
        REGISTRO_DE_PASIVOS = 4,

        [Description("DISPERSION DE ANTICIPOS")]
        [DefaultValue("DISPERSION DE ANTICIPOS")]
        DISPERSION_ANTICIPOS = 5,

        [Description("COMPROBACIÓN DE GASTOS")]
        [DefaultValue("COMPROBACIÓN DE GASTOS")]
        COMPROBACION_DE_GASTOS = 6,

        [Description("MANO DE OBRA")]
        [DefaultValue("MANO DE OBRA")]
        REGISTRO_MANO_DE_OBRA = 7,

        [Description("CONSUMO DE COMBUSTIBLES")]
        [DefaultValue("CONSUMO DE COMBUSTIBLES")]
        CONSUMO_DE_COMBUSTIBLES = 8,

    }
    public enum eType
    {
        success = 0,
        warning = 1,
        error = 2,
        proceso = 3
    }
}
