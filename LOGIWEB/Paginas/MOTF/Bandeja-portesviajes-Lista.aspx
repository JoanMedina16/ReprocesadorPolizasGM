<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-success acceso-add btn-sm" type="button"  id="btnAddcnf" disabled="disabled"><i class="fas fa-plus"></i>&nbsp;Facturación masiva</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 
        <div class="col-12 jqGdatos">           
             <table id="tbl-motorfacturacion" class="table table-bordered table-hover"></table>
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
                    
                        <div class="col-8">
                            <label>Nombre arrendatario</label>
                            <input class="form-control" id="txtNombrearref" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el nombre del arrendatario">
                        </div>
                        <div class="col-4">
                            <label>RFC</label>
                            <input class="form-control css-texto" id="txtrfcarrendaf" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el RFC">
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
    var oBandeja = null;
    $(document).ready(function () {
        oBandeja = new BandejaFacturas();
        oBandeja.m_InicializaEvents();
    });
</script>
