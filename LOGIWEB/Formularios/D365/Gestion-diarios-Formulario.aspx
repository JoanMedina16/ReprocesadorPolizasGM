<form action="#" autocomplete="off">
<div class="card-body">
    <div class="row">        
        <div class="col-2">
            <label>N° de documento</label>
            <input class="form-control css-texto" id="txtnumero" onkeypress="return oGlob.m_soloNumeros(event)" type="text" placeholder="Captura el número de documento">
        </div>   
        <div class="col-6">
            <label>Nombre del documento contable</label>
            <input class="form-control css-texto" id="txtnombre" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el nombre del documento">
        </div>
        <div class="col-4">
            <label>Nombre del diario</label>
            <input class="form-control css-texto" id="txtdiario" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el nombre del diario">
        </div>
        <div class="col-4">
            <label>Método de ambiente</label>
            <input class="form-control css-texto" id="txtmetodo" onkeypress="return oGlob.m_campoEspecialLetras(event)" type="text" placeholder="Captura el método para la URL">
        </div>
        <div class="col-3">
            <br />&nbsp;
            <div class="form-check">
                <input class="form-check-input" type="checkbox" id="chkactivo" checked>
                <label class="form-check-label" for="chkactivo">Documento activo</label>
            </div>
        </div>         
    </div>
</div>
</form>