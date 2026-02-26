<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                     <li class="">
                        <button class="btn btn-success btn-sm" type="button" id="btnPoliza" ><i class="fas fa-file-invoice"></i>&nbsp;Genera poliza</button></li>
                    <li>&nbsp;</li>
                    <li class="" >
                        <button class="btn btn-primary acceso-find btn-sm" type="button" id="btnFiltro"><i class="fas fa-filter"></i>&nbsp;Buscar</button></li>
                    <li>&nbsp;</li>
                </ul>
            </div>
        </div> 
        <div class="col-12 jqGdatos">           
             <table id="tbl-lista-combustibles" class="table table-bordered table-hover"></table>
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
                            <label>Fecha de creación desde</label>
                            <input class="form-control css-texto" id="txtFechacrinicio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                        <div class="col-4">
                            <label>Hasta</label>
                            <input class="form-control css-texto" id="txtFechacrfin" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
                        </div>
                    <div class="col-4">
                        <label>N° Corporativo</label>
                        <input class="form-control css-texto" id="txtcorporativo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero corporativo">
                    </div>
                    <div class="col-4">
                        <label>N° Economico</label>
                        <input class="form-control css-texto" id="txteconomico" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el numero economico">
                    </div>
                    <div class="col-4">
                        <label>N° Poliza</label>
                        <input class="form-control css-texto" id="txtpoliza" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe la póliza">
                    </div>
                    <div class="col-4">
                        <label>Estatus</label>
                        <select  class="form-control css-texto" id="cmbestatus">
                            <option value="0">Todos</option>
                            <option value="1">Disponibles</option>
                            <option value="2">Registrados</option>
                        </select>
                    </div> 
                    <div class="col-8">
                        <label>Almacén de consumo</label>
                        <select  class="form-control css-texto" id="cmbalmacen">
                        </select>
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



        <div class="modal fade" id="modalimportes" tabindex="-100" role="dialog" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
                <div class="modal-content CuerpoDialogo">
                    <div class="modal-header">
                        <h5 class="modal-title"><b>Creación de póliza consumo de combustible interno</b></h5> 
                    </div>
                    <div class="modal-body" style="overflow-y: auto !important; position: relative; width: 100%;">
                         
                            <div class="col-12 TipEstilo">
                                <p>
                                    Los gastos seleccionados serán programados para ser enviados a Dynamics 365 bajo el folio de asignación automático. Esperar como mínimo <b>cinco minutos</b> posterior a su selección y envío. <b>El tiempo de grabado dependerá de la cantidad de registro(s)</b> seleccionados. Asegurate que la fecha seleccionda sea de un período contable abierto.
                                </p>
                            </div>
                            <div class="col-12">
                                <div class="row">

                                    <div class="col-3 EncCategorizado">
                                        <p><b>Proveedor de estación:</b></p>
                                    </div>
                                    <div class="col-9">
                                        <div class="row">
                                            <div class="col-10">                                                
                                                <input class="form-control css-texto" id="txtEstaciont"  type="text" placeholder="Confirma la estación de combustible" maxlength="16" disabled="disabled"/>                                                
                                            </div>
                                        </div>
                                    </div>

                                        <div class="col-3 EncCategorizado">
                                        <p><b>Fecha de documento:</b></p>
                                    </div>
                                    <div class="col-9">
                                        <div class="row">
                                            <div class="col-10">                                                
                                                <input class="form-control css-texto" id="txtFechadocto"  type="text" placeholder="Selecciona la fecha" maxlength="30" />                                                
                                            </div>
                                        </div>
                                    </div>

                                    <div class="col-12" >
                                        <p>&nbsp;</p>
                                    </div>

                                    <div class="col-10">
                                        <div class="row">
                                            <div class="col-3">Numero de cargas:</div>
                                            <div class="col-8">
                                                <input class="form-control css-texto" id="txtNum"  type="text" placeholder="Escribe los números de cargas" maxlength="16" disabled="disabled"/>                                                
                                            </div>
                                            <div class="col-3">Litros totales:</div>
                                            <div class="col-8">
                                                <input class="form-control css-texto" id="txtLitros"  type="text" placeholder="Escribe los litros de combustibles" maxlength="16" disabled="disabled"/>                                                
                                            </div>
                                            <div class="col-3">Precio combustible:</div>
                                            <div class="col-8">
                                                <input class="form-control css-texto" id="txtPreciocombus"  type="text" placeholder="Escribe el importe del precio del combustible" onkeypress="return filterFloat(event,this);" />                                                
                                            </div>
                                            <div class="col-3">Importes totales:</div>
                                            <div class="col-8">
                                                <input class="form-control css-texto" id="txtImportetot"  type="text" placeholder="Escribe el importe de la provisión" maxlength="16" disabled="disabled"/>                                                
                                            </div>  
                                        </div>
                                    </div>
                            </div>
                            <div class="col-3">
                                 <input type="hidden" id="txtElementos" />
                            </div>
                            <div class="col-9">
                                
                            </div>
                        </div> 
                    
                    <div class="modal-footer">
                        <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                        <button type="button" class="btn btn-success" id="btnAceptapoliza">Aceptar</button>
                    </div>
                </div>
            </div>
        </div>
            </div>

<script>
    var oBitacoraLista = null;
    $(document).ready(function () {
        oBitacoraLista = new Consultabitacora();
        oBitacoraLista.m_InicializaEvents();
    });
</script>
