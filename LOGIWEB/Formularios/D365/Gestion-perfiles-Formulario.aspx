<form action="#" autocomplete="off">
    <div class="card card-secondary card-tabs">
        <div class="card-header p-0 pt-1">
            <ul class="nav nav-tabs" id="custom-content-above-tab" role="tablist">
                <li class="nav-item">
                    <a class="nav-link active" id="tab-propiedades" data-toggle="pill" href="#tab-propiedades-div" role="tab" aria-controls="custom-content-above-home" aria-selected="true">Perfil de usuario</a>
                </li>
            </ul>
        </div>
    </div>

    <div class="card-body">

        <div class="row">
            <div class="col-6">
                <label>Captura el perfil o rol</label>
                <input class="form-control css-texto" id="txtFormNombre" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el nombre del perfil">
            </div>
            <div class="col-6" style="display: none">
                <input type="hidden" id="txtPerfilID" />
            </div>
            <div class="col-6">
                <div class="row" id="div_accesos">
                </div>
            </div>

            <div class="col-12">
                <hr>
            </div>
            <div class="col-6">
                <label>Descripción</label>
                <input class="form-control css-texto" id="txtdescrip" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura la descripción del concepto">
            </div>
            <div class="col-3">
                 <label>Rango cuenta desde</label>
                <input class="form-control css-texto" id="txtcuenta_desde" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Rango de cuenta de inicio">
            </div>
            <!--<div class="col-3">
                <label>Rango cuenta hasta</label>
                <input class="form-control css-texto" id="txtcuenta_hasta" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Rango de cuenta fin">
            </div>-->
            <div class="col-3">
                <label>Importe inicial</label>
                <input class="form-control css-texto" id="txtmonto_desde" onkeypress="return oGlob.m_SoloNumeroDecimal(this,event)" type="text" placeholder="Rango de monto inicio">
            </div>
            <div class="col-3">
                <label>Importe final</label>
                <input class="form-control css-texto" id="txtmonto_hasta" onkeypress="return oGlob.m_SoloNumeroDecimal(this,event)" type="text" placeholder="Rango de monto final">
            </div>
            <div class="col-12">
                <div class="row justify-content-end">
                    <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important; margin-top: 1.2em !important;">
                        <li>&nbsp;</li>
                        <li class="">
                            <button class="btn btn-success btn-sm" type="button" id="btnAddconcepto">Agregar matriz</button></li>
                        <li>&nbsp;</li>
                    </ul>
                </div>
            </div>
            <div class="col-12 jqGdatos">
                <table id="tbl-cuentas-matriz" class="table table-bordered table-hover"></table>
                <div id="jqGridPagermatriz"></div>
            </div>

        </div>
    </div>
</form>