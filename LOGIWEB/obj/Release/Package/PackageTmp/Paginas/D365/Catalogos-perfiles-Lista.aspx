<div class="card-body">
    <div class="row">
        <!---->
        <div class="col-12 text-right">
            <div class="row justify-content-end">
                <ul class="list-group list-group-horizontal text-right" style="list-style-type: none !important;">
                    <li class="">
                        <button class="btn btn-success acceso-add btn-sm" type="button" data-title="Nuevo registro de rol" data-key="" data-page="D365/Gestion-perfiles-Formulario" id="btnModAdd"><i class="fas fa-plus"></i>&nbsp;Agregar rol</button></li>
                    <li>&nbsp;</li>
                    <li class=""></li>
                </ul>
            </div>
        </div> 

        <div class="col-12 jqGdatos">           
             <table id="tbl-pefiles" class="table table-bordered table-hover"></table>
              <div id="jqGridPager"></div>
        </div>
    </div>
</div>

<script>
    var oPerfilctrol = null;
    $(document).ready(function () {
        oPerfilctrol = new Perfil();
        oPerfilctrol.m_InicializaEvents();
    });
</script>
