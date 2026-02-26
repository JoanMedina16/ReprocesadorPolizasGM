<div class="card-body">
    <div class="row">
        
        <div class="col-12">
            <!--INICIO INVOICE-->            
            <div class="invoice p-3 mb-3">
                <div class="row encabezadofactura">
                    <div class="col-12" style="color:White !important; margin-top:10px !important;">                                
                        <h5 >Modulo de Emision de factura o remision</h5>                        
                    </div>
                    <div class="col-12  tips-factura">
                        <div class="row">
                            <div class="col-12">
                                <p><img src="../../Imagenes/Info.png" /><b>Tips</b>:Esta ventana creará un nuevo documento para el pedido: LM/MEX.2021009917</p>
                            </div>
                        </div>
                    </div>
                </div>
                <!-- info row -->
                <div class="row invoice-info">
                    <div class="col-sm-12 invoice-col">
                        <b style="color: #EC7D1F">Datos del documento</b><br />
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" value="" id="chkInternacional" disabled="disabled">
                            <label class="form-check-label" for="chkInternacional">
                               <b>Transporte internacional</b>
                            </label>
                        </div>
                    </div>
                </div>
                <div class="row invoice-info">
                    <div class="col-sm-4 invoice-col">
                        Origen
                        <address>
                            <label id="lblorigen"></label><br />
                            <label><b>Operador:&nbsp;</b></label><label id="lbloperador" class="labelclean">Lorem impsum dolor</label><br />
                            <label><b>Salida:&nbsp;</b></label><label id="lblsalida" class="labelclean">Lorem impsum dolor</label><br />
                            <label><b>Unidades:&nbsp;</b></label><label id="lblunidades" class="labelclean">Lorem impsum dolor</label><br />
                        </address>
                    </div>
                    <!-- /.col -->
                    <div class="col-sm-4 invoice-col">
                        Destino                
                        <address>
                            <label id="lbldestino"></label>
                            <br />
                            <label><b>Llegada:&nbsp;</b></label><label id="lbllegada" class="labelclean">Lorem impsum dolor</label><br />
                            <label><b>Distancia:&nbsp;</b></label><label id="lbldistancia" class="labelclean">Lorem impsum dolor</label><br />
                        </address>
                    </div>
                    <!-- /.col -->
                    <div class="col-sm-4 invoice-col">
                         <address>
                            <b>Folio del siguiente documento:&nbsp; <label id="lblsiguientefolio" style="color:red">0000000</label></b><br />
                             <b>Folio electrónico:&nbsp; <label id="lblfolioelectronico" style="color:red">000000000</label></b><br />                            
                             <label><b>Cliente:&nbsp;</b></label><label id="lblCliente" class="labelclean">Lorem impsum dolor</label><br />
                              <label><b>Servicio:&nbsp;</b></label><label id="lblServicio" class="labelclean">Lorem impsum dolor</label><br />
                            <label><b>Clave de transporte:&nbsp;</b></label><br />
                        </address> 
                    </div>
                    <!-- /.col -->
                </div>
                <!-- /.row -->

                <!-- Table row -->

                <div class="card card-secondary card-outline">
                    <!--<div class="card-header">
                <h3 class="card-title">
                    <i class="fas fa-edit"></i> Información del documento
            </h3>
            </div>-->
                    <div class="card-body">
                        <!--<h4>Factura</h4>-->
                        <div class="row">
                            <div class="col-5 col-sm-3">
                                <div class="nav flex-column nav-tabs h-100" id="vert-tabs-tab" role="tablist" aria-orientation="vertical">
                                    <a class="nav-link" id="vert-tabs-home-tab" data-toggle="pill" href="#vert-tabs-home" role="tab" aria-controls="vert-tabs-home" aria-selected="true">Conceptos a facturar</a>
                                    <a class="nav-link active" id="vert-tabs-profile-tab" data-toggle="pill" href="#vert-tabs-profile" role="tab" aria-controls="vert-tabs-profile" aria-selected="false">Productos seleccionados</a>
                                    <a class="nav-link" id="vert-tabs-messages-tab" data-toggle="pill" href="#vert-tabs-messages" role="tab" aria-controls="vert-tabs-messages" aria-selected="false">Datos adicionales</a>
                                    <a class="nav-link" id="vert-tabs-settings-tab" data-toggle="pill" href="#vert-tabs-settings" role="tab" aria-controls="vert-tabs-settings" aria-selected="false">Comentarios & tarifa del cliente</a>
                                    <a class="nav-link" id="vert-tabs-totales-tab" data-toggle="pill" href="#vert-tabs-totales" role="tab" aria-controls="vert-tabs-settings" aria-selected="false">Resumen de totales</a>


                                </div>
                            </div>
                            <div class="col-7 col-sm-9">
                                <div class="tab-content" id="vert-tabs-tabContent">
                                    <div class="tab-pane text-left fade " id="vert-tabs-home" role="tabpanel" aria-labelledby="vert-tabs-home-tab">
                                        <div class="row">
                                            <div class="col-12 jqGdatos">
                                                <table id="tbl-conceptos" class="table table-bordered table-hover"></table>
                                                <div id="jqGridPconcept"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade show active" id="vert-tabs-profile" role="tabpanel" aria-labelledby="vert-tabs-profile-tab">
                                        <div class="row">
                                            <div class="col-12 jqGdatos">
                                                <table id="tbl-productos" class="table table-bordered table-hover"></table>
                                                <div id="jqGridProduct"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade" id="vert-tabs-messages" role="tabpanel" aria-labelledby="vert-tabs-messages-tab">
                                        <div class="row">
                                            <div class="col-12 jqGdatos">
                                                <table id="tbl-datosadicionales" class="table table-bordered table-hover"></table>
                                                <div id="jqGridPdata"></div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="tab-pane fade" id="vert-tabs-settings" role="tabpanel" aria-labelledby="vert-tabs-settings-tab">

                                        <div class="row">
                                             <div class="col-12">
                                                 <p class="h6"><b>Comentarios:</b></p>
                                                <textarea class="form-control" id="txtcomentarios" rows="3"></textarea>
                                            </div>
                                        </div>

                                        <div class="row" style="padding-top:5px !important">
                                            <!-- accepted payments column -->                                            
                                            <!-- /.col -->
                                            <div class="col-6">&nbsp;</div>
                                            <div class="col-6">
                                                <p class="h5">Detalles de la tarifa del cliente.</p>
                                                <div class="row">                                                    
                                                    <div class="col-12">
                                                        <hr />
                                                        <div class="row">
                                                            <div class="col-3">
                                                                <label>Tarifa autorizada:</label>
                                                            </div>
                                                            <div class="col-9">
                                                                <label class="labelclean">$000,000.0</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="col-12">
                                                        <hr />
                                                        <div class="row">
                                                            <div class="col-3">
                                                                <label>Limite de credito:</label>
                                                            </div>
                                                            <div class="col-9">
                                                                <label class="labelclean">$000,000.0</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                     <div class="col-12">
                                                        <hr />
                                                        <div class="row">
                                                            <div class="col-3">
                                                                <label>Credito disponible:</label>
                                                            </div>
                                                            <div class="col-9">
                                                                <label class="labelclean">$000,000.0</label>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <!-- /.col -->
                                        </div>
                                    </div>

                                    <div class="tab-pane fade" id="vert-tabs-totales" role="tabpanel" aria-labelledby="vert-tabs-totales-tab">

                                        <!-------------------->
                                        <div class="row">                                            
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>Moneda:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">Naciona: (1)</label></div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>Importe:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">$000,000.0</label></div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>IVA:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">$000,000.0</label></div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>Retencion del 4%:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">$000,000.0</label></div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>Total MXN:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">$000,000.0</label></div>
                                                </div>
                                            </div>
                                            <div class="col-12">
                                                <hr />
                                                <div class="row">
                                                    <div class="col-3">
                                                        <label>Total USD:</label></div>
                                                    <div class="col-9">
                                                        <label class="labelclean">$000,000.0</label></div>
                                                </div>
                                            </div>
                                        </div>
                                        <!-------------->
                                    </div>


                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- /.row -->


                <!-- this row will not appear when printing -->
                <div class="row no-print">
                    <div class="col-12 text-right">
                        <b>Total importe factura:</b>
                        <label id="lblTotalgroup" class="labelclean"></label>
                    </div>
                </div>
                <div class="row no-print">
                    <div class="col-12">
                        <button class="btn btn-primary float-left" type="button" id="btnValidaCFDI">Validación CFDI</button>
                        <button class="btn btn-default float-right" type="button" id="btnCancela" data-title="Bandeja de facturación" data-lvuno="Motor de facturaci&oacute;n" data-lvdos="" data-empresa="0" data-type="0" data-page="MOTF/Bandeja-portesviajes-Lista" onclick="oGlob.m_CargaFormulario(this)" style="margin-left:4px">Cancelar</button>
                        <button class="btn btn-success float-right" type="button" id="btnFacturar" >Generar factura</button>
                    </div>
                </div>
            </div>
            <!--FIN-DEINVOICE-->
        </div>
    </div>
</div>