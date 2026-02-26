<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-success btn-sm" type="button" id="btnAgrega" data-title="Nuevo registro de usuario" data-page="D365/Gestion-usuarios-Formulario" ><i class="fas fa-user"></i>&nbsp;Nuevo usuario</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div>         
        <div class="col-12 jqGdatos">           
             <table id="tbl-usuarios" class="table table-bordered table-hover"></table>
              <div id="jqGridPager"></div>
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
                        <label>Folio documento D365</label>
                        <input class="form-control css-texto" id="txtFolioD365" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del documento">
                    </div>
                    <div class="col-6">
                        <label>Folio y serie</label>
                        <input class="form-control css-texto" id="txtfolserie" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio y serie de la factura">
                    </div>
                    <div class="col-6">
                        <label>Fecha contable desde</label>
                        <input class="form-control css-texto" id="txtFechacinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                    </div>
                    <div class="col-6">
                        <label>Hasta</label>
                        <input class="form-control css-texto" id="txtFechacfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                    </div>
                    <div class="col-6">
                        <label>Fecha de creación desde</label>
                        <input class="form-control css-texto" id="txtFechacrinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                    </div>
                    <div class="col-6">
                        <label>Hasta</label>
                        <input class="form-control css-texto" id="txtFechacrfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
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
    var oGestusuario = null;
    $(document).ready(function () {
        oGestusuario = new Usuarios();
        oGestusuario.m_InicializaEvents();
    });
</script>
