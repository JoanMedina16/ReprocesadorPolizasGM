<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <!--<li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>-->
                    <li class="">
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 
        <div class="col-12">
            <div class="row">                
                <div class="col-2">
                    <div class="row">
                        <div class="col-10">
                            <label>Tipo de plantilla</label>
                            <select class="form-control css-texto" id="cmbtipo">
                                <option value="">Selecciona un tipo de plantilla</option>
                                <option value="1">Dispersión electrónica</option>
                                <option value="2">Dispersión fondo fijo</option>
                            </select>
                        </div>
                        <div class="col-2">
                            <button class="btn btn-primary btn-sm" type="button" id="btncambia" style="margin-top: 2.5em"><i class="fas fa-retweet"></i></button>
                        </div>
                    </div>
                </div>

                <div class="col-3">                    
                    <label>Asistente</label>
                    <input class="form-control css-texto" id="txtAsistente" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el nombre del asistente">
                </div>
                <div class="col-2" style="display:none" id="div_banco">
                    <label>Banco</label>
                    <select class="form-control css-texto" id="cmbbancos"></select>
                </div>
                <div class="col-2" style="display:none" id="div_fondofij">
                    <label>Fondo fijo</label>
                    <select class="form-control css-texto" id="cmbbfondofi"></select>
                </div>
                <div class="col-3">
                    <div class="row">
                        <div class="col-10">
                            <label>Referencia</label>
                            <input class="form-control css-texto" id="txtRef" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe la referencia" maxlength="30">
                        </div>
                        <div class="col-2">
                            <button class="btn btn-primary btn-sm" type="button" id="btnref" style="margin-top:2.5em"><i class="fas fa-money-check-alt"></i></button>
                        </div>
                    </div>
                </div>
                 <div class="col-2 text-right">
                    <button class="btn btn-success btn-sm" type="button" id="btnNuevo" style="margin-top:2.5em"><i class="fas fa-file-excel"></i>&nbsp;Crea plantilla</button>
                </div>
            </div>
        </div>
        <div class="col-12 jqGdatos">           
             <table id="tbl-dispersiones" class="table table-bordered table-hover"></table>
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
                            <label>Folio viaje</label>
                            <input class="form-control css-texto" id="txtFoliovij" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del viaje">
                        </div>
                    <div class="col-6">
                            <label>Nombre operador</label>
                            <input class="form-control css-texto" id="txtOperador" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el nombre del operador">
                        </div>
                        <div class="col-6">
                            <label>RFC del operador</label>
                            <input class="form-control css-texto" id="txtrfc" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el RFC del operador">
                        </div>
                        <div class="col-6">
                            <label>Fecha creación desde</label>
                            <input class="form-control css-texto" id="txtFechacinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                        <div class="col-6">
                            <label>Hasta</label>
                            <input class="form-control css-texto" id="txtFechacfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div> 
                        <div class="col-6">
                            <label>Sucursal</label>
                            <select class="form-control css-texto" id="cmbsuc"></select>
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
    var oDispersion = null;
    $(document).ready(function () {
        oDispersion = new Dispersiones();
        oDispersion.m_InicializaEvents();
    });
</script>
