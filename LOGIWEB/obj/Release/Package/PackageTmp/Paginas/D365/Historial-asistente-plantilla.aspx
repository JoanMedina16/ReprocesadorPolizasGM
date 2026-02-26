<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                                        
                    <li class="">
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 
        <div class="col-12 jqGdatos">           
             <table id="tbl-historiales" class="table table-bordered table-hover"></table>
              <div id="jqGridPager"></div>
        </div>
    </div>
</div>


<div class="modal fade" id="div_modal_commentario" tabindex="-1" role="dialog" aria-hidden="true" style="z-index: 1250 !important">
    <div class="modal-dialog modal-dialog-centered modal-dialog modal-md" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Motivo de cancelación</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-12"><label>Folio documento: </label><input class="form-control css-texto" id="txtpolizafolio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" disabled="disabled" readonly="readonly"></div>
                    <div class="col-12 flex css-control-add">
                        <p>Escribe el motivo de la cancelación de la póliza.<b> Esta acción no revierte el movimiento contable creando en D365</b></p>
                    </div>
                    <div class="col-12">
                        <textarea class="form-control css-texto" id="txtcomments" onkeypress="return oGlob.m_campoEspecialLetras(event)" rows="5" maxlength="500"></textarea>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                <button class="btn btn-danger" type="button" id="bntDescarta">Descartar póliza</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modalfiltro" tabindex="-100" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Filtros de búsqueda</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body" style="overflow-y: auto !important;">
                <div class="row">
                    <div class="col-6">
                            <label>Folio asistente</label>
                            <input class="form-control css-texto" id="txtFolio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del documento">
                        </div>
                    <div class="col-6">
                            <label>Tipo de plantilla</label>
                            <select class="form-control css-texto" id="cmbtipo">
                                <option value="">Selecciona un tipo de plantilla</option>
                                <option value="1">Dispersión electrónica</option>
                                <option value="2">Dispersión fondo fijo</option>
                            </select>
                        </div>                       
                    <div class="col-6">
                            <label>Fecha de creación desde</label>
                            <input class="form-control css-texto" id="txtFechacrinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                        <div class="col-6">
                            <label>Hasta</label>
                            <input class="form-control css-texto" id="txtFechacrfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                    <div class="col-6">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" id="chkEliminado" >
                            <label class="form-check-label" for="chkEliminado">Mostrar solo los eliminados</label>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                <button class="btn btn-primary acceso-find" type="button" id="btnBusca">Buscar</button>
            </div>
        </div>
    </div>
</div>

<script>
    var oHisplantilla = null;
    $(document).ready(function () {
        oHisplantilla = new HistorialPlantilla();
        oHisplantilla.m_InicializaEvents();
    });
</script>
