<div class="modal fade" id="modalperiodo" tabindex="-100" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title"><b>Cierre periodo contable</b></h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body" style="overflow-y: auto !important;">
                <div class="row">
                    <div class="col-12"> <p>Esta configuración <b>cierra el periodo actual en OPEAdm</b>. Antes de realizar el movimiento asegurese de haber finalizado toda operación que afecte directamente el cierre ya 
                        que una vez haciendo dicho cierre esta operación no puede ser revertida. Es imporante que el cierre de mes se haga a la par con el de Dynamics 365.</p></div>
                    <div class="col-3">
                            <label>Número de empresa</label>
                            <input class="form-control css-texto" id="txtCia" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero de la empresa" readonly="readonly" disabled="disabled">
                        </div>
                    <div class="col-9">
                            <label>Razón social</label>
                            <input class="form-control css-texto" id="txtNombre" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el nombre de la empresa" readonly="readonly" disabled="disabled">
                        </div>
                    
                    <div class="col-12">
                         <p id="leyendaperiodo"></p>
                    </div>

                    <div class="col-3">
                            <label>Periodo a abrir</label>
                    </div>
                    <div class="col-9">
                            <input class="form-control css-texto" id="txtnuevoperiodo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" readonly="readonly">
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
    var oCierreope = null;
    $(document).ready(function () {
        oCierreope = new Cierreperiodo();
        oCierreope.m_InicializaEvents();
    });
</script>
