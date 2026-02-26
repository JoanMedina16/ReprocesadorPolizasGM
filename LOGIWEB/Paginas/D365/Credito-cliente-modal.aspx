<div class="modal fade" id="modalcredito" tabindex="-100" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"><b>Configura disponibilidad de crédito</b></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body" style="overflow-y: auto !important;">
                <div class="row">
                    <div class="col-12"> <p>Esta configuración permite <b>activar o desactivar</b> la comprobación de disponibilidad de crédito del cliente. 
                        Cada vez que se emita una factura a través del "Motor de Facturación" y esta opción esté activa, se validará el total de la factura vs la disponibilida de crédito en Dynamics 365.</p></div>
                    <div class="col-3">
                            <label>Número de empresa</label>
                            <input class="form-control css-texto" id="txtCia" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero de la empresa" readonly="readonly" disabled="disabled">
                        </div>
                    <div class="col-9">
                            <label>Razón social</label>
                            <input class="form-control css-texto" id="txtNombre" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el nombre de la empresa" readonly="readonly" disabled="disabled">
                        </div>                    
                        <div class="col-12">
                            &nbsp;
                            <div class="form-check">
                                <input class="form-check-input" type="checkbox" id="chksaldo">
                                <label class="form-check-label" for="chksaldo"><b>Habilita la verificación de crédito del cliente</b></label>
                            </div>
                        </div>
                </div>
            </div>
            <div class="modal-footer">                
                <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                <button class="btn btn-success" type="button" id="btnGuarda">Guardar</button>
            </div>
        </div>
    </div>
</div>

<script>
    var oCredito = null;
    $(document).ready(function () {
        oCredito = new Creditocliente();
        oCredito.m_InicializaEvents();
    });
</script>
