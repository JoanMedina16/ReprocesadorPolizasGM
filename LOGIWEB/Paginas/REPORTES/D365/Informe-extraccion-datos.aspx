<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">
                    <li class="">
                        <button class="btn btn-info acceso-dow btn-sm" type="button" id="btnDescarga"><i class="fas fa-file-excel"></i>&nbsp;Descarga información</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-primary acceso-find btn-sm" type="button" onclick="oGlob.m_FormFilters()"><i class="fas fa-filter"></i>&nbsp;Filtros</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 

        <div class="col-12 jqGdatos">           
             <table id="tbl-datos-zap" class="table table-bordered table-hover"></table>
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
                    <!-- <div class="col-4">
                        <label>Diario contable</label>
                       <input class="form-control" id="txtdiario" type="text" placeholder="Captura el diario">
                    </div> -->
                    <div class="col-4">
                        <label>Periodo de desde</label>
                        <input class="form-control" id="txtFechainicio" type="text" placeholder="Captura la fecha de inicio">
                    </div>
                    <div class="col-4">
                        <label>hasta</label>
                        <input class="form-control" id="txtFechafin" type="text" placeholder="Captura la fecha final">
                    </div> 
                    <div class="col-4">
                        <label>Cuenta D365</label>
                       <input class="form-control" id="txtcuenta" type="text" placeholder="Captura la cuenta contable">
                    </div>
                    <div class="col-4">
                        <label>Sucursal D365</label>
                       <input class="form-control" id="txtsucursal" type="text" placeholder="Captura la sucursal">
                    </div> 
                    <div class="col-4">
                        <label>Centro de costo D365</label>
                       <input class="form-control" id="txtcentro" type="text" placeholder="Captura el centro de costo">
                    </div>                    
                    <div class="col-4">
                        <label>Departamento D365</label>
                       <input class="form-control" id="txtdepto" type="text" placeholder="Captura el departamento">
                    </div>
                    <!--<div class="col-4">
                        <label>Documento D365</label>
                       <input class="form-control" id="txtdocumento" type="text" placeholder="Captura el documento">
                    </div>-->



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
    var oExtraccion = null;
    $(document).ready(function () {
        oExtraccion = new ExtraccionZAP();
        oExtraccion.m_InicializaEvents();
    });
</script>
