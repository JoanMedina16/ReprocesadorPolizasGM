<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                    <li class="" >
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 
        <div class="col-12 jqGdatos">           
             <table id="tbl-lista-maestra" class="table table-bordered table-hover"></table>
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
                    <div class="col-4">
                        <label>N° Corporativo</label>
                        <input class="form-control css-texto" id="txtcorporativo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero corporativo">
                    </div>
                    <div class="col-4">
                        <label>N° Economico</label>
                        <input class="form-control css-texto" id="txteconomico" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero economico">
                    </div>
                    <div class="col-4">
                        <label>Estatus</label>
                        <select  class="form-control css-texto" id="cmbestatus">
                            <option value="">Todos</option>
                            <option value="1">Activos</option>
                            <option value="2">Bajas</option>
                        </select>
                    </div>
                    <div class="col-4">
                        <label>Sucursal</label>
                        <select class="form-control css-texto" id="cmbsucursal"></select>
                    </div>
                    <div class="col-4">
                        <label>Centro de costos</label>
                        <select class="form-control css-texto" id="cmbcentro"></select>
                    </div>
                    <div class="col-4">
                        <label>Departamento</label>
                        <select class="form-control css-texto" id="cmbdepto"></select>
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
    var oListamaestra = null;
    $(document).ready(function () {
        oListamaestra = new Listamaestra();
        oListamaestra.m_InicializaEvents();
    });
</script>
