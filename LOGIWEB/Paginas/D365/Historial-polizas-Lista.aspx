<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-danger btn-sm" type="button" id="btnElimina" hidden="hidden"><i class="fas fa-trash-alt"></i>&nbsp;Descartar póliza(s)</button></li>
                    <li>&nbsp;</li>
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
                    <div class="col-6" hidden="hidden">
                            <label>Folio documento</label>
                            <input class="form-control css-texto" id="txtFolio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del documento">
                        </div>
                    <div class="col-6" hidden="hidden">
                            <label>Folio documento D365</label>
                            <input class="form-control css-texto" id="txtFolioD365" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del documento">
                        </div>
                        <div class="col-6">
                            <label>Tipo de documento</label>
                            <select  class="form-control css-texto" id="cmbdocs"></select>
                        </div>
                        <div class="col-6">
                            <label>Documento GM</label>
                            <input class="form-control css-texto" id="txtfolserie" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio y serie de la factura">
                        </div>
                        <div class="col-6" hidden="hidden">
                            <label>Fecha contable desde</label>
                            <input class="form-control css-texto" id="txtFechacinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                        <div class="col-6" hidden="hidden">
                            <label>Hasta</label>
                            <input class="form-control css-texto" id="txtFechacfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                    <div class="col-6">
                            <label>Fecha Log</label>
                            <input class="form-control css-texto" id="txtFechacrinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                        <div class="col-6">
                            <label>Hasta</label>
                            <input class="form-control css-texto" id="txtFechacrfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                    <div class="col-4" hidden="hidden">
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" id="chkEliminado" >
                            <label class="form-check-label" for="chkEliminado">Incluir eliminados</label>
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
    var oHistorial = null;
    $(document).ready(function () {
        oHistorial = new Historicos();
        oHistorial.m_InicializaEvents();
    });
</script>
