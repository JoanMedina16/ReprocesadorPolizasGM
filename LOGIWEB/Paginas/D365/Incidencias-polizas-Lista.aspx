<div class="card-body">
    <div class="row">
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">                    
                    <li class="">
                        <button class="btn btn-info btn-sm" type="button" id="btnDescarga" ><i class="fas fa-file-excel"></i>&nbsp;Descargar información</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-secondary btn-sm" type="button" id="btnReincio" hidden="hidden"><i class="fas fa-paper-plane"></i>&nbsp;Procesar póliza(s)</button></li>
                    <li>&nbsp;</li>
                    <li class="">
                        <button class="btn btn-secondary btn-sm" type="button" id="btnReproceso"><i class="fas fa-paper-plane"></i>&nbsp;Reprocesar</button></li>
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
             <table id="tbl-incidencias" class="table table-bordered table-hover"></table>
              <div id="jqGridPager"></div>
        </div>
    </div>
</div>

<div class="modal fade" id="modalReprocesa" tabindex="-100" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Reprocesar</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
				<div class="modal-body" style="overflow-y: auto !important;">
					<div class="row">
						<div class="col-6">
							<label>Tipo de Documento</label>
							<select  class="form-control css-texto" id="cmbprocdocs"></select>
						</div>
					</div>
                    <div class="row">
				        <div class="col-6">
					        <label>Fecha</label>
					        <input class="form-control css-texto" id="txtFechaProcesa" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="dd/MM/yyyy">
				        </div>
                    </div>
				</div>
		    <div class="modal-footer">
			    <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
			    <button class="btn btn-primary acceso-find" type="button" id="btnInsertaPeticion">Reprocesar</button>
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
                            <label>Folio GM</label>
                            <input class="form-control css-texto" id="txtFolio" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Escribe el folio del documento">
                        </div>
                </div>
                <div class="row">                    
                        <div class="col-6">
                            <label>Tipo de documento</label>
                            <select  class="form-control css-texto" id="cmbdocs"></select>
                        </div>
                        <div class="col-6">
                            <label>Documento</label>
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

<div class="modal fade" id="div_modal_xml" tabindex="-1" role="dialog" aria-hidden="true" style="z-index: 10474899 !important">
    <div class="modal-dialog modal-dialog-centered modal-dialog modal-md" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Adjunta documento de tipo CFDI (XML)</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <div class="row">
                    <div class="col-12 flex css-control-add" id="uploadxmlitem">
                        <iframe id="iUploadXML" class="custom_iframe" frameborder="0"  src="Formularios/UPLOAD/UploadFilesXML.html"></iframe>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
                <button class="btn btn-success acceso-add" type="button" id="bntUpload">Adjunta CFDI</button>
                <button class="btn btn-success acceso-add" type="button" id="bntUploadViatico">Adjunta comprobante CFDI</button>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="modalbug" tabindex="-100" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title">Contenido de respuesta enviada/recibida</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body" style="overflow-y: auto !important;">
                    <div class="row">
                    <div class="col-8">
                        <label>Ambiente de trabajo:</label>
                        <input class="form-control" id="txtEndPoint" type="text" readonly disabled placeholder="">
                    </div>

                    <div class="col-4">
                        <label>Fecha de envío:</label>
                        <input class="form-control" id="txtFechaTransacc" type="text" readonly disabled placeholder="">
                    </div>
                    <div class="col-12">
                        <label>Usuario registró:</label>
                        <input class="form-control" id="txtusuario" type="text" readonly disabled value="Windows - Replica automática a través de servicio">
                    </div>
                    <div class="col-12">
                        <label>Mensaje de D365:</label>
                        <textarea class="form-control" id="txtcomentarios" readonly disabled rows="3"></textarea>                        
                    </div>

                        <div class="col-12" style="padding-top: 1em">
                            <div class="col-12" id="div_copiado">
                                <h6 class="text-center"><b>Texto copiado al portapales</b></h6>
                            </div>
                            <div class="card card-primary card-outline card-outline-tabs">
                                <div class="card-header p-0 border-bottom-0">
                                    <ul class="nav nav-tabs" id="custom-tabs-four-tab" role="tablist">
                                        <li class="nav-item">
                                            <a id="tab_send" class="nav-link active"  data-toggle="pill" href="#custom-tabs-four-home" role="tab" aria-controls="custom-tabs-four-home" aria-selected="true">Dato enviado</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link" id="custom-tabs-four-profile-tab" data-toggle="pill" href="#custom-tabs-four-profile" role="tab" aria-controls="custom-tabs-four-profile" aria-selected="false">Respuesta Servidor</a>
                                        </li>
                                    </ul>
                                </div>
                                <div class="card-body">
                                    <div class="tab-content" id="custom-tabs-four-tabContent">
                                        <div class="tab-pane fade show active" id="custom-tabs-four-home" role="tabpanel" aria-labelledby="custom-tabs-four-home-tab">
                                            <i class="fas fa-copy text-right" style="color: lightskyblue !important; font-size: 1.4rem; cursor: pointer" onclick="oIncidencia.m_OnCopyObject('#parent_object')"></i>
                                            <pre style="max-height: 15em !important; overflow-y: auto" id="parent_object"><output id="div_objeto"></output></pre>
                                        </div>
                                        <div class="tab-pane fade" id="custom-tabs-four-profile" role="tabpanel" aria-labelledby="custom-tabs-four-profile-tab">
                                            <i class="fas fa-copy text-right" style="color: lightskyblue !important; font-size: 1.4rem; cursor: pointer" onclick="oIncidencia.m_OnCopyObject('#parent_response')"></i>
                                            <pre style="max-height: 15em !important; overflow-y: auto" id="parent_response"><output id="div_response"></output></pre>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.card -->
                            </div>
                        </div>
                </div>
            </div>
            <div class="modal-footer">
                <button class="btn btn-default" type="button" data-dismiss="modal">Cancelar</button>
            </div>
        </div>
    </div>
</div> 


<script>
    var oIncidencia = null;
    $(document).ready(function () {
        oIncidencia = new Incidencias();
        oIncidencia.m_InicializaEvents();
    });
</script>
